using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 3.0f;

    public AudioSource audioSource;
    public AudioClip hurtSound;
    
    private Rigidbody enemyRb;
    private PlayerController player;
    private ScoreManager scoreManager;
    private GameManager gameManager;

    private Vector3 lookDirection;
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        player = FindObjectOfType<PlayerController>();
        scoreManager = FindObjectOfType<ScoreManager>();
        gameManager = FindObjectOfType<GameManager>();
        speed = gameManager.enemyMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed);
        
        //Enemy destroy when y = -10

        if (transform.position.y <= -10.8)
        {
            audioSource.PlayOneShot(hurtSound, 1f);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, lookDirection);
    }
}
