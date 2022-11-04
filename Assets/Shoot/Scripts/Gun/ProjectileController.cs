using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : CollidableProjectile
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
}
