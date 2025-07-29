using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody enemyRb;
    private float bottomBound = -10.0f;
    private float speed = 3f;

    void Start()
    {
        enemyRb = gameObject.GetComponent<Rigidbody>();
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

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
}
