using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    private float spawnRangeBound = 9f;
    private byte enemiesQuontity = 1;
    public List<GameObject> enemiesList;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one DeliveryManager instance");
        }

        Instance = this;
    }

    void Update()
    {
        if (GameManager.Instance.state == GameManager.State.GameProcess)
        {
            if (enemiesList.Count == 0)
            {
                enemiesQuontity++;
                SpawnEnemysWave(enemiesQuontity);
                Instantiate(powerupPrefab, GetRandomSpawnPos(), powerupPrefab.transform.rotation);
                GameManager.Instance.ChangeWave();
            }
        }
    }

    void SpawnEnemysWave(byte enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            enemiesList.Add(Instantiate(enemyPrefab, GetRandomSpawnPos(), enemyPrefab.transform.rotation));
        }
    }

    private Vector3 GetRandomSpawnPos()
    {
        float spawnPosX = Random.Range(-spawnRangeBound, spawnRangeBound);
        float spawnPosZ = Random.Range(-spawnRangeBound, spawnRangeBound);

        return new Vector3(spawnPosX, 0, spawnPosZ);
    }

    public void DestroyEnemy(GameObject enemy)
    {
        Destroy(enemy);
        enemiesList.RemoveAt(enemiesList.IndexOf(enemy));
    }

    public void DestroyAllEnemies()
    {
        // enemiesList.ForEach(enemy => Destroy(enemy));
        enemiesList.Clear();
    }
}
