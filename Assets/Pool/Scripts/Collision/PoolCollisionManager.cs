using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolCollisionManager : MonoBehaviour
{
    public static PoolCollisionManager Instance;

    public PoolBallController targetBall;
    [SerializeField] PoolGameLoopController poolGameLoopController;
    [SerializeField] PoolBallController[] poolBallControllerArray;
    [SerializeField] PoolWallController[] poolWallControllerArray;
    [SerializeField] PoolHoleController[] poolHoleControllerArray;

    [SerializeField] float collisionCheckFrequency = 1f;
    [SerializeField] bool showDebugLines;
    
    private float holeEnterSpeedMax = 10f;
    private float collisionCheckFrequencyMax;
    private bool shouldCheckCollision = true;
    

    private void Awake()
    {
        Instance = this;
        collisionCheckFrequencyMax = collisionCheckFrequency;
    }
    private void Start()
    {
        poolGameLoopController.OnGameEndStart += PoolGameLoopController_OnGameEndStart; ;
        poolGameLoopController.OnGameRestart += PoolGameLoopController_OnGameRestart;
    }

    

    private void Update()
    {
        CheckForCollision();
        CountDownCollisionCheckFrequency();
    }
    private void CheckForCollision()
    {
        if (collisionCheckFrequency > 0f) return;
        if (!shouldCheckCollision) return;

        BallToBallCollision();
        
        foreach (PoolBallController ballController in poolBallControllerArray)
        { 
            WallToBallCollision(ballController);
            HoleToBallCollision(ballController);
        }
    }
    private void BallToBallCollision()
    {
        PoolBallController ball = poolBallControllerArray[0];
        PoolBallController otherBall = poolBallControllerArray[1];

        if (CalculateCircleCollision(ball, otherBall))
        {
            Vector2 ballPosition = ball.transform.position;
            Vector2 otherBallPosition = otherBall.transform.position;

            Vector2 ballGivedForce = ball.CalculateForceGiveToOtherBall(otherBallPosition);
            Vector2 otherBallGivedForce = otherBall.CalculateForceGiveToOtherBall(ballPosition);

            ball.BallCollision(otherBallPosition, otherBallGivedForce);
            otherBall.BallCollision(ballPosition, ballGivedForce);

            collisionCheckFrequency = collisionCheckFrequencyMax;
        }
    }
    private void WallToBallCollision(PoolBallController ballController)
    {
        foreach (PoolWallController wallController in poolWallControllerArray)
        {
            float ballRadius = ballController.radius;
            Vector3 ballCenter = ballController.transform.position;           
            Vector3 wallCenter = wallController.transform.position;

            PoolWallController.wallDirectionEnum wallDirectionVertical = PoolWallController.wallDirectionEnum.vertical;
            Vector2 triangleAVertex = wallCenter;
            Vector2 triangleBVertex = ballCenter;           
            Vector2 triangleCVertex = (wallController.wallDirection == wallDirectionVertical) ? new Vector2(triangleBVertex.x, triangleAVertex.y) : new Vector2(triangleAVertex.x, triangleBVertex.y);
            Vector2 triangleASide = triangleBVertex - triangleCVertex;
            
            if(showDebugLines && wallController == poolWallControllerArray[0] && ballController == targetBall)
            {
                Debug.DrawLine(triangleAVertex, triangleBVertex, Color.red);
                Debug.DrawLine(triangleAVertex, triangleCVertex, Color.blue);
                Debug.DrawLine(triangleBVertex, triangleCVertex, Color.green);
            }

            float distance = triangleASide.magnitude;

            if(distance < ballRadius + wallController.GetColliderSideLength(ballCenter))
            {
                Vector2 wallNormalVector = wallController.CalculateNormalDirectionVector(ballCenter);
                ballController.WallCollision(wallNormalVector);

                collisionCheckFrequency = collisionCheckFrequencyMax;
            }
        }
    }
    private void HoleToBallCollision(PoolBallController ballController)
    {
        foreach(PoolHoleController holeController in poolHoleControllerArray)
        {
            if (CalculateCircleCollision(ballController, holeController))
            {
                if (ballController.directionVector.magnitude < holeEnterSpeedMax)
                {
                    bool isWin = ballController != targetBall;
                    poolGameLoopController.StartGameEnd(isWin);
                    
                    ballController.gameObject.SetActive(false);
                    ballController.Stop();
                    
                    collisionCheckFrequency = collisionCheckFrequencyMax;
                    return;
                }         
            }
        }
        
    }
    private static bool CalculateCircleCollision(PoolCollidableCircle circle, PoolCollidableCircle otherCircle)
    {
        float circleRadius = circle.radius;
        Vector3 circlePosition = circle.transform.position;

        float otherCircleRadius = otherCircle.radius;
        Vector3 otherCirclePosition = otherCircle.transform.position;

        Vector2 distanceVector = otherCirclePosition - circlePosition;
        float distance = distanceVector.magnitude;

        if (distance < circleRadius + otherCircleRadius) return true;
        else return false;
    }
    private void CountDownCollisionCheckFrequency()
    {
        if (collisionCheckFrequency > 0f)
            collisionCheckFrequency -= Time.deltaTime;
        collisionCheckFrequency = Mathf.Max(collisionCheckFrequency, 0f);
    }

    private void PoolGameLoopController_OnGameEndStart(string arg1, int arg2)
    {
        shouldCheckCollision = false;
    }
    private void PoolGameLoopController_OnGameRestart(object sender, EventArgs e)
    {
        shouldCheckCollision = true;
    }
}
