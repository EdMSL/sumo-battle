using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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

    public DifficultyLevel difficultyLevel { get; private set; } = DifficultyLevel.Normal;

    public int score { get; private set; }
    public State state { get; private set; }
    public int wave { get; private set; }
    public int lives { get; private set; }

    private float gameTimer;
    private bool isCountdownStarted = false;
    private GameUI gameUI;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        Instance = this;

        state = State.MainMenu;

        Application.targetFrameRate = 60;
    }

    void Start()
    {
        score = 0;
        wave = 0;
    }

    void Update()
    {
        Debug.Log(state);

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

    public void StartGame(string btnName)
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

        this.state = State.Waiting;
        SceneManager.LoadScene(1);
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        gameUI.menuContainer.style.display = DisplayStyle.Flex;
    }

    public void RepeatGame()
    {
        Time.timeScale = 1;
        SpawnManager.Instance.DestroyAllEnemies();
        state = State.Waiting;
        isCountdownStarted = false;
        SceneManager.LoadScene(1);
    }

    public void EndGame()
    {
        Time.timeScale = 1;
        Destroy(gameObject);
        SceneManager.LoadScene(0);
    }

    public void ChangeWave()
    {
        wave += 1;
        gameUI.waveCounter.text = wave.ToString();
    }

    IEnumerator GamePrepare()
    {
        gameUI = FindAnyObjectByType<GameUI>();
        gameUI.countdownContainer.style.display = DisplayStyle.Flex;
        gameUI.countdownText.text = "3";

        yield return new WaitForSeconds(1f);

        gameUI.countdownText.text = "2";

        yield return new WaitForSeconds(1f);

        gameUI.countdownText.text = "1";

        yield return new WaitForSeconds(1f);

        gameUI.countdownText.text = LocalizationSettings.StringDatabase.GetLocalizedString("countdown-go");

        yield return new WaitForSeconds(1f);

        state = State.GameProcess;
        gameUI.countdownContainer.style.display = DisplayStyle.None;
    }
}
