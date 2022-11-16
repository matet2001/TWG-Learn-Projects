using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolHoleController : PoolCollidableCircle
{
    [field:SerializeField] public static float holeEnterSpeedMax = 10f;
    
    public override bool CheckCollision(PoolBallCollider ballCollider)
    {
        if (!CalculateCircleCollision(ballCollider, this)) return false;
        if (ballCollider.directionVector.magnitude > holeEnterSpeedMax) return false;

        return true;
    }
}
