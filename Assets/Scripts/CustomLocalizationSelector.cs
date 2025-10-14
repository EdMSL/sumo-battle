using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Runtime.InteropServices;

[DisplayName("My Startup Selector")]
[Serializable]
public class CustomLocalizationSelector : IStartupLocaleSelector
{
    [DllImport("__Internal")]
    private static extern string GetLanguage();

    public Locale GetStartupLocale(ILocalesProvider availableLocales)
    {
        if (PlayerPrefs.GetInt("selected-locale", -1) != -1)
        {
            return availableLocales.GetLocale(PlayerPrefs.GetInt("selected-locale") == 0 ? "ru" : "en");
        }
        else
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            var langCode = GetLanguage().ToLower();

            if (langCode == "ru" || langCode == "be" || langCode == "kk" || langCode == "uk" || langCode == "uz")
            {
                return availableLocales.GetLocale("ru");
            }
            else
            {
                return availableLocales.GetLocale("en");
            }
#else
            return availableLocales.GetLocale("en");
#endif
        }
    }
}
