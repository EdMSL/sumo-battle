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
    private float speed = 10f;
    private bool isMovementBlocked;
    private float movementBlockedTimer;
    private bool isHavePowerup;
    private InputAction menuAction;
    private GameUI gameUI;

    private Coroutine powerUpCoroutine;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            AudioManager.PlaySound(GameAudioLibrarySounds.powerup);
            Destroy(other.gameObject);
            SpawnManager.Instance.powerupsQuantity--;

            if (isHavePowerup)
            {
                StopCoroutine(powerUpCoroutine);
            }

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
        yield return new WaitForSeconds(5);

        isHavePowerup = false;
        indicator.gameObject.SetActive(false);
    }

    private void RestorePosition()
    {
        float maxDistance = 0f;

        Vector3 newPosition = transform.position;

        for (int i = 0; i < restorePoints.Count; i++)
        {
            if (maxDistance < Mathf.Abs(Vector3.Distance(gameObject.transform.position, restorePoints[i].transform.position)))
            {
                newPosition = restorePoints[i].position;
            }
        }

        transform.position = newPosition;
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
