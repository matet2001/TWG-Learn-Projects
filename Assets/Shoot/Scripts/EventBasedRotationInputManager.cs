using UnityEngine;

public class EventBasedRotationInputManager : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField] Transform targetTransform;

    [SerializeField] int oneTimeRotationLimit;

    private float previousAngle;
    private bool isDrag;

    public delegate void FloatSenderDelegate(float value);
    public event FloatSenderDelegate OnDragStart;
    public event FloatSenderDelegate OnAngleChange;

    private void Start()
    {
        mainCamera = Camera.main;
        OnDragStart += AdvancedRotationInputManager_OnDragStart;
    }
    private void Update()
    {
        SetIsDrag();
        ManageInput();
    }
    private void ManageInput()
    {
        if (!isDrag) return;

        float signedAngle = CalculateSignedAngle();

        if (CanRotate(CalculateSignedDifference()) && previousAngle != signedAngle)
        {
            OnAngleChange?.Invoke(signedAngle);
            previousAngle = signedAngle;
        } 
    }
    private void SetIsDrag()
    {
        if (!Input.GetMouseButton(0)) isDrag = false;

        if (Input.GetMouseButton(0) && !isDrag)
            OnDragStart?.Invoke(CalculateSignedAngle());
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
    private float CalculateSignedDifference()
    {
        Vector2 lookVector = GetLookVector();
        return Vector2.SignedAngle(targetTransform.up, lookVector);
    }
    private float CalculateSignedAngle()
    {
        Vector2 lookVector = GetLookVector();
        return Vector2.SignedAngle(Vector2.up, lookVector);
    }
    private void AdvancedRotationInputManager_OnDragStart(float value)
    {
        isDrag = true;
    }
}
