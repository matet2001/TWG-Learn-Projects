using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBallSpawnController : MonoBehaviour
{
    [SerializeField] PoolGameLoopController poolGameLoopController;
    [SerializeField] PoolBallCollider[] ballColliderArray;
    
    private Bounds bounds;
    
    private void Awake()
    {
        bounds = GetComponent<SpriteRenderer>().bounds;
    }
    private void Start()
    {
        SuscribeToEvents();
        RandomizeBallPositions();     
    }
    private void SuscribeToEvents()
    {
        poolGameLoopController.OnGameEnd += PoolGameLoopController_OnGameEnd;
    }
    #region Random Spawning
    private void RandomizeBallPositions()
    {
        List<Vector2> spawnPositions = new List<Vector2>();

        foreach (PoolBallCollider ball in ballColliderArray)
        {
            Vector2 randomPosition = CalculateRandomPosition();
            bool canBreakLoop = false;
            
            while (!canBreakLoop)
            {
                canBreakLoop = true;
                randomPosition = CalculateRandomPosition();
                
                foreach (Vector2 position in spawnPositions)
                {
                    if (Vector2.Distance(randomPosition, position) < ball.radius * 2f) 
                        canBreakLoop = false;
                }
            }

            spawnPositions.Add(randomPosition);
            ball.transform.position = randomPosition;
            ball.shouldCheckHoleCollision = true;
            ball.gameObject.SetActive(true);
        }
    }
    private Vector2 CalculateRandomPosition()
    {
        float ballRadius = ballColliderArray[0].radius;

        float width = bounds.extents.x - ballRadius;
        float height = bounds.extents.y - ballRadius;

        float x = Random.Range(-width, width);
        float y = Random.Range(-height, height);

        Vector2 randomPosition = new Vector2(x, y);
        return randomPosition;
    }
    #endregion
    #region Events
    private void PoolGameLoopController_OnGameEnd()
    {
        RandomizeBallPositions();
        poolGameLoopController.TriggerOnGameRestart();
    }
    #endregion
}
