using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;
    public GameUI gameUI;
    public GameObject focalPoint;
    public float powerupStrength = 10.0f;
    public bool isHavePowerup;
    public GameObject indicator;
    public Transform respawn;
    public AudioClip powerup;
    public AudioClip touch;
    public AudioClip heavyTouch;
    public AudioClip lose;

    private AudioSource audioSource;
    private Rigidbody playerRb;
    private float speed = 10f;

    public int lives { get; set; }
    public bool isOnGround { get; set; }
    [HideInInspector] public Vector2 movement;

    void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody>();
        audioSource = gameObject.GetComponent<AudioSource>();

        switch (Save.difficultyLevel)
        {
            case Save.DifficultyLevel.Easy:
                speed = 30f;
                lives = 3;
                break;
            case Save.DifficultyLevel.Normal:
                speed = 20f;
                lives = 2;
                break;
            case Save.DifficultyLevel.Hard:
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
        if (gameManager.isGamePlay)
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

    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    private void RecountLives()
    {
        audioSource.PlayOneShot(lose);
        lives -= 1;
        gameUI.livesCounter.text = lives.ToString();

        if (lives <= 0)
        {
            gameUI.livesCounter.text = "0";
            gameManager.GameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            audioSource.PlayOneShot(powerup);
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

                audioSource.PlayOneShot(heavyTouch);
                enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
                isHavePowerup = false;
                indicator.gameObject.SetActive(false);
            }
            else
            {
                audioSource.PlayOneShot(touch);
            }
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            playerRb.linearDamping = 0.5f;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
            playerRb.linearDamping = 3f;
        }
    }
}
