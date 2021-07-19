using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    public GameManager gameManager;
    private float spawnRangeBound = 9f;
    private byte enemiesQuontity = 1;
    public int enemiesCounter;

    void Start()
    {
        // SpawnEnemysWave(enemiesQuontity);
        // Instantiate(powerupPrefab, GetRandomSpawnPos(), powerupPrefab.transform.rotation);
    }

    void SpawnEnemysWave(byte enemiesToSpawn)
    {
      for (int i = 0; i < enemiesToSpawn; i++)
      {
          Instantiate(enemyPrefab, GetRandomSpawnPos(), enemyPrefab.transform.rotation);
      }
    }

    void Update()
    {
        if (gameManager.isGamePlay)
        {
            enemiesCounter = FindObjectsOfType<Enemy>().Length;

            if (enemiesCounter == 0)
            {
                enemiesQuontity++;
                SpawnEnemysWave(enemiesQuontity);
                Instantiate(powerupPrefab, GetRandomSpawnPos(), powerupPrefab.transform.rotation);
                gameManager.ChageWave();
            }
        }
    }

    private Vector3 GetRandomSpawnPos()
    {
        float spawnPosX = Random.Range(-spawnRangeBound, spawnRangeBound);
        float spawnPosZ = Random.Range(-spawnRangeBound, spawnRangeBound);

        return new Vector3(spawnPosX, 0, spawnPosZ);
    }
}
