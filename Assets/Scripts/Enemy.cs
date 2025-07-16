using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody enemyRb;
    private GameObject player;
    private float bottomBound = -10.0f;
    private float speed = 3f;

    void Start()
    {
        enemyRb = gameObject.GetComponent<Rigidbody>();
        player = GameObject.Find("Player");

        switch (Save.difficultyLevel)
        {
            case Save.DifficultyLevel.Easy:
                speed = 1f;
                break;
            case Save.DifficultyLevel.Normal:
                speed = 2f;
                break;
            case Save.DifficultyLevel.Hard:
                speed = 3f;
                break;
            default:
                break;
        }
    }

    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;

        enemyRb.AddForce(lookDirection * speed);

        if (transform.position.y < bottomBound)
        {
            Destroy(gameObject);
        }

        if (player.transform.position.y < -1f)
        {
            enemyRb.linearVelocity = Vector3.zero;
        }
    }
}
