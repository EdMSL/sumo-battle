public class Platform
{
    public static bool IsMobileBrowser()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
    return WebGLHandler.IsMobileBrowser(); // value based on the current browser
#endif

        return UnityEngine.Device.Application.isMobilePlatform;
    }
}