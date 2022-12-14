using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolShootController : MonoBehaviour
{
    [SerializeField] PoolInputManager poolInputManager;
    [SerializeField] PoolGameLoopController poolGameLoopController;

    private Camera mainCamera;
 
    public float chargeValue { private set; get; }
    
    [SerializeField] float ballShootSpeed = 1f;

    private bool isMouseButtonDown;
    private float timeForFullCharge = 2f;

    private GameObject chargeSound;

    private void Start()
    {
        mainCamera = Camera.main;
        SuscribeToEvents();
    }
    private void SuscribeToEvents()
    {
        poolInputManager.OnMouseClick += PoolInputManager_OnMouseClick;
        poolInputManager.OnMouseRelease += PoolInputManager_OnMouseRelease;

        poolGameLoopController.OnGameEndStart += PoolGameLoopController_OnGameEndStart;
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
        PoolBallCollider targetBall = PoolCollisionManager.Instance.targetBall;

        Vector2 ballPosition = targetBall.transform.position;
        Vector2 startPosition = mainCamera.ScreenToWorldPoint(PoolInputManager.mousePosition);
        Vector2 mouseBallVectorNormalized = (ballPosition - startPosition).normalized;

        float baseSpeed = 1f;
        float speed = baseSpeed + chargeValue * ballShootSpeed;
        
        targetBall.directionVector =  mouseBallVectorNormalized * speed;
    }
    private void PoolInputManager_OnMouseClick()
    {
        isMouseButtonDown = true;
        //PoolCollisionManager.Instance.targetBall.flashController.StartCharge(Color.yellow, timeForFullCharge);
        PoolCollisionManager.Instance.targetBall.flashController.StartFlashing(Color.yellow, 1f);
        chargeSound = SoundManager.PlaySound("_ChargeSound", Vector2.zero, true);
    }
    private void PoolInputManager_OnMouseRelease()
    {
        isMouseButtonDown = false;
        Shoot();
        PoolCollisionManager.Instance.targetBall.flashController.StopFlashState();
        SoundManager.StopSound(chargeSound);
    }
    private void PoolGameLoopController_OnGameEndStart(string arg1, int arg2)
    {
        isMouseButtonDown = false;
        PoolCollisionManager.Instance.targetBall.flashController.StopFlashState();
    }
}
