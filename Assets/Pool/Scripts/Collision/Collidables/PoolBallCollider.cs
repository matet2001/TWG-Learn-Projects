using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBallCollider : PoolCollidableCircle
{
    [SerializeField] PoolBallMoveController moveController;
    public Vector2 directionVector
    {
        set => moveController.directionVector = value;
        get => moveController.directionVector;
    }
    private Vector2 directionBeforeCollision;

    [HideInInspector]
    public bool shouldCheckHoleCollision = true;

    public override bool CheckCollision(PoolBallCollider ballCollider)
    {
        return CalculateCircleCollision(this, ballCollider);
    }
    #region Ball Collision
    public void Collision(PoolBallCollider otherBallCollider)
    {
        directionBeforeCollision = directionVector;
        Vector2 reflectDirectionVector = CalculateReflectDirectionVector(otherBallCollider);     
        directionVector = reflectDirectionVector;
    }
    private Vector2 CalculateReflectDirectionVector(PoolBallCollider otherBallCollider)
    {
        Vector2 otherBallPosition = otherBallCollider.transform.position;

        Vector2 ownForceVector = CalculateOwnCreatedForce(otherBallPosition);
        Vector2 otherBallGivedForce = otherBallCollider.CalculateForceGiveToOtherBall(transform.position);
    
        Vector2 reflectDirectionVector = ownForceVector + otherBallGivedForce;
        return reflectDirectionVector;
    }
    private Vector2 CalculateOwnCreatedForce(Vector2 otherPosition)
    {
        if (directionBeforeCollision == Vector2.zero) return Vector2.zero;

        Vector2 normalVector = (otherPosition - (Vector2)transform.position).normalized;
        Vector2 tangentVector = new Vector2(-normalVector.y, normalVector.x);
        
        Vector2 ownForceVector = Vector2.Dot(directionBeforeCollision, tangentVector) * tangentVector;
        return ownForceVector;
    }
    public Vector2 CalculateForceGiveToOtherBall(Vector2 otherPosition)
    {
        if (directionBeforeCollision == Vector2.zero) return Vector2.zero;

        Vector2 normalVector = (otherPosition - (Vector2)transform.position).normalized;
        Vector2 forceGiveToOtherBall = Vector2.Dot(directionBeforeCollision, normalVector) * normalVector;

        return forceGiveToOtherBall;
    }
    #endregion
    #region Wall Collision
    public void Collision(Vector2 normalVector)
    {
        Vector2 reflectVector = Vector2.Reflect(directionVector, normalVector);

        float speedDecreaseAmmount = 0.8f;

        directionVector = reflectVector * speedDecreaseAmmount;
    }
    #endregion
    #region Hole Collision
    public void Collision()
    {
        shouldCheckHoleCollision = false;
        moveController.TurnOff();
    }
    #endregion
    
}
