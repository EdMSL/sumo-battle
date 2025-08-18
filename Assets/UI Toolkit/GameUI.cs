using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using JSAM;

public class GameUI : MonoBehaviour
{
    private VisualElement _gameMenuWindow;
    private VisualElement _gameMenuWindowContent;
    private Button _gameMenuContinueGameBtn;
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

    public Button menuTestBtn;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        _gameMenuWindow = uiDocument.rootVisualElement.Q("game-menu-window");
        _gameMenuWindowContent = _gameMenuWindow.Q("popup-content");
        _gameMenuContinueGameBtn = _gameMenuWindowContent.Q<Button>("continue-btn");
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

        menuTestBtn = uiDocument.rootVisualElement.Q<Button>("menu-test-btn");

        // _settingsCancelButton = _settingsWindow.Q<Button>("cancel-btn");
        // _settingsAcceptButton = _settingsWindow.Q<Button>("accept-btn");

        repeatGameBtn = _endGameWindow.Q<Button>("btn-yes");
        goToMenuBtn = _endGameWindow.Q<Button>("btn-no");

        _gameMenuContinueGameBtn.RegisterCallback<ClickEvent>(OnTestBtnClick);
        _gameMenuSettingsGameBtn.RegisterCallback<ClickEvent>(OnSettingsBtnClick);
        _gameMenuGoToMenuGameBtn.RegisterCallback<ClickEvent>(OnGoToMenuBtnClick);

        repeatGameBtn.RegisterCallback<ClickEvent>(OnRepeatGameBtnClick);
        goToMenuBtn.RegisterCallback<ClickEvent>(OnGoToMenuBtnClick);

        menuTestBtn.RegisterCallback<ClickEvent>(OnTestBtnClick);

        // _settingsAcceptButton.RegisterCallback<ClickEvent>(OnSettingsBtnClick);
        // _settingsCancelButton.RegisterCallback<ClickEvent>(OnSettingsBtnClick);

        livesCounter.text = PlayerController.Instance.lives.ToString();
        waveCounter.text = GameManager.Instance.wave.ToString();
    }

    private void OnTestBtnClick(ClickEvent evt)
    {
        AudioManager.PlaySound(GameAudioLibrarySounds.click);

        if (_gameMenuWindow.ClassListContains("popup-show"))
        {
            StartCoroutine(UIDelay());
        }
        else
        {
            Time.timeScale = 0f;

            _gameMenuWindow.RemoveFromClassList("popup-hidden");
            _gameMenuWindowContent.RemoveFromClassList("animation__hide");
            _gameMenuWindow.AddToClassList("popup-show");
            _gameMenuWindowContent.AddToClassList("animation__show");
        }
    }

    private void OnGoToMenuBtnClick(ClickEvent evt)
    {
        GameManager.Instance.EndGame();
    }

    private void OnRepeatGameBtnClick(ClickEvent evt)
    {
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

        yield return new WaitForSeconds(0.3f);

        _gameMenuWindow.RemoveFromClassList("popup-show");
        _gameMenuWindow.AddToClassList("popup-hidden");
    }
}
