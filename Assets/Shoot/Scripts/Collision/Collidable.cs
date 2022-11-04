using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collidable : MonoBehaviour
{
    public float radius { get; protected set;}
    public float speed;
    public Vector2 direction { get; protected set; }

    public void Awake()
    {
        radius = GetComponent<SpriteRenderer>().bounds.extents.x;
    }
    public abstract void Collision(Transform transform);
    public void Move()
    {
        Vector2 moveVector = direction * speed * Time.deltaTime;
        transform.position += new Vector3(moveVector.x, moveVector.y);
    }
}
public abstract class CollidableProjectile : Collidable
{
    public void SetDirection(Vector2 direction) => this.direction = direction;
    public void SetSpeed(float speed) => this.speed = speed;

    public void DestroyProjectile()
    {
        CollisionManager.Instance.RemoveFromCollidableList(this);
        Destroy(gameObject);
    }
}
