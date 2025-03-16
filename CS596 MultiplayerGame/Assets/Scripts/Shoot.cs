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
    [SerializeField] public AudioClip bulletFX;

    //private NetworkVariable<Transform> bulletTransform = new NetworkVariable<Transform>();

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
        
        // Locally process the rotation 
        Vector3 rotation = mousePos - transform.position;
        //Vector2 direction = (mousePos - bulletTransform.position).normalized;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        // Check to see if player can fire
        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
                //if (Input.GetMouseButton(0) && canFire)
                //{
                //    // Obtain the direction 
                //    Vector2 direction = (mousePos - bulletTransform.Value.position).normalized;
                //    ShootServerRpc(bulletTransform.Value.position, direction);
                //}
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

    [Rpc(SendTo.Server)]
    void ShootServerRpc(Vector2 spawnPos, Vector2 direction)
    {
        GameObject bulletObj = Instantiate(bullet, spawnPos, Quaternion.identity);
        bulletObj.GetComponent<NetworkObject>().Spawn();

        Bullet bulletComp = bulletObj.GetComponent<Bullet>();
        bulletComp.InitializeBullet(direction);
        SoundManager.instance.PlaySound(bulletFX, transform, .5f);
    }
}