using System.Collections;
using UnityEditor.Localization.Editor;
using UnityEditor.Search;
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

    void Start()
    {
        score = 0;
        wave = 0;
        StartCoroutine(GamePrepare());
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        EndGameScreen.SetActive(true);
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
        gameUI.countdownText.style.display = DisplayStyle.Flex;
        gameUI.countdownText.text = "3";

        yield return new WaitForSeconds(1f);

        gameUI.countdownText.text = "2";

        yield return new WaitForSeconds(1f);

        gameUI.countdownText.text = "1";

        yield return new WaitForSeconds(1f);

        gameUI.countdownText.text = LocalizationSettings.StringDatabase.GetLocalizedString("countdown-go");
        isGamePlay = true;

        yield return new WaitForSeconds(0.3f);

        gameUI.countdownText.style.display = DisplayStyle.None;
    }
}
