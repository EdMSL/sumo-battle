using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public GameUI gameUI;
    public Text countdownText;
    public GameObject EndGameScreen;
    public PlayerController player;
    public bool isGamePlay { get; set; } = false;
    public int score { get; private set; }
    public int wave { get; private set; }
    public int lives { get; private set; }

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        score = 0;
        wave = 0;
        StartCoroutine(GamePrepare());
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        gameUI.menuContainer.style.display = DisplayStyle.Flex;
    }

    public void RepeatGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void EndGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void ChageWave()
    {
        wave += 1;
        gameUI.waveCounter.text = wave.ToString();
    }

    IEnumerator GamePrepare()
    {
        gameUI.countdownContainer.style.display = DisplayStyle.Flex;
        gameUI.countdownText.text = "3";

        yield return new WaitForSeconds(1f);

        gameUI.countdownText.text = "2";

        yield return new WaitForSeconds(1f);

        gameUI.countdownText.text = "1";

        yield return new WaitForSeconds(1f);

        gameUI.countdownText.text = LocalizationSettings.StringDatabase.GetLocalizedString("countdown-go");

        yield return new WaitForSeconds(1f);

        isGamePlay = true;
        gameUI.countdownContainer.style.display = DisplayStyle.None;
    }
}
