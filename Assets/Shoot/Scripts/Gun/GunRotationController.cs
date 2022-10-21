using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotationController : MonoBehaviour
{  
    private Vector2 startMouseVector, startRotationVector;
 
    private void Start()
    {
        SuscribeToEvents();
    }
    private void SuscribeToEvents()
    {
        GunRotationMouseInput rotationInputManager = GetComponent<GunRotationMouseInput>();
        rotationInputManager.OnAngleChange += RotationInputManager_OnAngleChange;
        rotationInputManager.OnDragStart += RotationInputManager_OnDragStart;
    }
    private void SetRotation(float value)
    {
        Vector2 currentVector = Quaternion.Euler(0f, 0f, value) * Vector2.up;
        float signedDifference = Vector2.SignedAngle(startMouseVector, currentVector);
        float currentRotation = Vector2.SignedAngle(Vector2.up, startRotationVector);

        transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, signedDifference + currentRotation));
    }  
    private void RotationInputManager_OnDragStart(float value)
    {
        startMouseVector = Quaternion.Euler(0f, 0f, value) * Vector2.up;
        startRotationVector = Quaternion.Euler(0f, 0f, transform.localEulerAngles.z) * Vector2.up;
    }
    private void RotationInputManager_OnAngleChange(float value)
    {
        SetRotation(value);
    }
}
