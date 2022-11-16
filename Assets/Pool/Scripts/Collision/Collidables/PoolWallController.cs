using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolWallController : PoolCollidable
{
    public float width { protected set; get; }
    public float height { protected set; get; }

    public enum wallDirectionEnum { vertical, horizontal};
    public wallDirectionEnum wallDirection;

    private void Awake()
    {
        GetSpriteBoundSizes();
    }
    private void GetSpriteBoundSizes()
    {
        SpriteRenderer spriteRender = GetComponent<SpriteRenderer>();
        width = spriteRender.bounds.extents.x * 2;
        height = spriteRender.bounds.extents.y * 2;
    }
    public override bool  CheckCollision(PoolBallCollider ballController)
    {
        float distanceFromWall = GetDistanceFromBall(ballController);
        float ballRadius = ballController.radius;

        return distanceFromWall < ballRadius;
    }
    private float GetDistanceFromBall(PoolBallCollider ballController)
    {
        Vector3 ballCenter = ballController.transform.position;
        Vector3 wallCenter = transform.position;

        wallDirectionEnum wallDirectionVertical = wallDirectionEnum.vertical;
        Vector2 triangleAVertex = wallCenter;
        Vector2 triangleBVertex = ballCenter;
        Vector2 triangleCVertex = (wallDirection == wallDirectionVertical) ? new Vector2(triangleBVertex.x, triangleAVertex.y) : new Vector2(triangleAVertex.x, triangleBVertex.y);
        Vector2 triangleASide = triangleBVertex - triangleCVertex;

        //if (showDebugLines && wallController == poolWallControllerArray[0] && ballController == targetBall)
        //{
        //    Debug.DrawLine(triangleAVertex, triangleBVertex, Color.red);
        //    Debug.DrawLine(triangleAVertex, triangleCVertex, Color.blue);
        //    Debug.DrawLine(triangleBVertex, triangleCVertex, Color.green);
        //}

        Vector2 normalDirectionVector = CalculateNormalDirectionVector(ballController);
        float distance = triangleASide.magnitude - GetSpriteSideSizeAtDirection(normalDirectionVector);
        return distance;
    }
    private float GetSpriteSideSizeAtDirection(Vector2 normalDirectionVector)
    {      
        float returnValue = 0f;

        if (normalDirectionVector == Vector2.right || normalDirectionVector == Vector2.left)
            returnValue = width / 2f;
        else if (normalDirectionVector == Vector2.up || normalDirectionVector == Vector2.down)
            returnValue = height / 2f;
        else Debug.Log("Something went wrong at side length calculation!");

        return returnValue;
    }
    public Vector2 CalculateNormalDirectionVector(PoolBallCollider ballCollider)
    {
        Vector3 ballPosition = ballCollider.transform.position;
        Vector2 returnDirection = Vector2.zero;

        if (wallDirection == wallDirectionEnum.vertical)
        {
            if (ballPosition.y > transform.position.y) returnDirection = Vector2.up;
            if (ballPosition.y < transform.position.y) returnDirection = Vector2.down;
        }

        if (wallDirection == wallDirectionEnum.horizontal)
        {
            if (ballPosition.x > transform.position.x) returnDirection = Vector2.right;
            if (ballPosition.x < transform.position.x) returnDirection = Vector2.left;
        }
  
        return returnDirection;
    }
}
