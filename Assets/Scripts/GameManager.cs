using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnCountdawnStart;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    public event EventHandler OnGameOver;

    public enum State
    {
        MainMenu,
        Waiting,
        Countdown,
        GameProcess,
        GameOver,
    }
    public enum DifficultyLevel
    {
        Easy,
        Normal,
        Hard,
    }

    public float waitingTime = 1f;

    public DifficultyLevel difficultyLevel { get; private set; } = DifficultyLevel.Hard;
    public Material playerSkin;
    public State state { get; private set; }
    public int score { get; private set; }
    public int wave { get; private set; }

    private float gameTimer;
    private bool isCountdownStarted = false;
    private GameUI gameUI;
    private bool isGamePaused = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        Instance = this;

        // state = State.Waiting;
        state = State.MainMenu;

        Application.targetFrameRate = 60;
    }

    void Start()
    {
        ResetCounters();
    }

    void Update()
    {
        if (state != State.MainMenu)
        {
            switch (state)
            {
                case State.Waiting:
                    gameTimer += Time.deltaTime;

                    if (gameTimer > waitingTime)
                    {
                        gameTimer = 0f;
                        state = State.Countdown;
                    }

                    break;
                case State.Countdown:
                    if (!isCountdownStarted)
                    {
                        isCountdownStarted = true;
                        OnCountdawnStart?.Invoke(this, EventArgs.Empty);
                        StartCoroutine(GamePrepare());
                    }

                    break;
                case State.GameProcess:
                    break;
                case State.GameOver:
                    break;
                default:
                    break;
            }
        }
    }

    public void StartGame(int sceneIndex)
    {
        state = State.Waiting;
        SceneManager.LoadScene(sceneIndex);
    }

    public void SetDifficultyLevel(string btnName)
    {
        if (btnName == DifficultyLevel.Easy.ToString().ToLower())
        {
            difficultyLevel = DifficultyLevel.Easy;
        }
        else if (btnName == DifficultyLevel.Normal.ToString().ToLower())
        {
            difficultyLevel = DifficultyLevel.Normal;
        }
        else if (btnName == DifficultyLevel.Hard.ToString().ToLower())
        {
            difficultyLevel = DifficultyLevel.Hard;
        }
    }

    public void SetPlayerSkin(Material skin)
    {
        playerSkin = skin;
    }

    public void GameOver()
    {
        state = State.GameOver;
        OnGameOver?.Invoke(this, EventArgs.Empty);
    }

    public void RepeatGame()
    {
        SpawnManager.Instance.DestroyAllEnemies();
        isCountdownStarted = false;
        state = State.Waiting;
        ResetCounters();
        SceneManager.LoadScene(1);
    }

    public void EndGame()
    {
        Destroy(gameObject);
        state = State.MainMenu;
        SceneManager.LoadScene(0);
    }

    public void ChangeWave()
    {
        wave += 1;
        gameUI.waveCounter.text = wave.ToString();
    }

    IEnumerator GamePrepare()
    {
        if (!gameUI)
        {
            gameUI = FindAnyObjectByType<GameUI>();
        }

        gameUI.countdownContainer.ToggleInClassList("hide");

        gameUI.countdownText.text = "3";
        gameUI.countdownText.AddToClassList("countdown__title-hide");
        // gameUI.countdownText.ToggleInClassList(".countdown__title-show");
        // gameUI.countdownText.ToggleInClassList(".countdown__title-hide");

        yield return new WaitForSeconds(1f);

        gameUI.countdownText.RemoveFromClassList("countdown__title-hide");
        gameUI.countdownText.AddToClassList("countdown__title-show");
        gameUI.countdownText.text = "2";
        gameUI.countdownText.AddToClassList("countdown__title-hide");
        gameUI.countdownText.RemoveFromClassList("countdown__title-show");

        yield return new WaitForSeconds(1f);

        gameUI.countdownText.RemoveFromClassList("countdown__title-hide");
        gameUI.countdownText.AddToClassList("countdown__title-show");
        gameUI.countdownText.text = "1";
        gameUI.countdownText.RemoveFromClassList("countdown__title-show");
        gameUI.countdownText.AddToClassList("countdown__title-hide");

        yield return new WaitForSeconds(1f);

        gameUI.countdownText.text = LocalizationSettings.StringDatabase.GetLocalizedString("countdown-go");

        yield return new WaitForSeconds(1f);

        state = State.GameProcess;
        gameUI.countdownContainer.ToggleInClassList("hide");
    }

    private void ResetCounters()
    {
        wave = 0;
        score = 0;
    }

    public void OnGamePauseAction(InputAction.CallbackContext context)
    {
        if (context.performed && state != State.GameOver && state != State.MainMenu)
        {
            TogglePauseGame();
        }
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;

        if (!gameUI)
        {
            gameUI = FindAnyObjectByType<GameUI>();
        }

        if (isGamePaused)
        {
            Time.timeScale = 0f;

            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;

            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }
}
