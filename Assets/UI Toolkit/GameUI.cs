using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using JSAM;
using AlpaSunFade;

public class GameUI : MonoBehaviour
{
    [SerializeField] TransitionPanel transitionPanelScript;

    private VisualElement _gameMenuWindow;
    private VisualElement _gameMenuWindowContent;
    private VisualElement _mainMenuContainer;
    private Button _mainMenuResumeGameBtn;
    private Button _mainMenuSettingsGameBtn;
    private Button _mainMenuGoToMenuGameBtn;
    private VisualElement _settingsContainer;
    private VisualElement _endGameWindow;
    private VisualElement _endGameWindowContent;
    private VisualElement _endGameContainer;
    private Button _settingsAcceptButton;
    private Button _settingsCancelButton;
    private VisualElement _hudContainer;
    private VisualElement _mobileControlsContainer;
    public VisualElement countdownContainer;
    public Label livesCounter;
    public Label waveCounter;
    public Label countdownText;
    public Button _endGameRepeatBtn;
    public Button _endGameGoToMenuBtn;

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

        countdownContainer = uiDocument.rootVisualElement.Q("countdown-container");
        countdownText = countdownContainer.Q<Label>("countdown-text");

        _hudContainer = uiDocument.rootVisualElement.Q("hud-container");

        _mobileControlsContainer = uiDocument.rootVisualElement.Q("mobile-controls");

        livesCounter = _hudContainer.Q("lives-block").Q<Label>(className: "hud__counter");
        waveCounter = _hudContainer.Q("wave-block").Q<Label>(className: "hud__counter");

        _endGameWindow = uiDocument.rootVisualElement.Q("end-game-window");
        _endGameWindowContent = _endGameWindow.Q("popup-content");

        _endGameContainer = _endGameWindow.Q("end-game-container");
        _endGameRepeatBtn = _endGameContainer.Q<Button>("btn-yes");
        _endGameGoToMenuBtn = _endGameContainer.Q<Button>("btn-no");

        _mainMenuResumeGameBtn.RegisterCallback<ClickEvent>(OnResumeBtnClick);
        _mainMenuSettingsGameBtn.RegisterCallback<ClickEvent>(OnSettingsBtnClick);
        _mainMenuGoToMenuGameBtn.RegisterCallback<ClickEvent>(OnGoToMenuBtnClick);

        _settingsAcceptButton.RegisterCallback<ClickEvent>(OnSettingsBtnClick);
        _settingsCancelButton.RegisterCallback<ClickEvent>(OnSettingsBtnClick);

        _endGameRepeatBtn.RegisterCallback<ClickEvent>(OnRepeatGameBtnClick);
        _endGameGoToMenuBtn.RegisterCallback<ClickEvent>(OnGoToMenuBtnClick);
    }

    private void OnDisable()
    {
        _mainMenuResumeGameBtn.UnregisterCallback<ClickEvent>(OnResumeBtnClick);
        _mainMenuSettingsGameBtn.UnregisterCallback<ClickEvent>(OnSettingsBtnClick);
        _mainMenuGoToMenuGameBtn.UnregisterCallback<ClickEvent>(OnGoToMenuBtnClick);

        _settingsAcceptButton.UnregisterCallback<ClickEvent>(OnSettingsBtnClick);
        _settingsCancelButton.UnregisterCallback<ClickEvent>(OnSettingsBtnClick);

        _endGameRepeatBtn.UnregisterCallback<ClickEvent>(OnRepeatGameBtnClick);
        _endGameGoToMenuBtn.UnregisterCallback<ClickEvent>(OnGoToMenuBtnClick);

        GameManager.Instance.OnCountdawnStart -= GameManager_OnCountdawnStart;
        GameManager.Instance.OnGamePaused -= GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused -= GameManager_OnGameUnpaused;
        GameManager.Instance.OnGameOver -= GameManager_OnGameOver;
    }

    private void Start()
    {
        GameManager.Instance.OnCountdawnStart += GameManager_OnCountdawnStart;
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;

        if (!Platform.IsMobileBrowser())
        {
            _mobileControlsContainer.AddToClassList("hide");
        }

        livesCounter.text = PlayerController.Instance.lives.ToString();
        waveCounter.text = GameManager.Instance.wave.ToString();

        /// Нормально работает только с задержкой начала (иначе отклбючается сразу же) и продолжительностью, помноженной на 2.
        transitionPanelScript.StartTransition(false, 0.1f, GameManager.Instance.waitingTime * 2);
    }

    private void GameManager_OnGameOver(object sender, EventArgs e)
    {
        _endGameWindow.RemoveFromClassList("popup-hidden");
        _endGameWindowContent.RemoveFromClassList("animation__hide");
        _endGameWindow.AddToClassList("popup-show");
        _endGameWindowContent.AddToClassList("animation__show");
    }

    private void GameManager_OnCountdawnStart(object sender, EventArgs e)
    {
        if (transitionPanelScript != null)
        {
            transitionPanelScript.gameObject.SetActive(false);
        }
    }

    private void GameManager_OnGamePaused(object sender, EventArgs e)
    {
        SwitchMainMenu(true);
    }

    private void GameManager_OnGameUnpaused(object sender, EventArgs e)
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

    private void OnResumeBtnClick(ClickEvent evt)
    {
        GameManager.Instance.TogglePauseGame();
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
