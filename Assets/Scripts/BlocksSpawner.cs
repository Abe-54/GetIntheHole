using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject groundPrefab;
    public GameObject groundWithHolePrefab;

    [Space(5)]
    
    [Header("Layer Adjustments")]
    public int layerLength = 10;
    public int layerWidth = 10;
    public int prefabScaleOffset = 3;
    public float prefabSpawnOffset = 3;

    [Space(5)]
    
    [Header("Game Adjustments")]
    public float spawnSpeed = 0.3f;
    public bool finishedSpawning = false;
    public IEnumerator spawnGrid()
    {
        int randomPosX = Random.Range(0, layerWidth);
        int randomPosZ = Random.Range(0, layerLength);
        
        for (int x = 0; x < layerWidth; x++)
        {
            for (int z = 0; z < layerLength; z++)
            {
                yield return new WaitForSeconds(spawnSpeed);

                if (x == randomPosX && z == randomPosZ)
                {
                    SpawnPrefabBlock(groundWithHolePrefab, x, z);
                }
                else
                {
                    SpawnPrefabBlock(groundPrefab, x, z);
                }
            }
        }

        finishedSpawning = true;
    }

    private void SpawnPrefabBlock(GameObject block, int x, int z)
    {
        block = Instantiate(block, Vector3.zero, groundPrefab.transform.rotation);
        block.transform.parent = transform;
        block.transform.localPosition = new Vector3((x * prefabScaleOffset) - prefabSpawnOffset, transform.position.y, (z * prefabScaleOffset) - prefabSpawnOffset);

    }
}
