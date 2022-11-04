using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonInputManager : MonoBehaviour
{
    public Action<int> OnMouseButtonPress;
    public Action<int> OnMouseButtonRelease;
    public Action<float> OnMousePositionChange;

    [SerializeField] float sensitivity = 0.01f;
    private Vector2 lastMousePosition;

    private void Update()
    {
        InvokeMousePressEvents();
        CheckHorizontalMousePosition();
    }
    private void InvokeMousePressEvents()
    {
        if (Input.GetMouseButtonDown(0)) OnMouseButtonPress?.Invoke(0);
        if (Input.GetMouseButtonUp(0)) OnMouseButtonRelease?.Invoke(0);
    }
    private void CheckHorizontalMousePosition()
    {
        Vector2 mousePos = new Vector2(Input.mousePosition.x, 0f);

        if (lastMousePosition != mousePos)
        {
            Vector2 mouseMoveVector = (mousePos - lastMousePosition).normalized;
            float value = mouseMoveVector.magnitude * sensitivity * Mathf.Sign(mouseMoveVector.x);

            lastMousePosition = new Vector2(mousePos.x, 0f);
            OnMousePositionChange?.Invoke(value);
        }
    }
}
