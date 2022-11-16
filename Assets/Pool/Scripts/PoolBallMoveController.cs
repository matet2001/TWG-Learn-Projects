using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBallMoveController : MonoBehaviour
{
    public Vector2 directionVector;
    [SerializeField] float friction = 5f;

    private void Update()
    {
        Move();
        ReduceSpeedByFriction();
    }
    public void Move()
    {
        Vector2 moveVector = directionVector * Time.deltaTime;
        transform.position += new Vector3(moveVector.x, moveVector.y);
    }
    private void ReduceSpeedByFriction()
    {
        float speed = directionVector.magnitude;

        if (speed != 0f)
        {
            speed -= friction * Time.deltaTime * Mathf.Sign(speed);

            if (Mathf.Abs(speed) < 0.01f) directionVector = Vector2.zero;
            else directionVector = directionVector.normalized * speed;
        }
    }
    public void TurnOff()
    {
        Stop();
        gameObject.SetActive(false);
    }
    private void Stop() => directionVector = Vector2.zero;
}
