using System;
using JSAM;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class GameSettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public static GameSettingsManager Instance { get; private set; }
    private Slider masterVolumeSlider, soundVolumeSlider, musicVolumeSlider;
    private Toggle soundSwitcher, musicSwitcher, shadowsSwitcher;
    private Button cancelBtn, acceptBtn;
    private ScrollView optionsList;

    public event EventHandler OnAcceptSettings;
    public event EventHandler OnCancelSettings;

    private float oldMasterVolume, oldSoundVolume, oldMusicVolume;
    private bool isSoundEnabledOld, isMusicEnabledOld, isShadowsEnabledOld;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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

        cancelBtn = root.Q<Button>("cancel-btn");
        acceptBtn = root.Q<Button>("accept-btn");

        masterVolumeSlider.RegisterValueChangedCallback(ChangeVolume);
        soundVolumeSlider.RegisterValueChangedCallback(ChangeVolume);
        musicVolumeSlider.RegisterValueChangedCallback(ChangeVolume);

        soundSwitcher.RegisterValueChangedCallback(Mute);
        musicSwitcher.RegisterValueChangedCallback(Mute);
        shadowsSwitcher.RegisterValueChangedCallback(ToggleShadows);

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

    private void Start()
    {
        UpdateOldSettingsValues();
    }

    private void OnCancelBtnClick(ClickEvent evt)
    {
        AudioManager.MasterVolume = oldMasterVolume;
        AudioManager.MusicVolume = oldMusicVolume;
        AudioManager.SoundVolume = oldSoundVolume;

        AudioManager.MusicMuted = isMusicEnabledOld;
        AudioManager.SoundMuted = isSoundEnabledOld;
        QualitySettings.shadows = isShadowsEnabledOld ? ShadowQuality.HardOnly : ShadowQuality.Disable;

        masterVolumeSlider.value = oldMasterVolume;
        soundVolumeSlider.value = oldMusicVolume;
        musicVolumeSlider.value = oldSoundVolume;

        soundSwitcher.value = isMusicEnabledOld;
        musicSwitcher.value = isSoundEnabledOld;
        shadowsSwitcher.value = isShadowsEnabledOld;

        optionsList.scrollOffset = new Vector2(optionsList.scrollOffset.x, 0);

        OnCancelSettings?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateOldSettingsValues()
    {
        oldMasterVolume = masterVolumeSlider.value;
        oldSoundVolume = soundVolumeSlider.value;
        oldMusicVolume = musicVolumeSlider.value;

        isSoundEnabledOld = soundSwitcher.value;
        isMusicEnabledOld = musicSwitcher.value;
        isShadowsEnabledOld = shadowsSwitcher.value;
    }

    private void OnAcceptBtnClick(ClickEvent evt)
    {
        UpdateOldSettingsValues();

        optionsList.scrollOffset = new Vector2(optionsList.scrollOffset.x, 0);

        OnAcceptSettings?.Invoke(this, EventArgs.Empty);
    }

    private void ToggleShadows(ChangeEvent<bool> evt)
    {
        QualitySettings.shadows = evt.newValue ? ShadowQuality.HardOnly : ShadowQuality.Disable;
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
                break;
            case "music":
                AudioManager.MusicMuted = !evt.newValue;
                musicVolumeSlider.SetEnabled(evt.newValue);
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
                break;
            case "sound":
                AudioManager.SoundVolume = evt.newValue;
                AudioManager.PlaySound(GameAudioLibrarySounds.heavyknock);
                break;
            case "music":
                AudioManager.MusicVolume = evt.newValue;
                break;
            default:
                break;
        }
    }
}
