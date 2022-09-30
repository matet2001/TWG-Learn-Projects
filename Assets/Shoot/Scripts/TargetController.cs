using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : Collidable
{
    [SerializeField] float moveZoneHeight;

    private Vector2 startPosition;

    private new void Awake()
    {
        base.Awake();
        startPosition = transform.position;
    }
    private void Update()
    {
        Move();
        ChangeDirection();
    } 
    private void ChangeDirection()
    {
        Vector2 moveZoneVector = new Vector2(0f, moveZoneHeight);
        Vector3 borderPostion = startPosition + moveZoneVector * Mathf.Sign(speed);

        if (Vector3.Distance(transform.position, borderPostion) < 0.1f) speed *= -1;
    }
    public override void Collision(Transform transform)
    {
        if(transform.CompareTag("Projectile"))
        {
            Debug.Log("Hit");
        }
    }
    private void OnDrawGizmos()
    {
        if(!Application.isPlaying)
        {
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(0f, moveZoneHeight));
            Gizmos.DrawLine(transform.position, transform.position - new Vector3(0f, moveZoneHeight));
        }
        else
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }//For debug
}
