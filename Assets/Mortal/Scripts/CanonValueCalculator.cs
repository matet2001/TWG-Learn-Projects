using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventDelegateContainer;

public class CanonValueCalculator : MonoBehaviour
{
    public event FloatSenderDelegate OnChargeValueChange;

    [SerializeField] CanonInputManager canonInputManager;
    [SerializeField] CanonCanvasController canonCanvasController;

    private bool isButtonPressed;
    private float chargeValue, timeForFullCharge = 2f;
    private int chargeIncreaseButton = 1;

    private void Start()
    {
        SubscribeToEvents();
    }
    private void SubscribeToEvents()
    {
        canonInputManager.OnMouseButtonPress += CanonInputManager_OnMouseButtonPress;
        canonInputManager.OnMouseButtonRelease += CanonInputManager_OnMouseButtonRelease;
    }
    private void Update()
    {
        CalculateChargeValue();
    }
    private void CalculateChargeValue()
    {
        float chargeValuePerFrame = 1 / timeForFullCharge;
        bool isValueChanged = false;

        if (isButtonPressed)
        {
            chargeValue += chargeValuePerFrame * Time.deltaTime;
            isValueChanged = true;
            ChargeValueChange(chargeValue);
        }
        if (!isButtonPressed)
        {
            chargeValue -= chargeValuePerFrame * Time.deltaTime;
            isValueChanged = true;
        }

        if (isValueChanged)
        {
            chargeValue = Mathf.Clamp(chargeValue, 0f, 1f);
            ChargeValueChange(chargeValue);
        }
    }
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
        
        OnChargeValueChange(value);
    }
}
