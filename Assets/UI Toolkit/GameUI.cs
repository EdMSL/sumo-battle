using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using JSAM;

public class GameUI : MonoBehaviour
{
    private VisualElement _gameMenuWindow;
    private VisualElement _gameMenuWindowContent;
    private Button _gameMenuResumeGameBtn;
    private Button _gameMenuSettingsGameBtn;
    private Button _gameMenuGoToMenuGameBtn;
    private VisualElement _endGameWindow;
    // private VisualElement _settingsWindow;
    private Button _settingsAcceptButton;
    private Button _settingsCancelButton;
    private VisualElement _settingsContainer;
    private VisualElement _playerContainer;
    public VisualElement countdownContainer;
    public Label livesCounter;
    public Label waveCounter;
    public Label countdownText;
    public Button repeatGameBtn;
    public Button goToMenuBtn;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        _gameMenuWindow = uiDocument.rootVisualElement.Q("game-menu-window");
        _gameMenuWindowContent = _gameMenuWindow.Q("popup-content");
        _gameMenuResumeGameBtn = _gameMenuWindowContent.Q<Button>("continue-btn");
        _gameMenuSettingsGameBtn = _gameMenuWindowContent.Q<Button>("settings-btn");
        _gameMenuGoToMenuGameBtn = _gameMenuWindowContent.Q<Button>("gotomenu-btn");

        // _settingsWindow = uiDocument.rootVisualElement.Q("settings-window");
        // _settingsContainer = _gameMenuWindowContent.Q("popup-content");

        _playerContainer = uiDocument.rootVisualElement.Query<VisualElement>("hud-container");

        _playerContainer = uiDocument.rootVisualElement.Q("hud-container");
        countdownContainer = uiDocument.rootVisualElement.Q("countdown-container");

        _endGameWindow = uiDocument.rootVisualElement.Q("end-game-window");

        livesCounter = _playerContainer.Q("lives-block").Q<Label>(className: "hud__counter");
        waveCounter = _playerContainer.Q("wave-block").Q<Label>(className: "hud__counter");
        countdownText = countdownContainer.Q<Label>("countdown-text");

        // _settingsCancelButton = _settingsWindow.Q<Button>("cancel-btn");
        // _settingsAcceptButton = _settingsWindow.Q<Button>("accept-btn");

        repeatGameBtn = _endGameWindow.Q<Button>("btn-yes");
        goToMenuBtn = _endGameWindow.Q<Button>("btn-no");

        _gameMenuResumeGameBtn.RegisterCallback((ClickEvent evt) => { GameManager.Instance.TogglePauseGame(); });
        _gameMenuSettingsGameBtn.RegisterCallback<ClickEvent>(OnSettingsBtnClick);
        _gameMenuGoToMenuGameBtn.RegisterCallback<ClickEvent>(OnGoToMenuBtnClick);

        repeatGameBtn.RegisterCallback<ClickEvent>(OnRepeatGameBtnClick);
        goToMenuBtn.RegisterCallback<ClickEvent>(OnGoToMenuBtnClick);

        // _settingsAcceptButton.RegisterCallback<ClickEvent>(OnSettingsBtnClick);
        // _settingsCancelButton.RegisterCallback<ClickEvent>(OnSettingsBtnClick);

        livesCounter.text = PlayerController.Instance.lives.ToString();
        waveCounter.text = GameManager.Instance.wave.ToString();
    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        SwitchMainMenu(true);
    }

    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        SwitchMainMenu(false);
    }

    private void SwitchMainMenu(bool isOpen)
    {
        AudioManager.PlaySound(GameAudioLibrarySounds.click);

        if (isOpen)
        {
            Time.timeScale = 0f;

            _gameMenuWindow.RemoveFromClassList("popup-hidden");
            _gameMenuWindowContent.RemoveFromClassList("animation__hide");
            _gameMenuWindow.AddToClassList("popup-show");
            _gameMenuWindowContent.AddToClassList("animation__show");
        }
        else
        {
            StartCoroutine(UIDelay());
        }
    }

    private void OnGoToMenuBtnClick(ClickEvent evt)
    {
        AudioManager.PlaySound(GameAudioLibrarySounds.click);

        GameManager.Instance.EndGame();
    }

    private void OnRepeatGameBtnClick(ClickEvent evt)
    {
        AudioManager.PlaySound(GameAudioLibrarySounds.click);

        GameManager.Instance.RepeatGame();
    }

    private void OnSettingsBtnClick(ClickEvent evt)
    {
        AudioManager.PlaySound(GameAudioLibrarySounds.click);
    }

    IEnumerator UIDelay()
    {
        Time.timeScale = 1f;

        _gameMenuWindowContent.AddToClassList("animation__hide");
        _gameMenuWindowContent.RemoveFromClassList("animation__show");

        yield return new WaitForSecondsRealtime(0.3f);

        _gameMenuWindow.RemoveFromClassList("popup-show");
        _gameMenuWindow.AddToClassList("popup-hidden");
    }
}
