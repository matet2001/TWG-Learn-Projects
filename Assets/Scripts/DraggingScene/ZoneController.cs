using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneController : MonoBehaviour
{
    [SerializeField] CursorController cursorController;

    private Bounds colliderBounds;

    private void Awake()
    {
        colliderBounds = GetComponent<Collider2D>().bounds;
    }
    private void Start()
    {
        if(cursorController) cursorController.OnRelease += CursorController_OnRelease;
    }
    private void CursorController_OnRelease(Transform itemTransform)
    {
        if(colliderBounds.Contains(itemTransform.position))
        {
            itemTransform.position = transform.position;
        }
    }
}
