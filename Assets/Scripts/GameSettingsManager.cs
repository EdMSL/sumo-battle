using System;
using JSAM;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;

public class GameSettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AdsYandex YandexSDK;

    public static GameSettingsManager Instance { get; private set; }
    private Slider masterVolumeSlider, soundVolumeSlider, musicVolumeSlider;
    private Toggle soundSwitcher, musicSwitcher, shadowsSwitcher;
    private RadioButtonGroup languageSelector;
    private Button cancelBtn, acceptBtn;
    private ScrollView optionsList;

    public event EventHandler OnAcceptSettings;
    public event EventHandler OnCancelSettings;

    private float oldMasterVolume, oldSoundVolume, oldMusicVolume;
    private bool isSoundEnabledOld, isMusicEnabledOld, isShadowsEnabledOld;
    private int languageIndexOld;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (YandexSDK == null)
        {
            YandexSDK = FindFirstObjectByType<AdsYandex>();
        }
    }

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement.Q("root");

        optionsList = root.Q<ScrollView>("options-list");

        masterVolumeSlider = root.Q<Slider>("master-volume");
        soundVolumeSlider = root.Q<Slider>("sound-volume");
        musicVolumeSlider = root.Q<Slider>("music-volume");

        soundSwitcher = root.Q<Toggle>("sound-switcher");
        musicSwitcher = root.Q<Toggle>("music-switcher");
        shadowsSwitcher = root.Q<Toggle>("shadows-switcher");

        languageSelector = root.Q<RadioButtonGroup>("language-list");

        cancelBtn = root.Q<Button>("cancel-btn");
        acceptBtn = root.Q<Button>("accept-btn");

        masterVolumeSlider.RegisterValueChangedCallback(ChangeVolume);
        soundVolumeSlider.RegisterValueChangedCallback(ChangeVolume);
        musicVolumeSlider.RegisterValueChangedCallback(ChangeVolume);

        soundSwitcher.RegisterValueChangedCallback(Mute);
        musicSwitcher.RegisterValueChangedCallback(Mute);
        shadowsSwitcher.RegisterValueChangedCallback(ToggleShadows);

        languageSelector.RegisterValueChangedCallback(OnLanguageChange);

        acceptBtn.RegisterCallback<ClickEvent>(OnAcceptBtnClick);
        cancelBtn.RegisterCallback<ClickEvent>(OnCancelBtnClick);
    }

    private void OnDisable()
    {
        masterVolumeSlider.UnregisterValueChangedCallback(ChangeVolume);
        soundVolumeSlider.UnregisterValueChangedCallback(ChangeVolume);
        musicVolumeSlider.UnregisterValueChangedCallback(ChangeVolume);

        soundSwitcher.UnregisterValueChangedCallback(Mute);
        musicSwitcher.UnregisterValueChangedCallback(Mute);
        shadowsSwitcher.UnregisterValueChangedCallback(ToggleShadows);

        acceptBtn.UnregisterCallback<ClickEvent>(OnAcceptBtnClick);
        cancelBtn.UnregisterCallback<ClickEvent>(OnCancelBtnClick);
    }

    private void OnCancelBtnClick(ClickEvent evt)
    {
        AudioManager.MasterVolume = oldMasterVolume;
        AudioManager.MusicVolume = oldMusicVolume;
        AudioManager.SoundVolume = oldSoundVolume;

        AudioManager.MusicMuted = !isMusicEnabledOld;
        AudioManager.SoundMuted = !isSoundEnabledOld;
        QualitySettings.shadows = isShadowsEnabledOld ? ShadowQuality.HardOnly : ShadowQuality.Disable;

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageIndexOld];

        masterVolumeSlider.value = oldMasterVolume;
        soundVolumeSlider.value = oldSoundVolume;
        musicVolumeSlider.value = oldMusicVolume;

        musicSwitcher.value = isMusicEnabledOld;
        soundSwitcher.value = isSoundEnabledOld;
        shadowsSwitcher.value = isShadowsEnabledOld;

        languageSelector.SetValueWithoutNotify(languageIndexOld);

        optionsList.scrollOffset = new Vector2(optionsList.scrollOffset.x, 0);

        OnCancelSettings?.Invoke(this, EventArgs.Empty);
    }

    private void OnAcceptBtnClick(ClickEvent evt)
    {
        UpdateOldSettingsValues();

        optionsList.scrollOffset = new Vector2(optionsList.scrollOffset.x, 0);

        OnAcceptSettings?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateOldSettingsValues()
    {
        oldMasterVolume = masterVolumeSlider.value;
        oldSoundVolume = soundVolumeSlider.value;
        oldMusicVolume = musicVolumeSlider.value;

        isSoundEnabledOld = soundSwitcher.value;
        isMusicEnabledOld = musicSwitcher.value;
        isShadowsEnabledOld = shadowsSwitcher.value;

        languageIndexOld = languageSelector.value;
    }

    private void OnLanguageChange(ChangeEvent<int> evt)
    {
        Debug.Log("Trigger OnLanguageChange");

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[evt.newValue];
        PlayerPrefs.SetInt("selected-locale", evt.newValue);
    }

    private void ToggleShadows(ChangeEvent<bool> evt)
    {
        QualitySettings.shadows = evt.newValue ? ShadowQuality.HardOnly : ShadowQuality.Disable;
        PlayerPrefs.SetInt("settings-shadows", Convert.ToInt32(evt.newValue));
    }

    private void ToggleShadows(bool isEnabled)
    {
        QualitySettings.shadows = isEnabled ? ShadowQuality.HardOnly : ShadowQuality.Disable;
        shadowsSwitcher.value = isEnabled;
    }

    private void Mute(ChangeEvent<bool> evt)
    {
        var currentTargetElem = evt.currentTarget as VisualElement;
        string targetName = currentTargetElem.name.Split("-")[0];

        switch (targetName)
        {
            case "sound":
                AudioManager.SoundMuted = !evt.newValue;
                soundVolumeSlider.SetEnabled(evt.newValue);

                PlayerPrefs.SetInt("SOUND_ENABLED", Convert.ToInt32(evt.newValue));
                break;
            case "music":
                AudioManager.MusicMuted = !evt.newValue;
                musicVolumeSlider.SetEnabled(evt.newValue);
                PlayerPrefs.SetInt("MUSIC_ENABLED", Convert.ToInt32(evt.newValue));
                break;
            default:
                break;
        }
    }

    private void Mute(string name)
    {
        bool isEnabled;
        switch (name)
        {
            case "SOUND_ENABLED":
                isEnabled = Convert.ToBoolean(PlayerPrefs.GetInt("SOUND_ENABLED", 1));

                AudioManager.SoundMuted = !isEnabled;
                soundSwitcher.value = isEnabled;
                soundVolumeSlider.SetEnabled(isEnabled);
                break;
            case "MUSIC_ENABLED":
                isEnabled = Convert.ToBoolean(PlayerPrefs.GetInt("MUSIC_ENABLED", 1));

                AudioManager.MusicMuted = !isEnabled;
                musicSwitcher.value = isEnabled;
                musicVolumeSlider.SetEnabled(isEnabled);
                break;
            default:
                break;
        }
    }

    private void ChangeVolume(ChangeEvent<float> evt)
    {
        var currentTargetElem = evt.currentTarget as VisualElement;
        string targetName = currentTargetElem.name.Split("-")[0];

        switch (targetName)
        {
            case "master":
                AudioManager.MasterVolume = evt.newValue;
                PlayerPrefs.SetFloat("MASTER_VOL", evt.newValue);
                break;
            case "sound":
                AudioManager.SoundVolume = evt.newValue;
                PlayerPrefs.SetFloat("SOUND_VOL", evt.newValue);
                break;
            case "music":
                AudioManager.MusicVolume = evt.newValue;
                PlayerPrefs.SetFloat("MUSIC_VOL", evt.newValue);
                break;
            default:
                break;
        }
    }
    private void ChangeVolume(string name)
    {
        float volumeValue;
        switch (name)
        {
            case "MASTER_VOL":
                volumeValue = PlayerPrefs.GetFloat("MASTER_VOL", 1f);

                AudioManager.MasterVolume = volumeValue;
                masterVolumeSlider.value = volumeValue;
                break;
            case "SOUND_VOL":
                volumeValue = PlayerPrefs.GetFloat("SOUND_VOL", 1f);

                AudioManager.SoundVolume = volumeValue;
                soundVolumeSlider.value = volumeValue;
                break;
            case "MUSIC_VOL":
                volumeValue = PlayerPrefs.GetFloat("MUSIC_VOL", 1f);

                AudioManager.MusicVolume = volumeValue;
                musicVolumeSlider.value = volumeValue;
                break;
            default:
                break;
        }
    }

    public void LoadGameSettings()
    {
        SetGameLanguage();
        SetAudioSettings();
        ToggleShadows(Convert.ToBoolean(PlayerPrefs.GetInt("settings-shadows", 1)));
        UpdateOldSettingsValues();
    }

    private void SetAudioSettings()
    {
        ChangeVolume("MASTER_VOL");
        ChangeVolume("SOUND_VOL");
        ChangeVolume("MUSIC_VOL");
        Mute("SOUND_ENABLED");
        Mute("MUSIC_ENABLED");
    }

    private void SetGameLanguage()
    {
        var langIndex = 0;

        Locale currentSelectedLocale = LocalizationSettings.SelectedLocale;

        if (currentSelectedLocale != null)
        {
            int localeIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(currentSelectedLocale);

            if (localeIndex != -1)
            {
                langIndex = localeIndex;
            }
            else
            {
                Debug.LogWarning("Current locale not found in the list of available locales.");
            }
        }
        else
        {
            Debug.LogWarning("No locale is currently selected in LocalizationSettings.");
        }

        languageSelector.SetValueWithoutNotify(langIndex);
    }

    public bool GetIsMusicEnabled()
    {
        return isMusicEnabledOld;
    }
}
