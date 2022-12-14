using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolInputManager : MonoBehaviour
{
    [SerializeField] PoolGameLoopController poolGameLoopController;
    
    public event Action OnMouseClick;
    public event Action OnMouseRelease;
    
    public bool isMouseButtonDown { private set; get; }
    public static Vector2 mousePosition { private set; get; }

    private bool shouldReceiveInput = true;

    private void Start()
    {
        SuscribeToEvents();
    }
    private void SuscribeToEvents()
    {
        OnMouseClick += PoolInputManager_OnMouseClick;
        OnMouseRelease += PoolInputManager_OnMouseRelease;

        poolGameLoopController.OnGameEndStart += PoolGameLoopController_OnGameEndStart; ;
        poolGameLoopController.OnGameRestart += PoolGameLoopController_OnGameRestart;
    }
    private void Update()
    {
        InvokeMouseClickEvents();
        LogMousePosition();
    }
    private void InvokeMouseClickEvents()
    {
        if(shouldReceiveInput)
        {
            if (Input.GetMouseButtonDown(0)) OnMouseClick?.Invoke();
            if (Input.GetMouseButtonUp(0)) OnMouseRelease?.Invoke();
        } 
    }
    private void LogMousePosition()
    {
        if (isMouseButtonDown) mousePosition = Input.mousePosition;
    }

    private void PoolInputManager_OnMouseClick()
    {
        isMouseButtonDown = true;
    }
    private void PoolInputManager_OnMouseRelease()
    {
        isMouseButtonDown = false;
    }
    private void PoolGameLoopController_OnGameEndStart(string arg1, int arg2)
    {
        shouldReceiveInput = false;
    }
    private void PoolGameLoopController_OnGameRestart()
    {
        shouldReceiveInput = true;
    }
}
