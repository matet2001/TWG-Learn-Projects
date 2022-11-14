using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolCueController : MonoBehaviour
{
    [SerializeField] PoolInputManager poolInputManager;
    private LineRenderer lineRenderer;
    private Camera mainCamera;

    [SerializeField] float distanceFromBall = 1f;
    private bool isMouseButtonDown;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

        mainCamera = Camera.main;
    }
    private void Start()
    {
        poolInputManager.OnMouseClick += PoolInputManager_OnMouseClick;
        poolInputManager.OnMouseRelease += PoolInputManager_OnMouseRelease;
    }
    private void Update()
    {
        DisplayCue();
    }
    private void DisplayCue()
    {
        if (isMouseButtonDown)
        {
            PoolBallController targetBall = PoolCollisionManager.Instance.targetBall;
            Vector2 ballPosition = targetBall.transform.position;
            float ballRadius = targetBall.radius;

            Vector2 startPosition = mainCamera.ScreenToWorldPoint(PoolInputManager.mousePosition);
            Vector2 mouseBallVector = ballPosition - startPosition;
            Vector2 distanceVector = mouseBallVector.normalized  * (ballRadius * distanceFromBall);
            Vector2 endPosition = startPosition + mouseBallVector - distanceVector;

            lineRenderer.SetPosition(0, startPosition + distanceVector);
            lineRenderer.SetPosition(1, endPosition);     
        }
    }

    private void PoolInputManager_OnMouseClick(object sender, System.EventArgs e)
    {
        isMouseButtonDown = true;
        lineRenderer.enabled = true;
    }
    private void PoolInputManager_OnMouseRelease(object sender, System.EventArgs e)
    {
        isMouseButtonDown = false;
        lineRenderer.enabled = false;
    }
}
