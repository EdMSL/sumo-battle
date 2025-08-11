using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    private VisualElement _menuContainer;
    private VisualElement _settingsContainer;
    private VisualElement _btnBlock;
    private VisualElement _difficultyContainer;
    private VisualElement _levelContainer;
    private VisualElement _customizationContainer;
    private VisualElement _levelButtonsBlock;
    private Image _gameTitle;
    private Button _acceptButton;
    private Button _cancelButton;
    private Button _playButton;
    private Button _settingsButton;
    private Button _customizationBackButton;
    private Button _customizationOkButton;
    private Button _levelOkButton;
    private Button _levelBackButton;
    private Button _difficultyBackButton;
    private Button _difficultyEasyButton;
    private Button _difficultyNormalButton;
    private Button _difficultyHardButton;

    private UIDocument uiDocument;
    private int selectedLevelIndex;

    private void OnEnable()
    {
        uiDocument = GetComponent<UIDocument>();

        _menuContainer = uiDocument.rootVisualElement.Q("menu-container");
        _settingsContainer = uiDocument.rootVisualElement.Q("settings-container");
        _btnBlock = _menuContainer.Q("btns-block");
        _difficultyContainer = _menuContainer.Q("difficult-container");
        _levelContainer = _menuContainer.Q("level-container");
        _customizationContainer = _menuContainer.Q("customization-container");

        _cancelButton = _settingsContainer.Q<Button>("cancel-btn");
        _acceptButton = _settingsContainer.Q<Button>("accept-btn");

        _playButton = _btnBlock.Q<Button>("play-btn");
        _settingsButton = _btnBlock.Q<Button>("settings-btn");
        _customizationOkButton = _customizationContainer.Q<Button>("ok-btn");
        _customizationBackButton = _customizationContainer.Q<Button>("back-btn");
        _levelOkButton = _levelContainer.Q<Button>("ok-btn");
        _levelBackButton = _levelContainer.Q<Button>("back-btn");
        _difficultyBackButton = _difficultyContainer.Q<Button>("back-btn");
        _difficultyEasyButton = _difficultyContainer.Q<Button>("easy-btn");
        _difficultyNormalButton = _difficultyContainer.Q<Button>("normal-btn");
        _difficultyHardButton = _difficultyContainer.Q<Button>("hard-btn");

        _gameTitle = uiDocument.rootVisualElement.Q("game-title") as Image;

        _playButton.RegisterCallback<ClickEvent>(OnPlayBtnClick);
        _settingsButton.RegisterCallback<ClickEvent>(OnSettingsBtnClick);
        _acceptButton.RegisterCallback<ClickEvent>(OnCancelBtnClick);
        _cancelButton.RegisterCallback<ClickEvent>(OnCancelBtnClick);

        _customizationBackButton.RegisterCallback<ClickEvent>(OnCustomizationBackBtnClick);
        _customizationOkButton.RegisterCallback<ClickEvent>(OnCustomizationOkBtnClick);

        var localizedTexture = new LocalizedTexture { TableReference = "Game Assets", TableEntryReference = "title-img" };
        _gameTitle.SetBinding("image", localizedTexture);

        _levelButtonsBlock = _levelContainer.Q("level-btns-block");

        _levelOkButton.RegisterCallback<ClickEvent>(OnLevelOkBtnClick);
        _levelBackButton.RegisterCallback<ClickEvent>(OnLevelBackBtnClick);

        _difficultyBackButton.RegisterCallback<ClickEvent>(OnDifficultyBackBtnClick);
        _difficultyEasyButton.RegisterCallback<ClickEvent>(OnDifficultyBtnClick);
        _difficultyNormalButton.RegisterCallback<ClickEvent>(OnDifficultyBtnClick);
        _difficultyHardButton.RegisterCallback<ClickEvent>(OnDifficultyBtnClick);

        // uiDocument.rootVisualElement.RegisterCallback<GeometryChangedEvent>(SetUISize);
    }

    IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            var btn = new Button() { text = $"{LocalizationSettings.StringDatabase.GetLocalizedString("game-level")} {i}" };
            btn.AddToClassList("menu__btn");
            btn.AddToClassList("main__btn");
            btn.RegisterCallback<ClickEvent>(OnLevelBtnClick);
            _levelButtonsBlock.Add(btn);
        }
    }

    private void SetUISize(GeometryChangedEvent evt)
    {
        var root = uiDocument.rootVisualElement.Q("root");
        root.ClearClassList();

        if (Screen.height >= 2159)
        {
            root.AddToClassList("ui_2160");
        }
        else if (Screen.height >= 1439)
        {
            root.AddToClassList("ui_1440");
        }
        else if (Screen.height <= 580)
        {
            root.AddToClassList("ui_576");
        }
        else if (Screen.height <= 770)
        {
            root.AddToClassList("ui_768");
        }
    }

    private void OnCustomizationOkBtnClick(ClickEvent evt)
    {
        _customizationContainer.style.display = DisplayStyle.None;
        _levelContainer.style.display = DisplayStyle.Flex;
    }

    private void OnCustomizationBackBtnClick(ClickEvent evt)
    {
        _customizationContainer.style.display = DisplayStyle.None;
        _gameTitle.style.display = DisplayStyle.Flex;
        _btnBlock.style.display = DisplayStyle.Flex;
    }

    private void OnLevelOkBtnClick(ClickEvent evt)
    {
        _levelContainer.style.display = DisplayStyle.None;
        _difficultyContainer.style.display = DisplayStyle.Flex;
    }

    private void OnLevelBtnClick(ClickEvent evt)
    {
        var button = (Button)evt.target;
        selectedLevelIndex = Int32.Parse(button.text.Split(' ')[1]);
    }

    private void OnLevelBackBtnClick(ClickEvent evt)
    {
        _levelContainer.style.display = DisplayStyle.None;
        _customizationContainer.style.display = DisplayStyle.Flex;
    }

    private void OnDifficultyBtnClick(ClickEvent evt)
    {
        var button = (Button)evt.target;
        var btnName = button.name.Split('-')[0];
        GameManager.Instance.SetDifficultyLevel(btnName);
        GameManager.Instance.StartGame(selectedLevelIndex);
    }

    private void OnDifficultyBackBtnClick(ClickEvent evt)
    {
        _btnBlock.style.display = DisplayStyle.Flex;
        _difficultyContainer.style.display = DisplayStyle.None;
    }

    private void OnCancelBtnClick(ClickEvent evt)
    {
        _settingsContainer.style.display = DisplayStyle.None;
        _btnBlock.style.display = DisplayStyle.Flex;
    }

    private void OnSettingsBtnClick(ClickEvent evt)
    {
        _btnBlock.style.display = DisplayStyle.None;
        _settingsContainer.style.display = DisplayStyle.Flex;
    }

    private void OnPlayBtnClick(ClickEvent evt)
    {
        _btnBlock.style.display = DisplayStyle.None;
        _gameTitle.style.display = DisplayStyle.None;
        _customizationContainer.style.display = DisplayStyle.Flex;
    }

    private void OnDisable()
    {
        _playButton.UnregisterCallback<ClickEvent>(OnPlayBtnClick);
        _difficultyBackButton.UnregisterCallback<ClickEvent>(OnDifficultyBackBtnClick);
        _settingsButton.UnregisterCallback<ClickEvent>(OnSettingsBtnClick);
        _cancelButton.UnregisterCallback<ClickEvent>(OnCancelBtnClick);
        _acceptButton.UnregisterCallback<ClickEvent>(OnCancelBtnClick);
        _difficultyEasyButton.UnregisterCallback<ClickEvent>(OnDifficultyBtnClick);
        _difficultyNormalButton.UnregisterCallback<ClickEvent>(OnDifficultyBtnClick);
        _difficultyHardButton.UnregisterCallback<ClickEvent>(OnDifficultyBtnClick);
        _customizationBackButton.UnregisterCallback<ClickEvent>(OnCustomizationBackBtnClick);
        _customizationOkButton.UnregisterCallback<ClickEvent>(OnCustomizationOkBtnClick);
        _levelBackButton.UnregisterCallback<ClickEvent>(OnLevelBackBtnClick);

        _levelButtonsBlock.Query<Button>(className: "menu__btn").ForEach(elem => elem.UnregisterCallback<ClickEvent>(OnLevelBtnClick));
    }
}
