using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using JSAM;

public class GameUI : MonoBehaviour
{
    private VisualElement _gameMenuWindow;
    private VisualElement _gameMenuWindowContent;
    private VisualElement _mainMenuContainer;
    private Button _mainMenuResumeGameBtn;
    private Button _mainMenuSettingsGameBtn;
    private Button _mainMenuGoToMenuGameBtn;
    private VisualElement _settingsContainer;
    private VisualElement _endGameWindow;
    private Button _settingsAcceptButton;
    private Button _settingsCancelButton;
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

        _mainMenuContainer = _gameMenuWindowContent.Q("main-menu-container");
        _mainMenuResumeGameBtn = _mainMenuContainer.Q<Button>("continue-btn");
        _mainMenuSettingsGameBtn = _mainMenuContainer.Q<Button>("settings-btn");
        _mainMenuGoToMenuGameBtn = _mainMenuContainer.Q<Button>("gotomenu-btn");

        _settingsContainer = _gameMenuWindowContent.Q("settings-container");
        _settingsAcceptButton = _settingsContainer.Q<Button>("accept-btn");
        _settingsCancelButton = _settingsContainer.Q<Button>("cancel-btn");

        _playerContainer = uiDocument.rootVisualElement.Query<VisualElement>("hud-container");
        _playerContainer = uiDocument.rootVisualElement.Q("hud-container");
        countdownContainer = uiDocument.rootVisualElement.Q("countdown-container");

        _endGameWindow = uiDocument.rootVisualElement.Q("end-game-window");

        livesCounter = _playerContainer.Q("lives-block").Q<Label>(className: "hud__counter");
        waveCounter = _playerContainer.Q("wave-block").Q<Label>(className: "hud__counter");
        countdownText = countdownContainer.Q<Label>("countdown-text");

        repeatGameBtn = _endGameWindow.Q<Button>("btn-yes");
        goToMenuBtn = _endGameWindow.Q<Button>("btn-no");

        _mainMenuResumeGameBtn.RegisterCallback((ClickEvent evt) => { GameManager.Instance.TogglePauseGame(); });
        _mainMenuSettingsGameBtn.RegisterCallback<ClickEvent>(OnSettingsBtnClick);
        _mainMenuGoToMenuGameBtn.RegisterCallback<ClickEvent>(OnGoToMenuBtnClick);

        _settingsAcceptButton.RegisterCallback<ClickEvent>(OnSettingsBtnClick);
        _settingsCancelButton.RegisterCallback<ClickEvent>(OnSettingsBtnClick);

        repeatGameBtn.RegisterCallback<ClickEvent>(OnRepeatGameBtnClick);
        goToMenuBtn.RegisterCallback<ClickEvent>(OnGoToMenuBtnClick);


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

        ToggleSettingMenu();
    }

    private void ToggleSettingMenu()
    {
        _settingsContainer.ToggleInClassList("hide");
        _mainMenuContainer.ToggleInClassList("hide");
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
