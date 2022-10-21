using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCollidable : Collidable
{
    private new void Awake()
    {
        base.Awake();
        direction = Vector2.up;
    }
    private void Update()
    {
        Move();
    }
    public override void Collision(Transform transform)
    {
        if (transform.CompareTag("Projectile"))
        {
            Debug.Log("Hit");
        }
    }
}
