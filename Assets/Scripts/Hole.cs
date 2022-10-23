using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public int holeValue;

    private ScoreManager scoreManager;
    private GameObject holeIndicator;
    private PlayerController player;
    
    // Start is called before the first frame update
    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        player = FindObjectOfType<PlayerController>();
        holeIndicator = GameObject.Find("Hole Indicator");

        holeIndicator.transform.position = new Vector3(transform.position.x, -10.4f,
            transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Score(holeValue));
        }
    }

    IEnumerator Score(int scoreToAdd)
    {
        yield return new WaitForSeconds(1f);
        scoreManager.AddToScore(scoreToAdd);
        player.playerAudio.PlayOneShot(player.pointSound, 0.45f);
    }
}
