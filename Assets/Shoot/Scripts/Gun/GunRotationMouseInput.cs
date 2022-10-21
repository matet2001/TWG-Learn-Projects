using UnityEngine;

public class GunRotationMouseInput : MonoBehaviour
{
    private Camera mainCamera;

    private float previousAngle;
    private bool isDrag;

    public delegate void FloatSenderDelegate(float value);
    public event FloatSenderDelegate OnDragStart;
    public event FloatSenderDelegate OnAngleChange;

    private void Start()
    {
        mainCamera = Camera.main;
        OnDragStart += GunRotationMouseInput_OnDragStart;
    }
    private void Update()
    {
        SetIsDrag();
        ManageInput();
    }
    private void SetIsDrag()
    {
        if (!Input.GetMouseButton(0)) isDrag = false;

        if (Input.GetMouseButton(0) && !isDrag)
            OnDragStart?.Invoke(CalculateSignedAngle());
    }
    private void ManageInput()
    {
        if (!isDrag) return;

        float signedAngle = CalculateSignedAngle();

        if (previousAngle != signedAngle)
        {
            OnAngleChange?.Invoke(signedAngle);
            previousAngle = signedAngle;
        } 
    }
    private Vector2 GetLookVector()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookVector = mousePosition - (Vector2)transform.position;
        return lookVector;
    }
    private float CalculateSignedAngle()
    {
        Vector2 lookVector = GetLookVector();
        return Vector2.SignedAngle(Vector2.up, lookVector);
    }
    private void GunRotationMouseInput_OnDragStart(float value)
    {
        isDrag = true;
    }
}
