using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotationMouseInput : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField] Transform knobTransform;
    [SerializeField] KnobRotationController rotationController;

    [SerializeField] int oneTimeRotationLimit;
    private bool isDrag;
    [SerializeField] bool shouldRelativeToMouse;
    private Vector2 dragStartMouseVector, dragStartRotationVector;

    private void Start()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        SetIsDrag();
        ManageInput();
    }
    private void ManageInput()
    {
        if (!isDrag) return;

        Vector2 lookVector = GetLookVector();

        if (!shouldRelativeToMouse) TryRotate(lookVector);
        else TryRotateRelative(lookVector);
    }

    private void TryRotate(Vector2 lookVector)
    {
        float difference = CalculateSignedDifference(lookVector);

        if (CanRotate(difference))
        {
            float signedAngle = CalculateSignedAngle(lookVector);
            rotationController.Value = signedAngle;
        }
    }
    private void SetIsDrag()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue);
        if (raycastHit.transform == knobTransform && !isDrag)
        {
            isDrag = true;
            dragStartMouseVector = GetLookVector();
            dragStartRotationVector = Quaternion.Euler(0f, 0f, knobTransform.localEulerAngles.z) * Vector2.up;
        }
        if (!Input.GetMouseButton(0)) isDrag = false;
    }
    private bool CanRotate(float difference)
    {
        if (!isDrag) return false;
        if (Mathf.Abs(difference) > oneTimeRotationLimit) return false;

        return true;
    }
    private Vector2 GetLookVector()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookVector = mousePosition - (Vector2)transform.position;
        return lookVector;
    }
    private float CalculateSignedDifference(Vector2 lookVector)
    {
        return Vector2.SignedAngle(knobTransform.up, lookVector);
    }
    private float CalculateSignedAngle(Vector2 lookVector)
    {
        return Vector2.SignedAngle(Vector2.up, lookVector);
    }
    private void TryRotateRelative(Vector2 lookVector)
    {
        float difference = Vector2.SignedAngle(dragStartMouseVector, lookVector);
        float currentRotation = CalculateSignedAngle(dragStartRotationVector);

        rotationController.Value = difference + currentRotation;
    }
}
