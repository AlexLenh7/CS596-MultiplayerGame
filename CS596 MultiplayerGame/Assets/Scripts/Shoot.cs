using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Shoot : NetworkBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    public GameObject bullet;
    public Transform bulletTransform;
    public bool canFire;
    private float timer;
    public float timeBetweenFiring;

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        // Only process input for the local player
        if (!IsOwner) return;

        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Ensure we're working in the 2D plane

        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
            }
        }

        if (Input.GetMouseButton(0) && canFire)
        {
            canFire = false;
            // Send the direction to the server
            Vector2 direction = (mousePos - bulletTransform.position).normalized;
            ShootServerRpc(bulletTransform.position, direction);
        }
    }

    [ServerRpc]
    void ShootServerRpc(Vector3 spawnPos, Vector2 direction)
    {
        // Server spawns the bullet
        GameObject bulletObj = Instantiate(bullet, spawnPos, Quaternion.identity);
        
        // Get the NetworkObject component
        NetworkObject netObj = bulletObj.GetComponent<NetworkObject>();
        if (netObj == null)
        {
            Debug.LogError("Bullet prefab missing NetworkObject component!");
            Destroy(bulletObj);
            return;
        }
        
        // First spawn the bullet on the network
        netObj.Spawn();
        
        // Initialize the bullet's movement AFTER spawning
        Bullet bulletComponent = bulletObj.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.shooterClientId = OwnerClientId;
            bulletComponent.InitializeBullet(direction);
        }
        else
        {
            Debug.LogError("Bullet prefab missing Bullet component!");
        }
    }
}