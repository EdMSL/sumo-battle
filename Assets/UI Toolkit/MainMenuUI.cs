using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;
using JSAM;
using AlpaSunFade;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Transform styleMenu;
    [SerializeField] private ListOfSkins skinsList;
    [SerializeField] private ListOfLevels levelsList;
    [SerializeField] private MeshRenderer playerMR;
    [SerializeField] TransitionPanel transitionPanelScript;
    [SerializeField] float fadeDuration = 3f;

    private VisualElement _menuContainer;
    private VisualElement _settingsWindow;
    private VisualElement _settingsWindowContent;
    private VisualElement _btnBlock;
    private VisualElement _difficultyContainer;
    private VisualElement _levelContainer;
    private VisualElement _customizationContainer;
    private RadioButtonGroup _customizationSkinsList;
    private RadioButtonGroup _levelsList;
    private Image _gameTitle;
    private Button _settingsAcceptButton;
    private Button _settingsCancelButton;
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
    private UIDocument transitionPanel;
    private int selectedLevelIndex;
    private AsyncOperationHandle<Locale> m_InitializeOperation;
    private int localeSelected;

    private void OnEnable()
    {
        uiDocument = GetComponent<UIDocument>();

        _menuContainer = uiDocument.rootVisualElement.Q("menu-container");
        _settingsWindow = uiDocument.rootVisualElement.Q("settings-window");
        _settingsWindowContent = _settingsWindow.Q("popup-content");
        _btnBlock = _menuContainer.Q("btns-block");
        _difficultyContainer = _menuContainer.Q("difficult-container");
        _levelContainer = _menuContainer.Q("level-container");
        _customizationContainer = _menuContainer.Q("customization-container");

        _settingsCancelButton = _settingsWindow.Q<Button>("cancel-btn");
        _settingsAcceptButton = _settingsWindow.Q<Button>("accept-btn");

        _playButton = _btnBlock.Q<Button>("play-btn");
        _settingsButton = _btnBlock.Q<Button>("settings-btn");
        _customizationSkinsList = _customizationContainer.Q<RadioButtonGroup>("skins-list");
        _customizationOkButton = _customizationContainer.Q<Button>("ok-btn");
        _customizationBackButton = _customizationContainer.Q<Button>("back-btn");
        _levelsList = _levelContainer.Q<RadioButtonGroup>("levels-list");
        _levelOkButton = _levelContainer.Q<Button>("ok-btn");
        _levelBackButton = _levelContainer.Q<Button>("back-btn");
        _difficultyBackButton = _difficultyContainer.Q<Button>("back-btn");
        _difficultyEasyButton = _difficultyContainer.Q<Button>("easy-btn");
        _difficultyNormalButton = _difficultyContainer.Q<Button>("normal-btn");
        _difficultyHardButton = _difficultyContainer.Q<Button>("hard-btn");

        _gameTitle = uiDocument.rootVisualElement.Q("game-title") as Image;

        transitionPanel = transitionPanelScript.GetComponent<UIDocument>();

        _playButton.RegisterCallback<ClickEvent>(OnPlayBtnClick);
        _settingsButton.RegisterCallback<ClickEvent>(OnSettingsBtnClick);
        _settingsAcceptButton.RegisterCallback<ClickEvent>(OnSettingsBtnClick);
        _settingsCancelButton.RegisterCallback<ClickEvent>(OnSettingsBtnClick);

        _customizationSkinsList.RegisterValueChangedCallback(OnSkinRadiobuttonChange);
        _customizationBackButton.RegisterCallback<ClickEvent>(OnCustomizationBackBtnClick);
        _customizationOkButton.RegisterCallback<ClickEvent>(OnCustomizationOkBtnClick);

        var localizedTexture = new LocalizedTexture { TableReference = "Game Assets", TableEntryReference = "title-img" };
        _gameTitle.SetBinding("image", localizedTexture);

        _levelsList.RegisterValueChangedCallback(OnLevelRadiobuttonChange);
        _levelOkButton.RegisterCallback<ClickEvent>(OnLevelOkBtnClick);
        _levelBackButton.RegisterCallback<ClickEvent>(OnLevelBackBtnClick);

        _difficultyBackButton.RegisterCallback<ClickEvent>(OnDifficultyBackBtnClick);
        _difficultyEasyButton.RegisterCallback<ClickEvent>(OnDifficultyBtnClick);
        _difficultyNormalButton.RegisterCallback<ClickEvent>(OnDifficultyBtnClick);
        _difficultyHardButton.RegisterCallback<ClickEvent>(OnDifficultyBtnClick);
    }

    IEnumerator Start()
    {
        SwitchUIElementsOnStart();

        yield return LocalizationSettings.InitializationOperation;

        m_InitializeOperation = LocalizationSettings.SelectedLocaleAsync;

        if (m_InitializeOperation.IsDone)
        {
            for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
            {
                var locale = LocalizationSettings.AvailableLocales.Locales[i];
                if (LocalizationSettings.SelectedLocale == locale)
                {
                    localeSelected = i;
                }
            }
        }

        GameSettingsManager.Instance.OnAcceptSettings += GameSettingsManager_OnAcceptSettings;
        GameSettingsManager.Instance.OnCancelSettings += GameSettingsManager_OnCancelSettings;

        transitionPanel.gameObject.SetActive(false);

        _levelsList.Clear();

        for (int i = 0; i < levelsList.Levels.Count; i++)
        {
            var levelElement = new RadioButton() { };
            levelElement.AddToClassList("list__item");
            levelElement.AddToClassList("levels__item");

            if (i == levelsList.Levels.Count - 1)
            {
                levelElement.AddToClassList("last-child");
            }

            levelElement.style.backgroundImage = levelsList.Levels[i].image;
            _levelsList.Add(levelElement);
        }

        _levelsList.value = 0;

        _customizationSkinsList.Clear();

        for (int i = 0; i < skinsList.Skins.Count; i++)
        {
            var skinElement = new RadioButton() { };
            skinElement.AddToClassList("list__item");
            skinElement.AddToClassList("skins__item");

            if (i == skinsList.Skins.Count - 1)
            {
                skinElement.AddToClassList("last-child");
            }

            skinElement.style.backgroundImage = skinsList.Skins[i].image;
            _customizationSkinsList.Add(skinElement);
        }

        _customizationSkinsList.value = 0;
    }

    private IEnumerator StartGame(string btnName)
    {
        transitionPanel.gameObject.SetActive(true);
        transitionPanel.sortingOrder = 1;
        /// Нормально работает только с задержкой начала (иначе отклбючается сразу же) и продолжительностью, помноженной на 2.
        transitionPanelScript.StartTransition(true, 0.1f, fadeDuration * 2);

        yield return new WaitForSeconds(fadeDuration);

        GameManager.Instance.SetDifficultyLevel(btnName);
        GameManager.Instance.StartGame(selectedLevelIndex);
    }

    private void GameSettingsManager_OnCancelSettings(object sender, EventArgs e)
    {
        OnSettingsBtnClick(null);
    }

    private void GameSettingsManager_OnAcceptSettings(object sender, EventArgs e)
    {
        OnSettingsBtnClick(null);
    }

    private void OnPlayBtnClick(ClickEvent evt)
    {
        AudioManager.PlaySound(GameAudioLibrarySounds.click);

        _btnBlock.style.display = DisplayStyle.None;
        _gameTitle.style.display = DisplayStyle.None;
        _customizationContainer.style.display = DisplayStyle.Flex;
        styleMenu.gameObject.SetActive(true);
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

    private void OnSkinRadiobuttonChange(ChangeEvent<int> evt)
    {
        AudioManager.PlaySound(GameAudioLibrarySounds.click);

        if (evt.newValue >= 0)
        {
            GameManager.Instance.SetPlayerSkin(skinsList.Skins[evt.newValue].material);
            playerMR.material = skinsList.Skins[evt.newValue].material;
        }
    }

    private void OnCustomizationOkBtnClick(ClickEvent evt)
    {
        AudioManager.PlaySound(GameAudioLibrarySounds.click);

        _customizationContainer.style.display = DisplayStyle.None;
        _levelContainer.style.display = DisplayStyle.Flex;

        styleMenu.gameObject.SetActive(false);

    }

    private void OnCustomizationBackBtnClick(ClickEvent evt)
    {
        AudioManager.PlaySound(GameAudioLibrarySounds.click);

        _customizationContainer.style.display = DisplayStyle.None;
        _gameTitle.style.display = DisplayStyle.Flex;
        _btnBlock.style.display = DisplayStyle.Flex;

        styleMenu.gameObject.SetActive(false);

    }

    private void OnLevelRadiobuttonChange(ChangeEvent<int> evt)
    {
        AudioManager.PlaySound(GameAudioLibrarySounds.click);

        if (evt.newValue >= 0)
        {
            selectedLevelIndex = evt.newValue + 1;
        }
    }

    private void OnLevelOkBtnClick(ClickEvent evt)
    {
        AudioManager.PlaySound(GameAudioLibrarySounds.click);

        _levelContainer.style.display = DisplayStyle.None;
        _difficultyContainer.style.display = DisplayStyle.Flex;
    }

    private void OnLevelBackBtnClick(ClickEvent evt)
    {
        AudioManager.PlaySound(GameAudioLibrarySounds.click);

        _levelContainer.style.display = DisplayStyle.None;
        _customizationContainer.style.display = DisplayStyle.Flex;

        styleMenu.gameObject.SetActive(true);
    }

    private void OnDifficultyBtnClick(ClickEvent evt)
    {
        AudioManager.PlaySound(GameAudioLibrarySounds.click);

        var button = (Button)evt.target;
        var btnName = button.name.Split('-')[0];

        StartCoroutine(StartGame(btnName));
    }

    private void OnDifficultyBackBtnClick(ClickEvent evt)
    {
        AudioManager.PlaySound(GameAudioLibrarySounds.click);

        _btnBlock.style.display = DisplayStyle.Flex;
        _difficultyContainer.style.display = DisplayStyle.None;
    }

    private void OnCancelBtnClick(ClickEvent evt)
    {
        AudioManager.PlaySound(GameAudioLibrarySounds.click);

        _settingsWindow.style.display = DisplayStyle.None;
        _btnBlock.style.display = DisplayStyle.Flex;
    }

    IEnumerator UIDelay()
    {
        _settingsWindowContent.AddToClassList("animation__hide");
        _settingsWindowContent.RemoveFromClassList("animation__show");

        yield return new WaitForSeconds(0.3f);

        _settingsWindow.RemoveFromClassList("popup-show");
        _settingsWindow.AddToClassList("popup-hidden");
    }

    private void SwitchUIElementsOnStart()
    {
        _btnBlock.style.display = DisplayStyle.Flex;
        _gameTitle.style.display = DisplayStyle.Flex;
        _customizationContainer.style.display = DisplayStyle.None;
        _levelContainer.style.display = DisplayStyle.None;
        _difficultyContainer.style.display = DisplayStyle.None;
        styleMenu.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _playButton.UnregisterCallback<ClickEvent>(OnPlayBtnClick);

        _settingsButton.UnregisterCallback<ClickEvent>(OnSettingsBtnClick);
        _settingsCancelButton.UnregisterCallback<ClickEvent>(OnCancelBtnClick);
        _settingsAcceptButton.UnregisterCallback<ClickEvent>(OnCancelBtnClick);

        _customizationOkButton.UnregisterCallback<ClickEvent>(OnCustomizationOkBtnClick);
        _customizationBackButton.UnregisterCallback<ClickEvent>(OnCustomizationBackBtnClick);
        _customizationSkinsList.UnregisterValueChangedCallback(OnSkinRadiobuttonChange);

        _difficultyEasyButton.UnregisterCallback<ClickEvent>(OnDifficultyBtnClick);
        _difficultyNormalButton.UnregisterCallback<ClickEvent>(OnDifficultyBtnClick);
        _difficultyHardButton.UnregisterCallback<ClickEvent>(OnDifficultyBtnClick);
        _difficultyBackButton.UnregisterCallback<ClickEvent>(OnDifficultyBackBtnClick);

        _levelsList.UnregisterValueChangedCallback(OnLevelRadiobuttonChange);
        _levelOkButton.UnregisterCallback<ClickEvent>(OnLevelOkBtnClick);
        _levelBackButton.UnregisterCallback<ClickEvent>(OnLevelBackBtnClick);

        GameSettingsManager.Instance.OnAcceptSettings -= GameSettingsManager_OnAcceptSettings;
        GameSettingsManager.Instance.OnCancelSettings -= GameSettingsManager_OnCancelSettings;
    }
}
