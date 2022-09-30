using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collidable : MonoBehaviour
{
    public float radius;
    public float speed;
    public Vector2 direction;

    public void Awake()
    {
        radius = GetComponent<SpriteRenderer>().bounds.extents.y;
    }
    public abstract void Collision(Transform transform);
    public void Move()
    {
        Vector2 moveVector = direction * speed;
        transform.position += new Vector3(moveVector.x, moveVector.y);
    }
}
