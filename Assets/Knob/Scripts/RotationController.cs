using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    [SerializeField] Transform knobTransform;
    [SerializeField] CanvasController canvasController;

    private float _value;
    public float Value 
    { 
        get => _value; 
        set
        {
            _value = value;
            SetRotation();
        }
    }
    private float rotationUnitAmmount, previousAngle;

    [SerializeField] float stepPerTurn, unitPerTurn;
    [SerializeField] int maxTurnAmmount;
    
    private void SetRotation()
    {
        float signedAngle = _value;
        float targetRotation = RoundToAngleUnit(signedAngle);

        float nextUnitAmmount = rotationUnitAmmount + CalculateUnitAmmountFromAngle(targetRotation);

        if (CanRotate(nextUnitAmmount))
        {
            knobTransform.rotation = Quaternion.Euler(new Vector3(0f, 0f, targetRotation));
            previousAngle = targetRotation;
            canvasController.SetAngleText(nextUnitAmmount.ToString("0"));
            rotationUnitAmmount = nextUnitAmmount;
        }
    }
    private float CalculateUnitAmmountFromAngle(float value)
    {    
        Vector2 currentAngleVector = Quaternion.Euler(0f, 0f, value) * Vector3.up;
        Vector2 previousAngleVector = Quaternion.Euler(0f, 0f, previousAngle) * Vector3.up;

        float differenceInSimpleAngle = Vector2.SignedAngle(currentAngleVector, previousAngleVector);
        float differenceInUnit = ConvertToUnit(differenceInSimpleAngle);

        return differenceInUnit;
    }
    private static float ConvertToSimpleAngle(float signedAngle)
    {
        return (signedAngle > 0) ? 360 - signedAngle : -signedAngle;
    }
    private float ConvertToUnit(float value) => unitPerTurn / (360f / value);
    private float RoundToAngleUnit(float value)
    {
        float angleUnit = 360 / stepPerTurn;
        value = Mathf.Round(value / angleUnit) * angleUnit;
        return value;
    }   
    private bool CanRotate(float value)
    {
        if (value < 0f) return false;
        if (value > maxTurnAmmount * unitPerTurn) return false;

        return true;
    }   
   
}