using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using JSAM;
using System.Collections.Generic;
using System;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    public GameObject focalPoint;
    public float noMovementTime = 3.0f;
    public float noMowementMagnitude = 0.5f;
    public float baseHitStrength = 2.5f;
    public float powerupStrength = 10.0f;
    public float linearDamping = 3.0f;
    public GameObject indicator;
    public Transform respawn;
    public List<Transform> restorePoints;

    private Rigidbody playerRb;
    private MeshRenderer playerMr;
    private float speed = 30f;
    private bool isMovementBlocked;
    private float movementBlockedTimer;
    private bool isHavePowerup;
    private InputAction menuAction;
    private GameUI gameUI;

    private Coroutine powerUpCoroutine;
    private Vector3 newPosition = Vector3.zero;

    public int lives { get; set; }
    public bool isOnGround { get; set; }

    [HideInInspector] public Vector2 movement;
    private float powerUpTime;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody>();
        playerMr = gameObject.GetComponent<MeshRenderer>();
        gameUI = FindAnyObjectByType<GameUI>();
        playerRb.linearDamping = linearDamping;

        menuAction = gameObject.GetComponent<PlayerInput>().actions["Menu"];

        if (menuAction != null)
        {
            menuAction.performed += GameManager_OnTogglePause;
        }

        GameManager.Instance.OnSecondChance += GameManager_OnSecondChance;

        switch (GameManager.Instance.difficultyLevel)
        {
            case GameManager.DifficultyLevel.Easy:
                lives = 3;
                powerUpTime = 15f;
                break;
            case GameManager.DifficultyLevel.Normal:
                lives = 2;
                powerUpTime = 10f;
                break;
            case GameManager.DifficultyLevel.Hard:
                lives = 1;
                powerUpTime = 5f;
                break;
            default:
                break;
        }

        gameUI.livesCounter.text = lives.ToString();
        SetSkin(GameManager.Instance.playerSkin);
    }

    private void OnDisable()
    {
        if (menuAction != null)
        {
            menuAction.performed -= GameManager_OnTogglePause;
        }
    }

    void Update()
    {
        if (GameManager.Instance.state == GameManager.State.GameProcess)
        {
            indicator.transform.rotation = Quaternion.identity;

            if (isMovementBlocked)
            {
                movementBlockedTimer += Time.deltaTime;

                if (movementBlockedTimer > noMovementTime)
                {
                    movementBlockedTimer = 0f;
                    RestorePosition();
                }
            }
            else
            {
                movementBlockedTimer = 0f;
            }
        }
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.state == GameManager.State.GameProcess)
        {
            if (newPosition != Vector3.zero)
            {
                transform.position = newPosition;
                newPosition = Vector3.zero;
            }

            float verticalInput = movement.y;

            if (isOnGround)
            {
                playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed);
            }

            if (playerRb.linearVelocity.magnitude < noMowementMagnitude)
            {
                if (!isOnGround)
                {
                    isMovementBlocked = true;
                }
            }
            else
            {
                if (isOnGround)
                {
                    isMovementBlocked = false;
                }
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup") && !isHavePowerup)
        {
            AudioManager.PlaySound(GameAudioLibrarySounds.powerup);
            Destroy(other.gameObject);
            SpawnManager.Instance.powerupsQuantity--;
            isHavePowerup = true;
            powerUpCoroutine = StartCoroutine(Counter());
            indicator.gameObject.SetActive(true);
        }

        if (other.CompareTag("DeadZone"))
        {
            transform.position = respawn.position;
            playerRb.linearVelocity = Vector3.zero;
            isHavePowerup = false;
            indicator.gameObject.SetActive(false);
            RecountLives();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            if (isHavePowerup)
            {

                AudioManager.PlaySound(GameAudioLibrarySounds.heavyknock);
                enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
                isHavePowerup = false;
                indicator.gameObject.SetActive(false);
            }
            else
            {
                AudioManager.PlaySound(GameAudioLibrarySounds.knock);
                enemyRb.AddForce(awayFromPlayer * baseHitStrength, ForceMode.Impulse);
            }
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            isMovementBlocked = false;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
        }
    }


    private void GameManager_OnSecondChance(object sender, EventArgs e)
    {
        lives = 1;
        gameUI.livesCounter.text = lives.ToString();
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

    IEnumerator Counter()
    {
        yield return new WaitForSeconds(powerUpTime);

        isHavePowerup = false;
        indicator.gameObject.SetActive(false);
    }

    private void RestorePosition()
    {
        float maxDistance = 0f;
        // Vector3 newPosition;

        for (int i = 0; i < restorePoints.Count; i++)
        {
            float curDistance = Mathf.Abs(Vector3.Distance(transform.position, restorePoints[i].transform.position));

            if (maxDistance < curDistance)
            {
                maxDistance = curDistance;
                newPosition = restorePoints[i].position;
            }
        }

        // transform.position = newPosition;
    }

    public void SetSkin(Material skin)
    {
        playerMr.material = skin;
    }

    private void GameManager_OnTogglePause(InputAction.CallbackContext context)
    {
        GameManager.Instance.TogglePauseGame();
    }
}
