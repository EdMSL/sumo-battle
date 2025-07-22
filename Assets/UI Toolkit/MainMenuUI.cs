using System;
using UnityEngine;
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
    private Image _gameTitle;
    private Button _playButton;
    private Button _settingsButton;
    private Button _difficultyBackButton;
    private Button _acceptButton;
    private Button _cancelButton;
    private Button _difficultyEasyButton;
    private Button _difficultyNormalButton;
    private Button _difficultyHardButton;

    private UIDocument uiDocument;

    private const int UI_DEFAULT_WIDTH = 1080;

    private void OnEnable()
    {
        uiDocument = GetComponent<UIDocument>();

        _menuContainer = uiDocument.rootVisualElement.Q("menu-container");
        _settingsContainer = uiDocument.rootVisualElement.Q("settings-container");
        _btnBlock = _menuContainer.Q("btns-block");
        _difficultyContainer = _menuContainer.Q("difficult-container");
        _levelContainer = _menuContainer.Q("level-container");

        _playButton = _btnBlock.Q<Button>("play-btn");
        _settingsButton = _btnBlock.Q<Button>("settings-btn");
        _difficultyBackButton = _difficultyContainer.Q<Button>("back-btn");
        _cancelButton = _settingsContainer.Q<Button>("cancel-btn");
        _acceptButton = _settingsContainer.Q<Button>("accept-btn");
        _difficultyEasyButton = _difficultyContainer.Q<Button>("easy-btn");
        _difficultyNormalButton = _difficultyContainer.Q<Button>("normal-btn");
        _difficultyHardButton = _difficultyContainer.Q<Button>("hard-btn");

        _gameTitle = uiDocument.rootVisualElement.Q("game-title") as Image;

        Debug.Log(SceneManager.sceneCount);

        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            var btn = new Button() { text = $"{LocalizationSettings.StringDatabase.GetLocalizedString("game-level")} {i}" };
            btn.AddToClassList("menu__btn");
            btn.RegisterCallback<ClickEvent>(OnLevelBtnClick);
            _levelContainer.Add(btn);
        }

        _playButton.RegisterCallback<ClickEvent>(OnPlayBtnClick);
        _difficultyBackButton.RegisterCallback<ClickEvent>(OnBackBtnClick);
        _settingsButton.RegisterCallback<ClickEvent>(OnSettingsBtnClick);
        _cancelButton.RegisterCallback<ClickEvent>(OnCancelBtnClick);
        _acceptButton.RegisterCallback<ClickEvent>(OnCancelBtnClick);
        _difficultyEasyButton.RegisterCallback<ClickEvent>(OnDifficultyBtnClick);
        _difficultyNormalButton.RegisterCallback<ClickEvent>(OnDifficultyBtnClick);
        _difficultyHardButton.RegisterCallback<ClickEvent>(OnDifficultyBtnClick);

        uiDocument.rootVisualElement.RegisterCallback<GeometryChangedEvent>(OnRootGeometryChange);
    }

    private void OnRootGeometryChange(GeometryChangedEvent evt)
    {
        if (uiDocument.rootVisualElement.resolvedStyle.height != UI_DEFAULT_WIDTH)
        {
            float ratio = Math.Abs(UI_DEFAULT_WIDTH / uiDocument.rootVisualElement.resolvedStyle.height);
            Debug.Log(ratio);

        }
    }

    private void OnLevelBtnClick(ClickEvent evt)
    {
        var btn = (Button)evt.target;

        GameManager.Instance.StartGame(Int32.Parse(btn.text.Split(' ')[1]));
    }

    private void OnDifficultyBtnClick(ClickEvent evt)
    {
        var button = (Button)evt.target;
        var btnName = button.name.Split('-')[0];
        GameManager.Instance.SetDifficultyLevel(btnName);
        _difficultyContainer.style.display = DisplayStyle.None;
        _levelContainer.style.display = DisplayStyle.Flex;
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
        _difficultyContainer.style.display = DisplayStyle.Flex;
    }

    private void OnBackBtnClick(ClickEvent evt)
    {
        _btnBlock.style.display = DisplayStyle.Flex;
        _difficultyContainer.style.display = DisplayStyle.None;
    }

    private void OnDisable()
    {
        _playButton.UnregisterCallback<ClickEvent>(OnPlayBtnClick);
        _difficultyBackButton.UnregisterCallback<ClickEvent>(OnBackBtnClick);
        _settingsButton.UnregisterCallback<ClickEvent>(OnSettingsBtnClick);
        _cancelButton.UnregisterCallback<ClickEvent>(OnCancelBtnClick);
        _acceptButton.UnregisterCallback<ClickEvent>(OnCancelBtnClick);
        _difficultyEasyButton.UnregisterCallback<ClickEvent>(OnDifficultyBtnClick);
        _difficultyNormalButton.UnregisterCallback<ClickEvent>(OnDifficultyBtnClick);
        _difficultyHardButton.UnregisterCallback<ClickEvent>(OnDifficultyBtnClick);
    }
}
