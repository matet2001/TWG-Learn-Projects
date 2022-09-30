using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : Collidable
{
    public override void Collision(Transform transform)
    {
        if (transform.CompareTag("Target"))
            Destroy(gameObject);
    }
}
