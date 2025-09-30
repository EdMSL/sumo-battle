using UnityEngine;
using System.Runtime.InteropServices;

public class AdsYandex : MonoBehaviour
{
    //   [SerializeField] private Level level;

    [DllImport("__Internal")]
    private static extern void ShowFullscreen();

    [DllImport("__Internal")]
    private static extern void ShowRewarded();

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
        GameManager.Instance.RepeatGame();
    }
}