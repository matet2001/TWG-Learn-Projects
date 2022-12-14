using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PoolCanvasManager : MonoBehaviour
{
    [Header("Instance References")]
    [SerializeField] PoolInputManager poolInputManager;
    [SerializeField] PoolShootController poolShooter;
    [SerializeField] PoolGameLoopController poolGameLoopController;
    [Header("UI References")]
    [SerializeField] Slider poolChargeSlider;
    [SerializeField] TextMeshProUGUI gameOverText, gameOverTimerText;
    private Camera mainCamera;
    [Header("Variables")]
    [SerializeField] float chargeSliderOffset;

    private bool isMouseButtonDown;

    private void Start()
    {
        SuscribeToEvents();
        
        SetChargeSliderVisible(false);
        SetGameOverTextsVisible(false);
        
        mainCamera = Camera.main;
    }
    private void SuscribeToEvents()
    {
        poolInputManager.OnMouseClick += PoolInputManager_OnMouseClick;
        poolInputManager.OnMouseRelease += PoolInputManager_OnMouseRelease;

        poolGameLoopController.OnGameEndStart += PoolGameLoopController_OnGameEndStart;
        poolGameLoopController.OnGameEnd += PoolGameLoopController_OnGameEnd;
    }
    private void Update()
    {
        ManageChargeSlider();
    }
    private void ManageChargeSlider()
    {
        if (isMouseButtonDown)
        {
            SetPoolChargeSliderValue(poolShooter.chargeValue);
            SetSliderPosition();
        }
    }

    private void SetSliderPosition()
    {  
        MoveSliderToPosition(CalculateTargetBallPosition());
    } 
    private void SetChargeSliderVisible(bool active) => poolChargeSlider.gameObject.SetActive(active);
    private void MoveSliderToPosition(Vector2 position) => poolChargeSlider.transform.position = new Vector2(position.x, position.y + chargeSliderOffset);
    private void SetPoolChargeSliderValue(float value) => poolChargeSlider.value = value;
    private Vector2 CalculateTargetBallPosition()
    {
        PoolBallCollider targetBall = PoolCollisionManager.Instance.targetBall;
        Vector2 targetBallPosition = mainCamera.WorldToScreenPoint(targetBall.transform.position);
        return targetBallPosition;
    }
    private void SetGameOverTextsVisible(bool active)
    {
        gameOverText.gameObject.SetActive(active);
        gameOverTimerText.gameObject.SetActive(active);
    }
    private void SetGameOverText(string text) => gameOverText.text = text;
    private void SetGameOverTimerText(int number) => gameOverTimerText.text = "Restart in " + number + " seconds";

    private void PoolInputManager_OnMouseClick()
    {
        MoveSliderToPosition(CalculateTargetBallPosition());
        SetChargeSliderVisible(true);
        isMouseButtonDown = true;
    }
    private void PoolInputManager_OnMouseRelease()
    {
        SetChargeSliderVisible(false);
        isMouseButtonDown = false;
    }
    private void PoolGameLoopController_OnGameEndStart(string text, int number)
    {
        SetGameOverText(text);
        SetGameOverTimerText(number);
        SetGameOverTextsVisible(true);
        SetChargeSliderVisible(false);
    }
    private void PoolGameLoopController_OnGameEnd()
    {
        SetGameOverTextsVisible(false);       
    }
}
