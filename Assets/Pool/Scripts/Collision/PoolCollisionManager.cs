using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoolCollisionManager : MonoBehaviour
{
    public static PoolCollisionManager Instance;

    public PoolBallCollider targetBall;
    [SerializeField] PoolGameLoopController poolGameLoopController;
    [SerializeField] PoolBallCollider[] poolBallColliderArray;
    [SerializeField] PoolWallCollider[] poolWallControllerArray;
    [SerializeField] PoolHoleCollider[] poolHoleControllerArray;

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
    #region CheckCollisions
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
            CheckBallToBallCollision(ballCollider);
            CheckWallToBallCollision(ballCollider);
            CheckHoleToBallCollision(ballCollider);
        }
    }
    private void CheckBallToBallCollision(PoolBallCollider ballCollider)
    {
        if(ballColliderTempList.Count < 1) ballColliderTempList = poolBallColliderArray.ToList();
        ballColliderTempList.Remove(ballCollider);

        foreach (PoolBallCollider nextBallCollider in ballColliderTempList)
        {
            if (ballCollider.CheckCollision(nextBallCollider))
            {
                ballCollider.HandleBallCollision(nextBallCollider);
                ballCollider.flashController.Flash(Color.green, 0.2f);
                nextBallCollider.HandleBallCollision(ballCollider);
                nextBallCollider.flashController.Flash(Color.red, 0.2f);

                SoundManager.PlaySound("_BallHitSound", ballCollider.transform.position);

                ResetCollisionFrequencyTimer();
            }
        } 
    }
    private void CheckWallToBallCollision(PoolBallCollider ballCollider)
    {
        foreach (PoolWallCollider wallController in poolWallControllerArray)
        {
            if (!wallController.CheckCollision(ballCollider)) continue;

            Vector2 normalDirectionVector = wallController.CalculateNormalDirectionVector(ballCollider);
            ballCollider.HandleWallCollision(normalDirectionVector);
            ballCollider.flashController.Flash(Color.green, 0.2f);
            wallController.flashController.Flash(Color.red, 0.2f);

            SoundManager.PlaySound("_WallHitSound", ballCollider.transform.position);

            ResetCollisionFrequencyTimer();
        }
    }
    private void CheckHoleToBallCollision(PoolBallCollider ballCollider)
    {
        if (!ballCollider.shouldCheckHoleCollision) return;
            
        foreach (PoolHoleCollider holeController in poolHoleControllerArray)
        {
            if (!holeController.CheckCollision(ballCollider)) continue;

            ballCollider.HandleHoleCollision();
            ResetCollisionFrequencyTimer();

            bool isWin = ballCollider != targetBall;
            poolGameLoopController.StartGameEnd(isWin); 
            return;
        }

    }
    #endregion
    #region Collision Frequency Timer
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
    #endregion
}
