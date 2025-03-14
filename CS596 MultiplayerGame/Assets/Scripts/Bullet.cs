using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Bullet : NetworkBehaviour
{
    public float force = 20f;
    [HideInInspector]
    public ulong shooterClientId;
    
    private Rigidbody2D rb;
    
    // Network variable to sync position
    private NetworkVariable<Vector3> netPosition = new NetworkVariable<Vector3>();
    // Network variable to sync rotation
    private NetworkVariable<Quaternion> netRotation = new NetworkVariable<Quaternion>();
    
    public override void OnNetworkSpawn()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (IsServer)
        {
            // Server updates the network variables based on physics
            netPosition.Value = transform.position;
            netRotation.Value = transform.rotation;
        }
        else
        {
            // Clients update their transform based on network variables
            transform.position = netPosition.Value;
            transform.rotation = netRotation.Value;
        }
    }
    
    // This is called by the Shoot script on the server
    public void InitializeBullet(Vector2 direction)
    {
        if (!IsServer) return;
        
        rb.linearVelocity = direction * force;
        
        // Set rotation based on direction
        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);
    }
    
    void OnBecameInvisible()
    {
        // Only destroy on the server, and only if the NetworkObject is spawned
        if (IsServer && IsSpawned)
        {
            NetworkObject.Despawn();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Only let the server handle collision
        if (!IsServer) return;
        
        if (collision.CompareTag("Ground") || collision.CompareTag("Player"))
        {
            if (IsSpawned)
            {
                NetworkObject.Despawn();
            }
        }
    }
}