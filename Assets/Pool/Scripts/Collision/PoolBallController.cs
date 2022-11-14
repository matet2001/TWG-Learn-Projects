using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBallController : PoolCollidableCircle
{
    public Vector2 directionVector { get; private set; }
    [SerializeField] float friction = 1f;

    private void Update()
    {
        Move();
        CalculateFriction();
    }
    public void Move()
    {
        Vector2 moveVector = directionVector * Time.deltaTime;
        transform.position += new Vector3(moveVector.x, moveVector.y);
    }
    private void CalculateFriction()
    {
        float speed = directionVector.magnitude;
        
        if (speed != 0f)
        {
            speed -= friction * Time.deltaTime * Mathf.Sign(speed);

            if (Mathf.Abs(speed) < 0.01f) directionVector = Vector2.zero;
            else directionVector = directionVector.normalized * speed;
        }
    }
    public void SetDirection(Vector2 directionVector) => this.directionVector = directionVector;

    public void BallCollision(Vector2 otherPosition, Vector2 otherForceVector)
    {
        Vector2 ownForceVector = CalculateOwnCreatedForce(otherPosition);
        directionVector = ownForceVector + otherForceVector;
    }

    private Vector2 CalculateOwnCreatedForce(Vector2 otherPosition)
    {
        if (directionVector == Vector2.zero) return Vector2.zero;

        Vector2 normalVector = (otherPosition - (Vector2)transform.position).normalized;
        Vector2 tangentVector = new Vector2(-normalVector.y, normalVector.x);
        
        Vector2 ownForceVector = Vector2.Dot(directionVector, tangentVector) * tangentVector;
        return ownForceVector;
    }
    public Vector2 CalculateForceGiveToOtherBall(Vector2 otherPosition)
    {
        if (directionVector == Vector2.zero) return Vector2.zero;

        Vector2 normalVector = (otherPosition - (Vector2)transform.position).normalized;
        Vector2 forceGiveToOtherBall = Vector2.Dot(directionVector, normalVector) * normalVector;
        return forceGiveToOtherBall;
    }
    public void WallCollision(Vector2 wallNormalVector)
    {
        float speedDecreaseAmmount = 0.8f;
        
        Vector2 reflectVector = Vector2.Reflect(directionVector, wallNormalVector);
        directionVector = reflectVector * speedDecreaseAmmount;
    }
    public void Stop() => directionVector = Vector2.zero;
}
