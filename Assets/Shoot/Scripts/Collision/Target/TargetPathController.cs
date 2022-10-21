using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPathController : MonoBehaviour
{
    private TargetCollidable targetCollidable;

    [SerializeField] float moveZoneHeight;
    private Vector2 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
        targetCollidable = GetComponent<TargetCollidable>();
    }
    private void Update()
    { 
        ChangeDirection();
    } 
    private void ChangeDirection()
    {
        Vector2 moveZoneVector = new Vector2(0f, moveZoneHeight);
        Vector3 borderPostion = startPosition + moveZoneVector * Mathf.Sign(targetCollidable.speed);

        if (Vector3.Distance(transform.position, borderPostion) < 0.1f) targetCollidable.speed *= -1;
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
            Gizmos.DrawWireSphere(transform.position, targetCollidable.radius);
            Gizmos.DrawLine(startPosition, startPosition + new Vector2(0f, moveZoneHeight));
            Gizmos.DrawLine(startPosition, startPosition - new Vector2(0f, moveZoneHeight));
        }
    }//For debug
}
