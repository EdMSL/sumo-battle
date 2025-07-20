using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    private VisualElement _menuContainer;
    private VisualElement _settingsContainer;
    private VisualElement _btnsContainer;
    private VisualElement _difficultyContainer;
    private Button _playButton;
    private Button _settingsButton;
    private Button _backButton;
    private Button _acceptButton;
    private Button _cancelButton;
    private Button _difficultyEasyButton;
    private Button _difficultyNormalButton;
    private Button _difficultyHardButton;


    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        _menuContainer = uiDocument.rootVisualElement.Q("menu-container");
        _settingsContainer = uiDocument.rootVisualElement.Q("settings-container");
        _btnsContainer = _menuContainer.Q("btns-container");
        _difficultyContainer = _menuContainer.Q("difficult-container");

        _playButton = _btnsContainer.Q<Button>("play-btn");
        _settingsButton = _btnsContainer.Q<Button>("settings-btn");
        _backButton = _difficultyContainer.Q<Button>("back-btn");
        _cancelButton = _settingsContainer.Q<Button>("cancel-btn");
        _acceptButton = _settingsContainer.Q<Button>("accept-btn");
        _difficultyEasyButton = _difficultyContainer.Q<Button>("easy-btn");
        _difficultyNormalButton = _difficultyContainer.Q<Button>("normal-btn");
        _difficultyHardButton = _difficultyContainer.Q<Button>("hard-btn");

        _playButton.RegisterCallback<ClickEvent>(OnPlayBtnClick);
        _backButton.RegisterCallback<ClickEvent>(OnBackBtnClick);
        _settingsButton.RegisterCallback<ClickEvent>(OnSettingsBtnClick);
        _cancelButton.RegisterCallback<ClickEvent>(OnCancelBtnClick);
        _acceptButton.RegisterCallback<ClickEvent>(OnCancelBtnClick);
        _difficultyEasyButton.RegisterCallback<ClickEvent>(OnDifficultyBtnClick);
        _difficultyNormalButton.RegisterCallback<ClickEvent>(OnDifficultyBtnClick);
        _difficultyHardButton.RegisterCallback<ClickEvent>(OnDifficultyBtnClick);
    }

    private void OnDifficultyBtnClick(ClickEvent evt)
    {
        var button = (Button)evt.target;
        var btnName = button.name.Split('-')[0];

        GameManager.Instance.StartGame(btnName);
    }

    private void OnCancelBtnClick(ClickEvent evt)
    {
        _settingsContainer.style.display = DisplayStyle.None;
        _btnsContainer.style.display = DisplayStyle.Flex;
    }

    private void OnSettingsBtnClick(ClickEvent evt)
    {
        _btnsContainer.style.display = DisplayStyle.None;
        _settingsContainer.style.display = DisplayStyle.Flex;
    }

    private void OnPlayBtnClick(ClickEvent evt)
    {
        _btnsContainer.style.display = DisplayStyle.None;
        _difficultyContainer.style.display = DisplayStyle.Flex;
    }

    private void OnBackBtnClick(ClickEvent evt)
    {
        _btnsContainer.style.display = DisplayStyle.Flex;
        _difficultyContainer.style.display = DisplayStyle.None;
    }

    private void OnDisable()
    {
        _playButton.UnregisterCallback<ClickEvent>(OnPlayBtnClick);
        _backButton.UnregisterCallback<ClickEvent>(OnBackBtnClick);
        _settingsButton.UnregisterCallback<ClickEvent>(OnSettingsBtnClick);
        _cancelButton.UnregisterCallback<ClickEvent>(OnCancelBtnClick);
        _acceptButton.UnregisterCallback<ClickEvent>(OnCancelBtnClick);
    }
}
