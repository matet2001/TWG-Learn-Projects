using System;
using UnityEngine;

public class GunMouseInputManager : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] GunRotationController gunRotationController;

    public event Action<float> OnDragStart;
    public event EventHandler OnShoot;
    public event EventHandler OnDragEnd;

    private float previousAngle;
    [SerializeField] int dragMouseButton = 1, shootMouseButton = 0;

    private void Start()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        InvokeClickEvents();
        SendMouseVectorData();
    }
    private void InvokeClickEvents()
    {
        if (Input.GetMouseButtonDown(dragMouseButton))
            OnDragStart?.Invoke(CalculateSignedAngle());

        if (Input.GetMouseButtonDown(shootMouseButton))
            OnShoot?.Invoke(this, EventArgs.Empty);

        if (Input.GetMouseButtonUp(dragMouseButton))
            OnDragEnd?.Invoke(this, EventArgs.Empty);
    }
    private void SendMouseVectorData()
    {
        float signedAngle = CalculateSignedAngle();

        if (previousAngle != signedAngle)
        {
            gunRotationController.SetRotation(signedAngle);
            previousAngle = signedAngle;
        } 
    }
    private float CalculateSignedAngle()
    {
        Vector2 lookVector = GetLookVector();
        return Vector2.SignedAngle(Vector2.up, lookVector);
    }
    private Vector2 GetLookVector()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookVector = mousePosition - (Vector2)transform.position;
        return lookVector;
    }
}
