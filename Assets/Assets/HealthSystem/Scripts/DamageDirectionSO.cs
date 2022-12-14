using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageDirection", menuName = "ScriptableObjects/HealthSystem/DamageDirection")]
public class DamageDirectionSO : ScriptableObject
{
    public Vector2 kickBackDirection;
}

