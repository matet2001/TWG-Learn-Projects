using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    [SerializeField] Transform knobTransform;
    [SerializeField] CanvasController canvasController;

    private Vector2 lookVector;
    public Vector2 Value 
    { 
        get => lookVector; 
        set
        {
            lookVector = value;
            SetRotation();
        }
    }
    private float rotationUnitAmmount;

    [SerializeField] float stepPerTurn, unitPerTurn;
    [SerializeField] int maxTurnAmmount;
    [SerializeField] int oneTimeRotationLimit;   
    
    private void SetRotation()
    {
        float signedAngle = CalculateSignedAngle(lookVector);
        float targetRotation = RoundToAngleUnit(signedAngle);

        float difference = CalculateSignedDifference(lookVector);
        float differenceUnit = ConvertToUnit(ConvertToAngle(RoundToAngleUnit(difference)));
        float nextUnitAmmount = rotationUnitAmmount + differenceUnit;

        if(CanRotate(nextUnitAmmount))
        {
            knobTransform.rotation = Quaternion.Euler(new Vector3(0f, 0f, targetRotation));
            canvasController.SetAngleText(nextUnitAmmount.ToString("0"));
            rotationUnitAmmount = nextUnitAmmount;
        }
    }
    private float RoundToAngleUnit(float value)
    {
        float angleUnit = 360 / stepPerTurn;
        value = Mathf.Round(value / angleUnit) * angleUnit;
        return value;
    }
    private static float ConvertToAngle(float signedAngle) => -signedAngle;
    private float ConvertToUnit(float value) => unitPerTurn / (360f / value);
    private bool CanRotate(float value)
    {
        if (value < 0f) return false;
        if (value > maxTurnAmmount * unitPerTurn) return false;

        return true;
    }
    private float CalculateSignedAngle(Vector2 lookVector)
    {
        return Vector2.SignedAngle(Vector2.up, lookVector);
    }
    private float CalculateSignedDifference(Vector2 lookVector)
    {
        return Vector2.SignedAngle(knobTransform.up, lookVector);
    }
}