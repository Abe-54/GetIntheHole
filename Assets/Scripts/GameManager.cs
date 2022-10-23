using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isGameActive = true;
    public bool finishedReset = false;
    public bool reachedBottom = false;
    public bool withEnemies = false;

    public List<GameObject> MainMenuItems;
    public List<GameObject> inGameUIItems;
    public GameObject GameOverScreen;
    public TMP_Text finalScoreText;

    public Spinning turretHolder;

    [Min(0.01f)]
    public float fallSpeed = 3;
    [Min(0.01f)]
    public float maxFallSpeed = 0.05f;
    [Min(0.01f)]
    public float difficultyIncrease = 0.05f;
    public float spawnRate = 1;
    public float maxPlayerSpeed = 30;
    public int waveNumber = 1;
    public float enemyMoveSpeed = 3.0f;

    private Vector3 orginalPos;
    private Vector3 holeIndicatorOriginalPos;
    
    private PlayerController player;
    private BlocksSpawner layerSpawner;
    private EnemySpawnManager enemySpawnManager;
    private ScoreManager scoreManager;
    private Turret turret;
    private GameObject holeIndicator;

    // Start is called before the first frame update
    void Start()
    {
        layerSpawner = FindObjectOfType<BlocksSpawner>();
        turret = FindObjectOfType<Turret>();
        turretHolder = FindObjectOfType<Spinning>();
        player = FindObjectOfType<PlayerController>();
        enemySpawnManager = FindObjectOfType<EnemySpawnManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        holeIndicator = GameObject.Find("Hole Indicator");
        
        orginalPos = layerSpawner.transform.position;
        holeIndicatorOriginalPos = holeIndicator.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (layerSpawner.finishedSpawning && !reachedBottom)
        {
            StartCoroutine(FallPlatorm());
        }

        if (layerSpawner.transform.position.y <= -9.25f)
        {
            reachedBottom = true;
        }

        Mathf.Clamp(layerSpawner.spawnSpeed, 0.05f, 3f);
        Mathf.Clamp(player.moveSpeed, 1f, maxPlayerSpeed);
        Mathf.Clamp(fallSpeed, maxFallSpeed, 1f);
        Mathf.Clamp(turretHolder.speedModifier, 0.01f, 4f);
        Mathf.Clamp(turret.timeBetweenShots, 0.01f, 4f);
    }

    IEnumerator SpawnPlatforms()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            if (reachedBottom && isGameActive)
            {
                Debug.Log("Reseting Game");
                yield return new WaitForSeconds(1f);
                ResetGame();
            }
            
            if (finishedReset)
            {
                finishedReset = false;
                reachedBottom = false;
                if (withEnemies)
                {
                    //Spawn Enemies
                    enemySpawnManager.SpawnEnemyWave(waveNumber, 5);
                }
                StartCoroutine(layerSpawner.spawnGrid());
            }
        }
    }

    IEnumerator FallPlatorm()
    {
        layerSpawner.transform.Translate(Vector3.down * (Time.deltaTime / fallSpeed) , Space.World);
        yield return null;
    }

    public void StartGame(int difficulty)
    {
        isGameActive = true;

        foreach (var item in MainMenuItems)
        {
            item.SetActive(false);
        }

        foreach (var item in inGameUIItems)
        {
            item.SetActive(true);
        }

        if (withEnemies)
        {
            turretHolder.gameObject.SetActive(false);
        }
        else
        {
            turretHolder.canSpin = true;
            turret.StartShoot();
        }
        
        switch (difficulty)
        {
            case 1:
                player.moveSpeed = 15;
                difficultyIncrease = 0.05f;
                layerSpawner.spawnSpeed = 1f;

                turretHolder.speedModifier = 0.05f;
                turret.shootRate = 3.0f;
                turret.timeBetweenShots = 0.6f;
                break;
            case 3:
                player.moveSpeed = 19;
                difficultyIncrease = 0.03f;
                layerSpawner.spawnSpeed = 0.75f;
                
                turretHolder.speedModifier = 0.2f;
                turret.shootRate = 5.0f;
                turret.timeBetweenShots = 0.5f;
                break;
            case 6 :
                player.moveSpeed = 25;
                difficultyIncrease = 0.01f;
                layerSpawner.spawnSpeed = 0.3f;
                
                turretHolder.speedModifier = 0.25f;
                turret.shootRate = 7.0f;
                turret.timeBetweenShots = 0.4f;
                break;
        }
        
        fallSpeed /= difficulty;
        
        StartCoroutine(SpawnPlatforms());
    }
    public void ResetGame()
    {
        finishedReset = false;
        reachedBottom = true;
        foreach (Transform child in layerSpawner.transform)
        {
            child.gameObject.SetActive(false);
        }
        
        foreach (EnemyAI enemy in FindObjectsOfType<EnemyAI>())
        {
            Destroy(enemy.gameObject);
        }
        
        layerSpawner.transform.position = orginalPos;
        layerSpawner.finishedSpawning = false;
        holeIndicator.transform.position = holeIndicatorOriginalPos;
        player.damageToDeal = 0;
        player.canMove = true;
        if (fallSpeed > maxFallSpeed)
        {
            fallSpeed -= difficultyIncrease;
        }
        
        if (player.moveSpeed < maxPlayerSpeed)
        {
            player.moveSpeed += 1;
        }

        if (layerSpawner.spawnSpeed > 0.1f)
        {
            layerSpawner.spawnSpeed -= 0.05f;
        }

        if (withEnemies)
        {
            waveNumber++;
            enemyMoveSpeed += 0.5f;
        }
        else
        {
            turretHolder.speedModifier += 0.01f;
        }
        
        finishedReset = true;
    }

    public IEnumerator GameOver()
    {   
        GameOverScreen.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        finalScoreText.gameObject.SetActive(true);
        finalScoreText.text = "FINAL SCORE: " + scoreManager.score;
    }
}
