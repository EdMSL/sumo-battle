using UnityEngine;
using System.Runtime.InteropServices;

public class AdsYandex : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowFullscreen();
    [DllImport("__Internal")]
    private static extern void ShowRewarded();
    [DllImport("__Internal")]
    private static extern void StartGameplayAPI();
    [DllImport("__Internal")]
    private static extern void StopGameplayAPI();

    public void StartYandexAPI()
    {
        StartGameplayAPI();
    }

    public void StopYandexAPI()
    {
        StopGameplayAPI();
    }

    public void Show1()
    {
        ShowFullscreen();
    }

    public void Show2()
    {
        ShowRewarded();
    }

    public void AdsCoints()
    {
        GameManager.Instance.RepeatGameAfterRevard();
    }
}