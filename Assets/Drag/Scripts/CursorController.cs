using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    private Camera mainCamera;

    private Transform draggedItem;
    private Vector2 dragOffset;

    public delegate void OnReleaseEventDelegate(Transform itemTransform);
    public event OnReleaseEventDelegate OnRelease;

    private void Start()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        HandleDrag();
    }
    private void HandleDrag()
    {
        StartDragging();
        Dragging();
        EndDragging();
    }
    private void StartDragging()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (draggedItem != null) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue);

        if (raycastHit.transform == null) return;

        if (raycastHit.transform.CompareTag("Item"))
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            dragOffset = (Vector2)raycastHit.transform.position - mousePos;
            draggedItem = raycastHit.transform;
        } 
    }
    private void Dragging()
    {
        if (draggedItem == null) return;

        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        draggedItem.transform.position = mousePos + dragOffset;
    }
    private void EndDragging()
    {
        if (draggedItem != null && Input.GetMouseButtonUp(0))
        {
            OnRelease?.Invoke(draggedItem);
            draggedItem = null;
        } 
    }
}
