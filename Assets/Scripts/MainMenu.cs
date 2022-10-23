using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject chooseMode;
    public GameObject playButton;
    public GameObject chooseDifficulty;
    
    public GameObject turretHolder;

    public GameObject EnemySpawnManager;
    
    public GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    
    public void Play()
    {
        playButton.SetActive(false);
        chooseMode.SetActive(true);
    }

    public void WithEnemies()
    {
        gameManager.withEnemies = true;
        gameManager.StartGame(1);
        EnemySpawnManager.SetActive(true);
    }
    
    public void WithTurret()
    {
        gameManager.withEnemies = false;
        chooseDifficulty.SetActive(true);
        chooseMode.SetActive(false);
    }
}
