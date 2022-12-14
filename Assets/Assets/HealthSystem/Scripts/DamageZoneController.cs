using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageZoneController : MonoBehaviour
{
    [SerializeField] List<DamageSO> damages;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (DamageSO damage in damages)
        {
            if (!collision.CompareTag(damage.tagToDamage)) continue;

            if (collision.TryGetComponent(out HealthSystem healthSystem))
            {

                Vector2 kickbackVector;
                
                if (damage.damageDirection == null)
                {
                    Vector2 relativeVector = collision.transform.position - transform.position;
                    float kickbackDirection = Mathf.Sign(relativeVector.x);
                    kickbackVector = new Vector2(Vector2.one.x * kickbackDirection, Vector2.one.y) * damage.kickBackForce;
                }
                else 
                {
                    kickbackVector = damage.damageDirection.kickBackDirection * damage.kickBackForce;
                }
                                 
                healthSystem.Damage(damage.damageAmount, damage.invincibilityTime, kickbackVector);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (damages == null) return;
        
        foreach (DamageSO damage in damages)
        {
            if (!damage.damageDirection) continue;

            SpriteRenderer spriteRenderer = GetComponentInParent<SpriteRenderer>();
            Bounds bounds = spriteRenderer.bounds;

            Vector2 direction = damage.damageDirection.kickBackDirection;
            float floatDirection = Mathf.Sign(direction.x);

            Vector3 spriteWidthVector = new Vector3(bounds.extents.x, 0f) - new Vector3(-bounds.extents.x, 0f);
            Vector3 startPos = transform.position - spriteWidthVector / 2 * floatDirection;
            Vector2 endPos = startPos + spriteWidthVector * floatDirection;

            Gizmos.DrawLine(startPos, endPos);
            Gizmos.DrawLine(endPos, endPos + new Vector2(-0.25f, 0.25f) * floatDirection);
            Gizmos.DrawLine(endPos, endPos + new Vector2(-0.25f, -0.25f) * floatDirection);
        }
    }
}