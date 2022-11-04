using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotationController : MonoBehaviour
{
    [SerializeField] GunMouseInputManager gunMouseInputManager;

    private Vector2 startMouseVector, startRotationVector;
    private bool isDrag;
 
    private void Start()
    {
        SuscribeToEvents();
    }
    private void SuscribeToEvents()
    {
        gunMouseInputManager.OnDragStart += GunMouseInputManager_OnDragStart;
        gunMouseInputManager.OnDragEnd += GunMouseInputManager_OnDragEnd;
    }
    public void SetRotation(float value)
    {
        if (!isDrag) return;

        Vector2 currentVector = Quaternion.Euler(0f, 0f, value) * Vector2.up;
        float signedDifference = Vector2.SignedAngle(startMouseVector, currentVector);
        float currentRotation = Vector2.SignedAngle(Vector2.up, startRotationVector);

        transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, signedDifference + currentRotation));
    }
    private void GunMouseInputManager_OnDragStart(float value)
    {
        isDrag = true;
        startMouseVector = Quaternion.Euler(0f, 0f, value) * Vector2.up;
        startRotationVector = Quaternion.Euler(0f, 0f, transform.localEulerAngles.z) * Vector2.up;
    }
    private void GunMouseInputManager_OnDragEnd(object sender, EventArgs e)
    {
        isDrag = false;
    }
}
