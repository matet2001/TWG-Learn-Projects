using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PoolCanvasManager : MonoBehaviour
{
    [SerializeField] PoolInputManager poolInputManager;
    [SerializeField] PoolShooter poolShooter;
    [SerializeField] PoolGameLoopController poolGameLoopController;

    [SerializeField] Slider poolChargeSlider;
    [SerializeField] TextMeshProUGUI gameOverText, gameOverTimerText;
    private Camera mainCamera;

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
        SetPoolChargeSliderValue();
        SetSliderPosition();
    }
    private void SetSliderPosition()
    {
        if (isMouseButtonDown)
        {
            MoveSliderToPosition(CalculateTargetBallPosition());
        }
    }
    private void SetPoolChargeSliderValue()
    {
        if (isMouseButtonDown)
        {
            poolChargeSlider.value = poolShooter.chargeValue;
        }
    }
    private void SetChargeSliderVisible(bool active) => poolChargeSlider.gameObject.SetActive(active);
    private void MoveSliderToPosition(Vector2 position) => poolChargeSlider.transform.position = new Vector2(position.x, position.y + chargeSliderOffset);
    private Vector2 CalculateTargetBallPosition()
    {
        PoolBallController targetBall = PoolCollisionManager.Instance.targetBall;
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

    private void PoolInputManager_OnMouseClick(object sender, System.EventArgs e)
    {
        MoveSliderToPosition(CalculateTargetBallPosition());
        SetChargeSliderVisible(true);
        isMouseButtonDown = true;
    }
    private void PoolInputManager_OnMouseRelease(object sender, System.EventArgs e)
    {
        SetChargeSliderVisible(false);
        isMouseButtonDown = false;
    }
    private void PoolGameLoopController_OnGameEndStart(string text, int number)
    {
        SetGameOverText(text);
        SetGameOverTimerText(number);
        SetGameOverTextsVisible(true);
    }
    private void PoolGameLoopController_OnGameEnd(object sender, System.EventArgs e)
    {
        SetGameOverTextsVisible(false);
    }
}
