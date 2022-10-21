using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [SerializeField] Text angleText;
    [SerializeField] KnobRotationController rotationController;

    private void Start()
    {
        rotationController.OnRotationValueChange += RotationController_OnRotationValueChange;
    }

    private void RotationController_OnRotationValueChange(float rotationToSet, float unitAmount)
    {
        SetAngleText(unitAmount.ToString("0"));
    }

    public void SetAngleText(string textToSet) => angleText.text = textToSet;
}
