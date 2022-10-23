using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int enemyCount;
    public int waveNumber = 1;
    public GameObject powerUpPrefab;
    
    public EnemyAI[] enemies;
    
    private float spawnRangeX = 4f;
    private float spawnRangeZMin = -4.7f;
    private float spawnRangeZMax = 3f;

    private GameManager gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        //Instantiate(powerUpPrefab, GenerateSpawnPosition(), powerUpPrefab.transform.rotation);
    }
    
    //For-Loop spawn wave
    public void SpawnEnemyWave(int enemiesToSpawn, int maxToSpawn)
    {
        if (enemiesToSpawn <= maxToSpawn)
        {
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                Vector3 randomPos = GenerateSpawnPosition();
                Instantiate(enemyPrefab, randomPos, enemyPrefab.transform.rotation);
                Debug.Log(randomPos);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        enemies = FindObjectsOfType<EnemyAI>();
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRangeX, spawnRangeX);
        float spawnPosZ = Random.Range(spawnRangeZMin, spawnRangeZMax);
        
        Vector3 randomPos = new Vector3(spawnPosX, -9.8f, spawnPosZ);

        return randomPos;
    }
}
