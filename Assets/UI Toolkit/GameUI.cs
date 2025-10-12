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
    private VisualElement _hudContainer;
    private VisualElement _mobileControlsContainer;
    private VisualElement _leftControl;
    private VisualElement _rightControl;
    private VisualElement _upControl;
    private VisualElement _downControl;
    private VisualElement _pauseControl;
    public VisualElement _endGameBlock;
    public VisualElement _endGameSecondChanceBlock;
    private Button _endGameGoToMenuBtn;
    private Button _endGameRepeatBtn;
    public VisualElement countdownContainer;
    public Label livesCounter;
    public Label waveCounter;
    public Label countdownText;
    public OnScreenControlTrigger PauseControlTrigger;
    public OnScreenControlTrigger UpControlTrigger;
    public OnScreenControlTrigger DownControlTrigger;
    public OnScreenControlTrigger LeftControlTrigger;
    public OnScreenControlTrigger RightControlTrigger;

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

        countdownContainer = uiDocument.rootVisualElement.Q("countdown-container");
        countdownText = countdownContainer.Q<Label>("countdown-text");

        _hudContainer = uiDocument.rootVisualElement.Q("hud-container");

        _mobileControlsContainer = uiDocument.rootVisualElement.Q("mobile-controls-container");

        if (Platform.IsMobileBrowser())
        {
            _leftControl = _mobileControlsContainer.Q("left-control");
            _rightControl = _mobileControlsContainer.Q("right-control");
            _upControl = _mobileControlsContainer.Q("up-control");
            _downControl = _mobileControlsContainer.Q("down-control");
            _pauseControl = _mobileControlsContainer.Q("pause-control");

            _upControl.RegisterCallback<PointerDownEvent>(OnUpControlClick);
            _downControl.RegisterCallback<PointerDownEvent>(OnDownControlClick);
            _leftControl.RegisterCallback<PointerDownEvent>(OnLeftControlClick);
            _rightControl.RegisterCallback<PointerDownEvent>(OnRightControlClick);
            _pauseControl.RegisterCallback<PointerDownEvent>(OnPauseControlClick);

            _upControl.RegisterCallback<PointerUpEvent>(OnUpControlUp);
            _downControl.RegisterCallback<PointerUpEvent>(OnDownControlUp);
            _leftControl.RegisterCallback<PointerUpEvent>(OnLeftControlUp);
            _rightControl.RegisterCallback<PointerUpEvent>(OnRightControlUp);

            _upControl.RegisterCallback<PointerLeaveEvent>(OnUpControlLeave);
            _downControl.RegisterCallback<PointerLeaveEvent>(OnDownControlLeave);
            _leftControl.RegisterCallback<PointerLeaveEvent>(OnLeftControlLeave);
            _rightControl.RegisterCallback<PointerLeaveEvent>(OnRightControlLeave);
        }

        livesCounter = _hudContainer.Q("lives-block").Q<Label>(className: "hud__counter");
        waveCounter = _hudContainer.Q("wave-block").Q<Label>(className: "hud__counter");

        _endGameWindow = uiDocument.rootVisualElement.Q("end-game-window");
        _endGameWindowContent = _endGameWindow.Q("popup-content");

        _endGameContainer = _endGameWindow.Q("end-game-container");
        _endGameBlock = _endGameContainer.Q("end-game-block");
        _endGameSecondChanceBlock = _endGameContainer.Q("second-chance-block");
        _endGameRepeatBtn = _endGameContainer.Q<Button>("btn-yes");
        _endGameGoToMenuBtn = _endGameContainer.Q<Button>("btn-no");

        _mainMenuResumeGameBtn.RegisterCallback<ClickEvent>(OnResumeBtnClick);
        _mainMenuSettingsGameBtn.RegisterCallback<ClickEvent>(OnSettingsBtnClick);
        _mainMenuGoToMenuGameBtn.RegisterCallback<ClickEvent>(OnGoToMenuBtnClick);

        _endGameRepeatBtn.RegisterCallback<ClickEvent>(OnRepeatGameBtnClick);
        _endGameGoToMenuBtn.RegisterCallback<ClickEvent>(OnGoToMenuBtnClick);
    }

    private void OnDisable()
    {
        _mainMenuResumeGameBtn.UnregisterCallback<ClickEvent>(OnResumeBtnClick);
        _mainMenuSettingsGameBtn.UnregisterCallback<ClickEvent>(OnSettingsBtnClick);
        _mainMenuGoToMenuGameBtn.UnregisterCallback<ClickEvent>(OnGoToMenuBtnClick);

        _endGameRepeatBtn.UnregisterCallback<ClickEvent>(OnRepeatGameBtnClick);
        _endGameGoToMenuBtn.UnregisterCallback<ClickEvent>(OnGoToMenuBtnClick);

        if (Platform.IsMobileBrowser())
        {
            _upControl.UnregisterCallback<PointerDownEvent>(OnUpControlClick);
            _downControl.UnregisterCallback<PointerDownEvent>(OnDownControlClick);
            _leftControl.UnregisterCallback<PointerDownEvent>(OnLeftControlClick);
            _rightControl.UnregisterCallback<PointerDownEvent>(OnRightControlClick);
            _pauseControl.UnregisterCallback<PointerDownEvent>(OnPauseControlClick);

            _upControl.UnregisterCallback<PointerUpEvent>(OnUpControlUp);
            _downControl.UnregisterCallback<PointerUpEvent>(OnDownControlUp);
            _leftControl.UnregisterCallback<PointerUpEvent>(OnLeftControlUp);
            _rightControl.UnregisterCallback<PointerUpEvent>(OnRightControlUp);

            _upControl.UnregisterCallback<PointerLeaveEvent>(OnUpControlLeave);
            _downControl.UnregisterCallback<PointerLeaveEvent>(OnDownControlLeave);
            _leftControl.UnregisterCallback<PointerLeaveEvent>(OnLeftControlLeave);
            _rightControl.UnregisterCallback<PointerLeaveEvent>(OnRightControlLeave);
        }

        GameSettingsManager.Instance.OnAcceptSettings -= GameSettingsManager_OnAcceptSettings;
        GameSettingsManager.Instance.OnCancelSettings -= GameSettingsManager_OnCancelSettings;

        GameManager.Instance.OnCountdawnStart -= GameManager_OnCountdawnStart;
        GameManager.Instance.OnGamePaused -= GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused -= GameManager_OnGameUnpaused;
        GameManager.Instance.OnGameOver -= GameManager_OnGameOver;
        GameManager.Instance.OnSecondChance -= GameManager_OnSecondChance;
    }

    private void Start()
    {
        GameSettingsManager.Instance.LoadGameSettings();

        GameSettingsManager.Instance.OnAcceptSettings += GameSettingsManager_OnAcceptSettings;
        GameSettingsManager.Instance.OnCancelSettings += GameSettingsManager_OnCancelSettings;

        GameManager.Instance.OnCountdawnStart += GameManager_OnCountdawnStart;
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
        GameManager.Instance.OnSecondChance += GameManager_OnSecondChance;

        if (!Platform.IsMobileBrowser())
        {
            _mobileControlsContainer.AddToClassList("hide");
        }

        livesCounter.text = PlayerController.Instance.lives.ToString();
        waveCounter.text = GameManager.Instance.wave.ToString();

        /// Нормально работает только с задержкой начала (иначе отключается сразу же) и продолжительностью, помноженной на 2.
        transitionPanelScript.StartTransition(false, 0.1f, GameManager.Instance.waitingTime * 2);
    }

    private void GameSettingsManager_OnCancelSettings(object sender, EventArgs e)
    {
        ToggleSettingMenu();
    }

    private void GameSettingsManager_OnAcceptSettings(object sender, EventArgs e)
    {
        ToggleSettingMenu();
    }

    private void OnUpControlClick(PointerDownEvent evt)
    {
        UpControlTrigger.Trigger(true);
    }
    private void OnDownControlClick(PointerDownEvent evt)
    {
        DownControlTrigger.Trigger(true);
    }
    private void OnLeftControlClick(PointerDownEvent evt)
    {
        LeftControlTrigger.Trigger(true);
    }
    private void OnRightControlClick(PointerDownEvent evt)
    {
        RightControlTrigger.Trigger(true);
    }
    private void OnPauseControlClick(PointerDownEvent evt)
    {
        PauseControlTrigger.Trigger();
    }

    private void OnUpControlUp(PointerUpEvent evt)
    {
        UpControlTrigger.Stop();
    }
    private void OnDownControlUp(PointerUpEvent evt)
    {
        DownControlTrigger.Stop();
    }
    private void OnLeftControlUp(PointerUpEvent evt)
    {
        LeftControlTrigger.Stop();
    }
    private void OnRightControlUp(PointerUpEvent evt)
    {
        RightControlTrigger.Stop();
    }

    private void OnUpControlLeave(PointerLeaveEvent evt)
    {
        UpControlTrigger.Stop();
    }
    private void OnDownControlLeave(PointerLeaveEvent evt)
    {
        DownControlTrigger.Stop();
    }
    private void OnLeftControlLeave(PointerLeaveEvent evt)
    {
        LeftControlTrigger.Stop();
    }
    private void OnRightControlLeave(PointerLeaveEvent evt)
    {
        RightControlTrigger.Stop();
    }

    private void GameManager_OnGameOver(object sender, EventArgs e)
    {
        _endGameBlock.ToggleInClassList("hide");
        _endGameSecondChanceBlock.ToggleInClassList("hide");
        _endGameWindow.RemoveFromClassList("popup-hidden");
        _endGameWindowContent.RemoveFromClassList("animation__hide");
        _endGameWindow.AddToClassList("popup-show");
        _endGameWindowContent.AddToClassList("animation__show");
    }

    private void GameManager_OnSecondChance(object sender, EventArgs e)
    {
        _endGameWindow.AddToClassList("popup-hidden");
        _endGameWindowContent.AddToClassList("animation__hide");
        _endGameWindow.RemoveFromClassList("popup-show");
        _endGameWindowContent.RemoveFromClassList("animation__show");
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
