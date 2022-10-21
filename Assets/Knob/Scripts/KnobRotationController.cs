using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnobRotationController : MonoBehaviour
{
    public float _value;
    public float Value 
    { 
        get => _value; 
        set
        {           
            SetRotation(value);
            _value = CalculateRotationValue();
        }
    }
    private float rotationUnitAmmount, previousAngle;

    [SerializeField] float stepPerTurn, unitPerTurn;
    [SerializeField] int maxTurnAmmount;

    public delegate void FloatSenderDelegate(float rotationToSet, float unitAmount);
    public event FloatSenderDelegate OnRotationValueChange;

    private void Start()
    {
        OnRotationValueChange += RotationController_OnRotationValueChange;
    }
    private float CalculateRotationValue()
    {
        float returnValue = (rotationUnitAmmount == 0f) ? 0f : rotationUnitAmmount / 100f;
        return returnValue;
    }
    private void SetRotation(float signedAngle)
    {
        float targetRotation = RoundToAngleUnit(signedAngle);
        float nextUnitAmmount = rotationUnitAmmount + CalculateUnitDifference(targetRotation);

        TryRotate(targetRotation, nextUnitAmmount);
    }
    private float RoundToAngleUnit(float value)
    {
        float angleUnit = 360 / stepPerTurn;
        value = Mathf.Round(value / angleUnit) * angleUnit;
        return value;
    }
    private float CalculateUnitDifference(float value)
    {    
        Vector2 currentAngleVector = Quaternion.Euler(0f, 0f, value) * Vector3.up;
        Vector2 previousAngleVector = Quaternion.Euler(0f, 0f, previousAngle) * Vector3.up;

        float differenceInSimpleAngle = Vector2.SignedAngle(currentAngleVector, previousAngleVector);
        float differenceInUnit = ConvertToUnit(differenceInSimpleAngle);

        return differenceInUnit;
    }
    //private static float ConvertToSimpleAngle(float signedAngle)
    //{
    //    return (signedAngle > 0) ? 360 - signedAngle : -signedAngle;
    //}
    private float ConvertToUnit(float value) => unitPerTurn / (360f / value); 
    private void TryRotate(float targetRotation, float nextUnitAmmount)
    {
        float maxUnitAmount = maxTurnAmmount * unitPerTurn;

        if (!CanRotate(nextUnitAmmount, maxUnitAmount)) return;
        nextUnitAmmount = Mathf.Clamp(nextUnitAmmount, 0, maxUnitAmount);

        OnRotationValueChange?.Invoke(targetRotation, nextUnitAmmount);
    }
    private bool CanRotate(float value, float maxValue)
    {
        if (value < 0f) return false;
        if (value > maxValue) return false;

        return true;
    }
    private void RotationController_OnRotationValueChange(float rotationToSet, float unitAmount)
    {
        previousAngle = rotationToSet;
        rotationUnitAmmount = unitAmount;
    }
}