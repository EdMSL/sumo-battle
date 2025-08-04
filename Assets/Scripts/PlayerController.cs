using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using JSAM;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    public GameUI gameUI;
    public GameObject focalPoint;
    public float noMovementTimer = 3.0f;
    public float powerupStrength = 10.0f;
    public bool isHavePowerup;
    public GameObject indicator;
    public Transform respawn;
    public List<Transform> restorePoints;

    private Rigidbody playerRb;
    private float speed = 10f;
    private bool isMovementBlocked;

    public int lives { get; set; }
    public bool isOnGround { get; set; }
    [HideInInspector] public Vector2 movement;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody>();

        switch (GameManager.Instance.difficultyLevel)
        {
            case GameManager.DifficultyLevel.Easy:
                speed = 30f;
                lives = 3;
                break;
            case GameManager.DifficultyLevel.Normal:
                speed = 20f;
                lives = 2;
                break;
            case GameManager.DifficultyLevel.Hard:
                speed = 10f;
                lives = 1;
                break;
            default:
                break;
        }

        gameUI.livesCounter.text = lives.ToString();
    }

    void Update()
    {
        if (GameManager.Instance.state == GameManager.State.GameProcess)
        {
            float verticalInput = movement.y;

            if (isOnGround)
            {
                playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed);
            }

            indicator.transform.rotation = Quaternion.identity;

            if (transform.position.y < -5f)
            {
                transform.position = respawn.position;
                playerRb.linearVelocity = Vector3.zero;
                isHavePowerup = false;
                indicator.gameObject.SetActive(false);
                RecountLives();
            }
        }
    }

    void FixedUpdate()
    {
        if (playerRb.linearVelocity.magnitude < 0.5f)
        {
            if (!isOnGround)
            {
                isMovementBlocked = true;
                RestorePosition();
            }
            Debug.Log("Object is not moving.");
            // Perform actions when no movement is detected
        }
        else
        {
            Debug.Log("Object is moving.");
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    private void RecountLives()
    {
        AudioManager.PlaySound(GameAudioLibrarySounds.die);
        lives -= 1;
        gameUI.livesCounter.text = lives.ToString();

        if (lives <= 0)
        {
            gameUI.livesCounter.text = "0";
            GameManager.Instance.GameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            AudioManager.PlaySound(GameAudioLibrarySounds.powerup);
            isHavePowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(Counter());
            indicator.gameObject.SetActive(true);
        }
    }

    IEnumerator Counter()
    {
        yield return new WaitForSeconds(5);
        isHavePowerup = false;
        indicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (isHavePowerup)
            {
                Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
                Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

                AudioManager.PlaySound(GameAudioLibrarySounds.heavyknock);
                enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
                isHavePowerup = false;
                indicator.gameObject.SetActive(false);
            }
            else
            {
                AudioManager.PlaySound(GameAudioLibrarySounds.knock);
            }
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            isMovementBlocked = false;
            // playerRb.linearDamping = 0.5f;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
            // playerRb.linearDamping = 3f;
        }
    }

    private void RestorePosition()
    {
        float maxDistance = 0f;
        Vector3 newPosition = transform.position;

        for (int i = 0; i < restorePoints.Count; i++)
        {
            if (maxDistance > Vector3.Distance(gameObject.transform.position, restorePoints[i].transform.position))
            {
                newPosition = restorePoints[i].position;
            }
        }
        Debug.Log(newPosition);

        // transform.position = maxDistance;
    }
}
