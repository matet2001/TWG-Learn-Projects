using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanonCanvasController : MonoBehaviour
{
    [SerializeField] CanonValueCalculator canonValueCalculator;
    [SerializeField] Slider canonChargeSlider;
    [SerializeField] TextMeshProUGUI rotationValueText, windValueText; 

    private void Start()
    {
        SuscribeToEvents();
    }
    private void SuscribeToEvents()
    {
        canonValueCalculator.OnChargeValueChange += CanonValueCalculator_OnChargeValueChange;
        canonValueCalculator.OnRotationValueChange += CanonValueCalculator_OnRotationValueChange;
        CanonProjectileManager.OnWindValueChange += CanonProjectileController_OnWindValueChange;
    }
    private void SetCanonChargeSliderValue(float value) => canonChargeSlider.value = value;
    private void SetRotationValueText(float value) => rotationValueText.text = value.ToString("0") + "°";
    private void SetWindValueText(float value) => windValueText.text = "Wind Value: " + value.ToString("0.0");
    private void CanonValueCalculator_OnChargeValueChange(float value)
    {
        SetCanonChargeSliderValue(value);
    }
    private void CanonValueCalculator_OnRotationValueChange(float value)
    {
        SetRotationValueText(value * 90f);
    }
    private void CanonProjectileController_OnWindValueChange(float value)
    {
        SetWindValueText(value);
    }
}
