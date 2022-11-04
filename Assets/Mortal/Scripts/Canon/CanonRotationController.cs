using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonRotationController : MonoBehaviour
{
    [SerializeField] CanonValueCalculator canonValueCalculator;

    private Vector3 spriteLengthVector;

    private void Awake()
    {
        CalculateSpriteLengthVector();
    }
    private void CalculateSpriteLengthVector()
    {
        Bounds bounds = GetComponent<SpriteRenderer>().bounds;
        spriteLengthVector = (bounds.center + bounds.extents) - (bounds.center - bounds.extents);
    }
    private void Start()
    {
        SuscribeToEvents();
    }
    private void SuscribeToEvents()
    {
        canonValueCalculator.OnRotationValueChange += CanonValueCalculator_OnRotationValueChange;
    }
    private void SetTransformRotation(float value)
    {
        float rotationValue = 45 - value * 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationValue);   
    }
    private void CanonValueCalculator_OnRotationValueChange(float value)
    {
        SetTransformRotation(value);
    }
    public Vector2 GetDuctSpriteLengthVectorRotated()
    {  
        Vector3 spriteLengthVectorRotated = transform.rotation * spriteLengthVector;
        return spriteLengthVectorRotated;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.rotation * spriteLengthVector);
    }
}
