#if ENABLE_INPUT_SYSTEM
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

public class OnScreenControlTrigger : OnScreenControl
{
    public string ControlPath;
    public float Value = 1f;

    protected override string controlPathInternal
    {
        get => ControlPath;
        set => ControlPath = value;
    }

    public void Trigger()
    {
        if (!string.IsNullOrEmpty(ControlPath))
            StartCoroutine(triggerEvent());
    }

    private IEnumerator triggerEvent()
    {
        yield return null;
        SentDefaultValueToControl();

        yield return null;
        SendValueToControl<float>(Value);

        yield return null;
        SentDefaultValueToControl();
    }
}
#endif