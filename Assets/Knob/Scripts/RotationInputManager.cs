using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotationInputManager : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField] Transform knobTransform;
    [SerializeField] RotationController rotationController;

    [SerializeField] int oneTimeRotationLimit;
    private bool isDrag;

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
        float difference = CalculateSignedDifference(lookVector);

        if (CanRotate(difference))
        {
            rotationController.Value = lookVector;
        } 
    }
    private void SetIsDrag()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue);
        if (raycastHit.transform == knobTransform) isDrag = true;

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
}
