using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PoolCollisionManager : MonoBehaviour
{
    public static PoolCollisionManager Instance;

    public PoolBallCollider targetBall;
    [SerializeField] PoolGameLoopController poolGameLoopController;
    [SerializeField] PoolBallCollider[] poolBallColliderArray;
    [SerializeField] PoolWallController[] poolWallControllerArray;
    [SerializeField] PoolHoleController[] poolHoleControllerArray;

    private List<PoolBallCollider> ballColliderTempList;

    [SerializeField] float collisionCheckFrequency = 1f;
    [SerializeField] bool showDebugLines;
    
    private float collisionCheckFrequencyMax;
    
    private void Awake()
    {
        Instance = this;
        ballColliderTempList = new List<PoolBallCollider>();
        collisionCheckFrequencyMax = collisionCheckFrequency;
    }
    private void Update()
    {
        CheckForCollision();
        CountDownCollisionCheckFrequency();
    }
    private void CheckForCollision()
    {
        if (collisionCheckFrequency > 0f) return;
         
        foreach (PoolBallCollider ballCollider in poolBallColliderArray)
        {
            BallToBallCollision(ballCollider);
            WallToBallCollision(ballCollider);
            HoleToBallCollision(ballCollider);
        }
    }
    private void BallToBallCollision(PoolBallCollider ballCollider)
    {
        if(ballColliderTempList.Count < 1) ballColliderTempList = poolBallColliderArray.ToList();
        ballColliderTempList.Remove(ballCollider);

        foreach (PoolBallCollider nextBallCollider in ballColliderTempList)
        {
            if (ballCollider.CheckCollision(nextBallCollider))
            {
                ballCollider.Collision(nextBallCollider);
                nextBallCollider.Collision(ballCollider);

                ResetCollisionFrequencyTimer();
            }
        } 
    }
    private void WallToBallCollision(PoolBallCollider ballCollider)
    {
        foreach (PoolWallController wallController in poolWallControllerArray)
        {
            if (!wallController.CheckCollision(ballCollider)) continue;

            Vector2 normalDirectionVector = wallController.CalculateNormalDirectionVector(ballCollider);
            ballCollider.Collision(normalDirectionVector);
            ResetCollisionFrequencyTimer();
        }
    }
    private void HoleToBallCollision(PoolBallCollider ballCollider)
    {
        if (!ballCollider.shouldCheckHoleCollision) return;
            
        foreach (PoolHoleController holeController in poolHoleControllerArray)
        {
            if (!holeController.CheckCollision(ballCollider)) continue;

            ballCollider.Collision();
            ResetCollisionFrequencyTimer();

            bool isWin = ballCollider != targetBall;
            poolGameLoopController.StartGameEnd(isWin); 
            return;
        }

    }
    private void CountDownCollisionCheckFrequency()
    {
        if (collisionCheckFrequency > 0f)
            collisionCheckFrequency -= Time.deltaTime;
        collisionCheckFrequency = Mathf.Max(collisionCheckFrequency, 0f);
    }
    private void ResetCollisionFrequencyTimer()
    {
        collisionCheckFrequency = collisionCheckFrequencyMax;
    }  
}
