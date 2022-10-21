using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : Collidable
{
    private void Update()
    {
        Move();        
    }
    public override void Collision(Transform transform)
    {
        if (transform.CompareTag("Target"))
        {
            DestroyProjectile();
        }           
    }
    private void DestroyProjectile()
    {
        CollisionManager.Instance.RemoveFromCollidableList(this);
        Destroy(gameObject);
    }
    public void SetDirection(Vector2 direction) => this.direction = direction;
}
