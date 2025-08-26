#if ENABLE_INPUT_SYSTEM
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class OnScreenControlTrigger : OnScreenControl
{
    [InputControl(layout = "Button")]
    public string ControlPath;
    public float Value = 1f;
    public bool isMove = false;

    private Coroutine coroutine;

    protected override string controlPathInternal
    {
        get => ControlPath;
        set => ControlPath = value;
    }

    public void Trigger(bool isHold = false)
    {
        if (!string.IsNullOrEmpty(ControlPath))
            StartCoroutine(triggerEvent(isHold));
    }

    public void Stop()
    {
        SentDefaultValueToControl();
    }

    private IEnumerator triggerEvent(bool isHold = false)
    {
        yield return null;
        SentDefaultValueToControl();

        yield return null;
        SendValueToControl<float>(Value);

        if (!isHold)
        {
            yield return null;
            SentDefaultValueToControl();
        }
    }
}
#endif