using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolInputManager : MonoBehaviour
{
    [SerializeField] PoolGameLoopController poolGameLoopController;
    
    public event EventHandler OnMouseClick;
    public event EventHandler OnMouseRelease;
    
    public bool isMouseButtonDown { private set; get; }
    public static Vector2 mousePosition { private set; get; }

    private bool shouldReceiveInput = true;

    private void Start()
    {
        OnMouseClick += PoolInputManager_OnMouseClick;
        OnMouseRelease += PoolInputManager_OnMouseRelease;

        poolGameLoopController.OnGameEndStart += PoolGameLoopController_OnGameEndStart; ;
        poolGameLoopController.OnGameRestart += PoolGameLoopController_OnGameRestart;
    }
    private void Update()
    {
        InvokeEvents();
        LogMousePosition();
    }
    private void InvokeEvents()
    {
        if(shouldReceiveInput)
        {
            if (Input.GetMouseButtonDown(0)) OnMouseClick?.Invoke(this, EventArgs.Empty);
            if (Input.GetMouseButtonUp(0)) OnMouseRelease?.Invoke(this, EventArgs.Empty);
        } 
    }
    private void LogMousePosition()
    {
        if (isMouseButtonDown) mousePosition = Input.mousePosition;
    }

    private void PoolInputManager_OnMouseClick(object sender, EventArgs e)
    {
        isMouseButtonDown = true;
    }
    private void PoolInputManager_OnMouseRelease(object sender, EventArgs e)
    {
        isMouseButtonDown = false;
    }
    private void PoolGameLoopController_OnGameEndStart(string arg1, int arg2)
    {
        shouldReceiveInput = false;
    }
    private void PoolGameLoopController_OnGameRestart(object sender, EventArgs e)
    {
        shouldReceiveInput = true;
    }
}
