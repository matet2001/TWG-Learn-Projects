using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanonCanvasController : MonoBehaviour
{
   [SerializeField] CanonValueCalculator canonValueCalculator;
   [SerializeField] Slider canonChargeSlider;

    private void Start()
    {
        canonValueCalculator.OnChargeValueChange += CanonValueCalculator_OnChargeValueChange;
    }
   
    private void SetCanonChargeSliderValue(float value) => canonChargeSlider.value = value;

    private void CanonValueCalculator_OnChargeValueChange(float value)
    {
        SetCanonChargeSliderValue(value);
    }
}
