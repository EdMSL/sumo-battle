using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using JSAM;

public class GameUI : MonoBehaviour
{
    private VisualElement _settingsWindow;
    private Button _settingsAcceptButton;
    private Button _settingsCancelButton;
    private VisualElement _settingsWindowContent;
    private VisualElement _playerContainer;
    public VisualElement countdownContainer;
    public VisualElement menuContainer;
    public Label livesCounter;
    public Label waveCounter;
    public Label countdownText;
    public Button repeatGameBtn;
    public Button goToMenuBtn;

    public Button menuTestBtn;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        _settingsWindow = uiDocument.rootVisualElement.Q("settings-window");
        _settingsWindowContent = _settingsWindow.Q("popup-content");

        _playerContainer = uiDocument.rootVisualElement.Query<VisualElement>("hud-container");

        _playerContainer = uiDocument.rootVisualElement.Q("hud-container");
        countdownContainer = uiDocument.rootVisualElement.Q("countdown-container");
        menuContainer = uiDocument.rootVisualElement.Q("menu-container");

        livesCounter = _playerContainer.Q("lives-block").Q<Label>(className: "hud__counter");
        waveCounter = _playerContainer.Q("wave-block").Q<Label>(className: "hud__counter");
        countdownText = countdownContainer.Q<Label>("countdown-text");

        menuTestBtn = uiDocument.rootVisualElement.Q<Button>("menu-test-btn");

        _settingsCancelButton = _settingsWindow.Q<Button>("cancel-btn");
        _settingsAcceptButton = _settingsWindow.Q<Button>("accept-btn");

        repeatGameBtn = menuContainer.Q<Button>("btn-yes");
        goToMenuBtn = menuContainer.Q<Button>("btn-no");

        repeatGameBtn.RegisterCallback<ClickEvent>(OnRepeatGameBtnClick);
        goToMenuBtn.RegisterCallback<ClickEvent>(OnGoToMenuBtnClick);

        menuTestBtn.RegisterCallback<ClickEvent>(OnSettingsBtnClick);

        _settingsAcceptButton.RegisterCallback<ClickEvent>(OnSettingsBtnClick);
        _settingsCancelButton.RegisterCallback<ClickEvent>(OnSettingsBtnClick);

        livesCounter.text = PlayerController.Instance.lives.ToString();
        waveCounter.text = GameManager.Instance.wave.ToString();
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

        if (_settingsWindow.ClassListContains("popup-show"))
        {
            StartCoroutine(UIDelay());
        }
        else
        {
            _settingsWindow.RemoveFromClassList("popup-hidden");
            _settingsWindowContent.RemoveFromClassList("animation__hide");
            _settingsWindow.AddToClassList("popup-show");
            _settingsWindowContent.AddToClassList("animation__show");
        }
    }

    IEnumerator UIDelay()
    {
        _settingsWindowContent.AddToClassList("animation__hide");
        _settingsWindowContent.RemoveFromClassList("animation__show");

        yield return new WaitForSeconds(0.3f);

        _settingsWindow.RemoveFromClassList("popup-show");
        _settingsWindow.AddToClassList("popup-hidden");
    }
}
