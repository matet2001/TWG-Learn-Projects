using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonProjectileController : CollidableProjectile
{
    public float wind { private get; set; }
    
    private void Update()
    {
        Move();
        ApplieForces();
    }
    private void ApplieForces()
    {
        float valueDiviner = 1000f;
        float windValue = wind / valueDiviner;
        float gravityValue = CanonProjectileManager.gravity / valueDiviner;

        direction = new Vector2(direction.x + windValue, direction.y - gravityValue);
    }
    public override void Collision(Transform transform)
    {
        if (transform.CompareTag("Target"))
        {
            DestroyProjectile();
        }
    }
}
