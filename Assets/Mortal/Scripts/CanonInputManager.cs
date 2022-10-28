using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventDelegateContainer;

public class CanonInputManager : MonoBehaviour
{
    public event MouseInputSenderDelegate OnMouseButtonPress;
    public event MouseInputSenderDelegate OnMouseButtonRelease;

    private void Update()
    {
        InvokeMousePressEvents();
    }
    private void InvokeMousePressEvents()
    {
        if (Input.GetMouseButtonDown(1)) OnMouseButtonPress?.Invoke(1);
        if (Input.GetMouseButtonUp(1)) OnMouseButtonRelease?.Invoke(1);

        if (Input.GetMouseButtonUp(0)) OnMouseButtonRelease?.Invoke(0);
    }
}
public static class EventDelegateContainer
{
    public delegate void MouseInputSenderDelegate(int mouseButton);
    public delegate void FloatSenderDelegate(float value);
}
