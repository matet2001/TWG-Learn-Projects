using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonValueCalculator : MonoBehaviour
{
    [SerializeField] CanonInputManager canonInputManager;
    [SerializeField] CanonCanvasController canonCanvasController;

    public event Action<float> OnChargeValueChange;
    public event Action<float> OnRotationValueChange;

    private bool isButtonPressed;
    private float chargeValue, previousChargeValue, timeForFullCharge = 2f;
    private int chargeIncreaseButton = 0;

    private float rotationValue;

    private void Start()
    {
        SubscribeToEvents();
    }
    private void SubscribeToEvents()
    {
        canonInputManager.OnMouseButtonPress += CanonInputManager_OnMouseButtonPress;
        canonInputManager.OnMouseButtonRelease += CanonInputManager_OnMouseButtonRelease;

        canonInputManager.OnMousePositionChange += CanonInputManager_OnMousePositionChange;
    }   
    private void Update()
    {
        CalculateChargeValue();
    }
    #region ChargeValue
    private void CalculateChargeValue()
    {
        float chargeValuePerFrame = 1 / timeForFullCharge;

        if (isButtonPressed)
            chargeValue += chargeValuePerFrame * Time.deltaTime;
        if (!isButtonPressed)
            chargeValue -= chargeValuePerFrame * Time.deltaTime;

        chargeValue = Mathf.Clamp01(chargeValue);

        if(chargeValue != previousChargeValue) 
            ChargeValueChange(chargeValue);
    }
    public float GetChargeValue() => chargeValue;
    private void CanonInputManager_OnMouseButtonPress(int mouseButton)
    {
        if (mouseButton == chargeIncreaseButton) isButtonPressed = true;
    }
    private void CanonInputManager_OnMouseButtonRelease(int mouseButton)
    {
        if (mouseButton == chargeIncreaseButton) isButtonPressed = false;
    }
    private void ChargeValueChange(float value)
    {
        previousChargeValue = chargeValue;
        OnChargeValueChange?.Invoke(value);
    }
    #endregion
    private void CanonInputManager_OnMousePositionChange(float value)
    {
        rotationValue += value;
        rotationValue = Mathf.Clamp01(rotationValue);

        OnRotationValueChange?.Invoke(rotationValue);
    }
}
