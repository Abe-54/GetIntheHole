using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Rigidbody bulletPrefab;
    public GameObject shootingPos;
    
    public float shootRate = 3.0f;
    public float timeBetweenShots = 1f;
    public float bulletSpeed = 5f;

    private PlayerController player;
    private GameManager gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        Rigidbody bullet = Instantiate(bulletPrefab, shootingPos.transform.position, bulletPrefab.transform.rotation);
        Vector3 lookDirection = (player.transform.position - bullet.transform.position).normalized;
        bullet.velocity = lookDirection * bulletSpeed;
        Destroy(bullet.gameObject, 3f);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject, 0.2f);
        }
    }

    public void StartShoot()
    {
        InvokeRepeating("Shoot", timeBetweenShots, shootRate);
    }
}
