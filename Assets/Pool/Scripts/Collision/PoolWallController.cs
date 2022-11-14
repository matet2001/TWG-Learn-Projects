using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolWallController : MonoBehaviour
{
    public float width { protected set; get; }
    public float height { protected set; get; }

    public enum wallDirectionEnum { vertical, horizontal};
    public wallDirectionEnum wallDirection;

    private void Awake()
    {
        SpriteRenderer spriteRender = GetComponent<SpriteRenderer>();
        width = spriteRender.bounds.extents.x * 2;
        height = spriteRender.bounds.extents.y * 2;
    }
    public float GetColliderSideLength(Vector3 ballPosition)
    {
        Vector2 normalDirectionVector = CalculateNormalDirectionVector(ballPosition);
        float returnValue = 0f;

        if (normalDirectionVector == Vector2.right || normalDirectionVector == Vector2.left)
            returnValue = width / 2f;
        else if (normalDirectionVector == Vector2.up || normalDirectionVector == Vector2.down)
            returnValue = height / 2f;
        else Debug.Log("Something went wrong at side length calculation!");

        return returnValue;
    }
    public Vector2 CalculateNormalDirectionVector(Vector3 ballPosition)
    {
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
        //Debug.DrawLine(transform.position, (Vector2)transform.position + returnDirection);
        return returnDirection;
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawLine(transform.position - new Vector3(width / 2, 0f), transform.position + new Vector3(width / 2, 0f));
    //    Gizmos.DrawLine(transform.position - new Vector3(0f ,height / 2), transform.position + new Vector3(0f, height / 2));
    //}
}
