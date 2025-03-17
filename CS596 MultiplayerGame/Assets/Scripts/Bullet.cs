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

    private NetworkVariable<Vector2> netDirection = new NetworkVariable<Vector2>();

    void Awake()
    {
        // Get the rigidbody bullet component
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    // Send the rigidbody velocity over to every client
    [ClientRpc]
    private void SetVelocityClientRpc(Vector2 dir)
    {
        rb.linearVelocity = dir * force;
        float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);
    }

    // This is called by the Shoot script on the server
    public void InitializeBullet(Vector2 direction)
    {
        rb.linearVelocity = direction * force;

        // Also tell every client to set that same velocity
        SetVelocityClientRpc(direction);
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

        // Despawn the object if either the ground or player collides with bullet
        if (collision.CompareTag("Ground") || collision.CompareTag("Player"))
        {
            if (IsSpawned)
            {
                NetworkObject.Despawn();
            }
        }
    }
}