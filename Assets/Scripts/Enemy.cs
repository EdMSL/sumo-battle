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
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.state == GameManager.State.GameProcess)
        {
            Vector3 lookDirection = (PlayerController.Instance.transform.position - transform.position).normalized;

            enemyRb.AddForce(lookDirection * speed);

            if (PlayerController.Instance.transform.position.y < -1f)
            {
                enemyRb.linearVelocity = Vector3.zero;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeadZone"))
        {
            SpawnManager.Instance.DestroyEnemy(gameObject);
        }
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
}
