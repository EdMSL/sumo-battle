using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text waveText;
    public Text livesText;
    public Text countdownText;
    public GameObject EndGameScreen;
    public PlayerController player;
    public bool isGamePlay {get; set;} = false;
    public int score {get; private set;}
    public int wave {get; private set;}
    public int lives {get; private set;}

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
        waveText.text = wave.ToString();
    }

    IEnumerator GamePrepare()
    {
        countdownText.text = "3";

        yield return new WaitForSeconds(1f);

        countdownText.text = "2";

        yield return new WaitForSeconds(1f);

        countdownText.text = "1";

        yield return new WaitForSeconds(1f);

        countdownText.text = "Battle!";
        isGamePlay = true;

        yield return new WaitForSeconds(0.3f);

        countdownText.gameObject.SetActive(false);
    }
}
