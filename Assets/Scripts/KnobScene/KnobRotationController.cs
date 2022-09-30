using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnobRotationController : MonoBehaviour
{
    [SerializeField] Transform knobTransform;
    [SerializeField] CanvasController canvasController;

    private Camera mainCamera;

    [SerializeField] int oneTimeRotationLimit; 
    [SerializeField] float unitPerRotation, unitPerTwist;
    [SerializeField] int maxTwistAmmount;
    private float rotationUnitAmmount;
    private bool isDrag;


    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void FixedUpdate()
    {
        ManageDrag();
    }
    private void ManageDrag()
    {
        SetDrag();

        if(isDrag) //Debughoz kell, amúgy nem kell ide
        {
            CalculateSignedAngle(out float signedAngle, out float difference);
            float nextRotationUnitAmmount = CalculateNextRotationUnitAmmount(difference);

            if (CanRotate(difference, nextRotationUnitAmmount))
            {
                Debug.Log(difference + " - " + nextRotationUnitAmmount);
                rotationUnitAmmount = nextRotationUnitAmmount;
                knobTransform.rotation = Quaternion.Euler(new Vector3(0f, 0f, signedAngle));
                canvasController.SetAngleText(rotationUnitAmmount.ToString("0"));
            }
        }
    }
    private void SetDrag()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue);
        if (raycastHit.transform == knobTransform) isDrag = true;

        if (!Input.GetMouseButton(0)) isDrag = false;
    }
    private void CalculateSignedAngle(out float signedAngle, out float difference)
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookVector = mousePosition - (Vector2)transform.position;

        signedAngle = ConvertToRotationUnit(Vector2.SignedAngle(Vector2.up, lookVector));
        difference = ConvertToRotationUnit(Vector2.SignedAngle(knobTransform.up, lookVector));
    }
    private float CalculateNextRotationUnitAmmount(float difference)
    {
        float unitMultiplier = unitPerTwist / (360 / unitPerRotation);
        float angleDifference = ConvertToAngle(difference);
        float unitDifference = ConvertToRotationUnit(angleDifference);
        float nextRotationUnitAmmount = (unitDifference * unitMultiplier) / unitPerRotation + rotationUnitAmmount;
        return nextRotationUnitAmmount;
    }
    private static float ConvertToAngle(float signedAngle)
    {
        return (signedAngle > 0) ? -signedAngle : -signedAngle;
    }
    private float ConvertToRotationUnit(float value)
    {
        value = Mathf.Round(value / unitPerRotation) * unitPerRotation;
        return value;
    }
    private bool CanRotate(float difference, float nextRotationUnitAmmount)
    {
        if (!isDrag) return false;

        if (Mathf.Abs(difference) > oneTimeRotationLimit) return false;
        if (nextRotationUnitAmmount < 0f) return false;
        if (nextRotationUnitAmmount > unitPerTwist * maxTwistAmmount) return false;

        return true;
    }
}