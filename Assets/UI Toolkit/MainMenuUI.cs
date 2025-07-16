using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    private VisualElement _container;
    private VisualElement _btnsContainer;
    private VisualElement _difficultyContainer;
    private Button _playButton;
    private Button _settingsButton;
    private Button _backButton;


    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        _container = uiDocument.rootVisualElement.Q("menu-container");
        _btnsContainer = _container.Q("btns-container");
        _difficultyContainer = _container.Q("difficult-container");

        _playButton = _btnsContainer.Q<Button>("play-btn");
        _settingsButton = _btnsContainer.Q<Button>("settings-btn");
        _backButton = _difficultyContainer.Q<Button>("back-btn");

        _playButton.RegisterCallback<ClickEvent>(OnPlayBtnClick);
        _backButton.RegisterCallback<ClickEvent>(OnBackBtnClick);
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
    }
}
