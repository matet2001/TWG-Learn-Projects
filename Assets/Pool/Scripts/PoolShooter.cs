using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolShooter : MonoBehaviour
{
    [SerializeField] PoolInputManager poolInputManager;
    private Camera mainCamera;
 
    public float chargeValue { private set; get; }
    
    [SerializeField] float ballShootSpeed = 1f;

    private bool isMouseButtonDown;
    private float timeForFullCharge = 2f;

    private void Start()
    {
        mainCamera = Camera.main;
        SuscribeToEvents();
    }
    private void SuscribeToEvents()
    {
        poolInputManager.OnMouseClick += PoolInputManager_OnMouseClick;
        poolInputManager.OnMouseRelease += PoolInputManager_OnMouseRelease;
    }
    private void Update()
    {
        CalculateChargeValue();
    }
    private void CalculateChargeValue()
    {
        if (isMouseButtonDown)
        {
            float chargeValuePerFrame = 1 / timeForFullCharge * Time.deltaTime;
            chargeValue += chargeValuePerFrame;

            chargeValue = Mathf.Clamp01(chargeValue);
        }
        else chargeValue = 0f;
    }
    private void Shoot()
    {
        PoolBallController targetBall = PoolCollisionManager.Instance.targetBall;

        Vector2 ballPosition = targetBall.transform.position;
        Vector2 startPosition = mainCamera.ScreenToWorldPoint(PoolInputManager.mousePosition);
        Vector2 mouseBallVector = ballPosition - startPosition;
        float baseSpeed = 1f;
        float speed = baseSpeed + chargeValue * ballShootSpeed;

        targetBall.SetDirection(mouseBallVector.normalized * speed);
    }
    private void PoolInputManager_OnMouseClick(object sender, System.EventArgs e)
    {
        isMouseButtonDown = true;
    }
    private void PoolInputManager_OnMouseRelease(object sender, System.EventArgs e)
    {
        isMouseButtonDown = false;
        Shoot();
    }
}
