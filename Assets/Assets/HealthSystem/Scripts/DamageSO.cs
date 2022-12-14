using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage", menuName = "ScriptableObjects/HealthSystem/Damage")]
public class DamageSO : ScriptableObject
{
    public string tagToDamage;
    public int damageAmount;
    public float kickBackForce { private set; get; } = 4f;
    public float invincibilityTime { private set; get; } = 1f;

    public DamageDirectionSO damageDirection;
}

