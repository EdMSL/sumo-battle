using System;
using JSAM;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class GameSettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    private Slider masterVolumeSlider, soundVolumeSlider, musicVolumeSlider;
    private Toggle soundSwitcher, musicSwitcher, shadowsSwitcher;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement.Q("root");

        masterVolumeSlider = root.Q<Slider>("master-volume");
        soundVolumeSlider = root.Q<Slider>("sound-volume");
        musicVolumeSlider = root.Q<Slider>("music-volume");

        soundSwitcher = root.Q<Toggle>("sound-switcher");
        musicSwitcher = root.Q<Toggle>("music-switcher");
        shadowsSwitcher = root.Q<Toggle>("shadows-switcher");

        masterVolumeSlider.RegisterValueChangedCallback(ChangeVolume);
        soundVolumeSlider.RegisterValueChangedCallback(ChangeVolume);
        musicVolumeSlider.RegisterValueChangedCallback(ChangeVolume);

        soundSwitcher.RegisterValueChangedCallback(Mute);
        musicSwitcher.RegisterValueChangedCallback(Mute);
        shadowsSwitcher.RegisterValueChangedCallback(ToggleShadows);
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
