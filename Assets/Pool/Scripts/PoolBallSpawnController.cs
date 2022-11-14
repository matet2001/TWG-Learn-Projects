using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBallSpawnController : MonoBehaviour
{
    [SerializeField] PoolGameLoopController poolGameLoopController;
    [SerializeField] PoolBallController[] poolBallControllerArray;
    
    private Bounds bounds;
    
    private void Awake()
    {
        bounds = GetComponent<SpriteRenderer>().bounds;
    }
    private void Start()
    {
        RandomizeBallPositions();

        poolGameLoopController.OnGameEnd += PoolGameLoopController_OnGameEnd;
    }
    private void RandomizeBallPositions()
    {
        List<Vector2> spawnPositions = new List<Vector2>();

        foreach (PoolBallController ball in poolBallControllerArray)
        {
            Vector2 randomPosition;

            do
            {
                randomPosition = CalculateRandomPosition();
            }
            while (spawnPositions.Contains(randomPosition));

            spawnPositions.Add(randomPosition);
            ball.transform.position = randomPosition;
            ball.gameObject.SetActive(true);
        }
    }
    private Vector2 CalculateRandomPosition()
    {
        float width = bounds.extents.x;
        float height = bounds.extents.y - poolBallControllerArray[0].radius;

        float x = Random.Range(-width, width);
        float y = Random.Range(-height, height);

        Vector2 randomPosition = new Vector2(x, y);
        return randomPosition;
    }
    
    private void PoolGameLoopController_OnGameEnd(object sender, System.EventArgs e)
    {
        RandomizeBallPositions();

        poolGameLoopController.TriggerOnGameRestart();
    }
}
