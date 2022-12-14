using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolCollidable : MonoBehaviour
{
    public abstract bool CheckCollision(PoolBallCollider ballCollider);
}
public abstract class PoolCollidableCircle : PoolCollidable
{
    public float radius { get; private set; }

    public void Awake()
    {
        radius = GetComponent<SpriteRenderer>().bounds.extents.x;
    }
    public static bool CalculateCircleCollision(PoolCollidableCircle circle, PoolCollidableCircle otherCircle)
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
}
