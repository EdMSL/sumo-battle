using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody enemyRb;
    private float bottomBound = -10.0f;
    private float speed = 3f;

    void Start()
    {
        enemyRb = gameObject.GetComponent<Rigidbody>();

        switch (GameManager.Instance.difficultyLevel)
        {
            case GameManager.DifficultyLevel.Easy:
                speed = 1f;
                break;
            case GameManager.DifficultyLevel.Normal:
                speed = 2f;
                break;
            case GameManager.DifficultyLevel.Hard:
                speed = 3f;
                break;
            default:
                break;
        }
    }

    void Update()
    {
        Vector3 lookDirection = (PlayerController.Instance.transform.position - transform.position).normalized;

        enemyRb.AddForce(lookDirection * speed);

        if (transform.position.y < bottomBound)
        {
            SpawnManager.Instance.DestroyEnemy(gameObject);
        }

        if (PlayerController.Instance.transform.position.y < -1f)
        {
            enemyRb.linearVelocity = Vector3.zero;
        }
    }
}
