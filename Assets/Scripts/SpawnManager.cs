using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    public float spawnRangeBound = 9f;
    public byte enemiesStartQuontity = 1;
    public float enemySpeed = 0f;

    private byte enemiesQuontity;
    [HideInInspector] public List<GameObject> enemiesList;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one DeliveryManager instance");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (enemiesStartQuontity == 0)
        {
            switch (GameManager.Instance.difficultyLevel)
            {
                case GameManager.DifficultyLevel.Easy:
                    enemiesQuontity = 2;
                    break;
                case GameManager.DifficultyLevel.Normal:
                    enemiesQuontity = 4;
                    break;
                case GameManager.DifficultyLevel.Hard:
                    enemiesQuontity = 6;
                    break;
                default:
                    break;
            }
        }
        else
        {
            enemiesQuontity = enemiesStartQuontity;
        }
    }

    void Update()
    {
        if (GameManager.Instance.state == GameManager.State.GameProcess)
        {
            if (enemiesList.Count == 0)
            {
                if (GameManager.Instance.wave > 1)
                {
                    enemiesQuontity++;
                }

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
            var enemy = Instantiate(enemyPrefab, GetRandomSpawnPos(), enemyPrefab.transform.rotation);

            if (enemySpeed == 0f)
            {
                switch (GameManager.Instance.difficultyLevel)
                {
                    case GameManager.DifficultyLevel.Easy:
                        enemySpeed = 1f;
                        break;
                    case GameManager.DifficultyLevel.Normal:
                        enemySpeed = 2f;
                        break;
                    case GameManager.DifficultyLevel.Hard:
                        enemySpeed = 3f;
                        break;
                    default:
                        break;
                }
            }

            enemy.GetComponent<Enemy>().SetSpeed(enemySpeed);
            enemiesList.Add(enemy);
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
        enemiesList.Clear();
    }
}
