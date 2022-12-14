using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event Action<int, Vector2> OnDamage;
    public event Action OnDie;
    public event Action OnHeal;

    public int healthPointAmount { private set; get; }

    [SerializeField] int maxHealthAmount;
    [SerializeField] float invincibilityTimer;

    public bool isHurt { private set; get; }
    public bool isDead { private set; get; }

    private void Update()
    {
        CountDown();
    } 
    public void Damage(int damageAmount, float invincibilityTime, Vector2 kickBackVector)
    {
        if (!isHurt) return;

        OnDamage?.Invoke(damageAmount, kickBackVector);

        invincibilityTimer = invincibilityTime;
        isHurt = true;

        AddToHealth(-damageAmount);
        ManageDie();
    }
    public void Heal(int healAmountPoint)
    {
        AddToHealth(healAmountPoint);
        OnHeal?.Invoke();
    }
    private void AddToHealth(int damageAmount)
    {
        healthPointAmount += damageAmount;

        if (maxHealthAmount != 0) healthPointAmount = Mathf.Max(healthPointAmount, 0);
        else healthPointAmount = Mathf.Clamp(healthPointAmount, 0, maxHealthAmount);
    }
    private void ManageDie()
    {
        if (healthPointAmount <= 0 && !isDead)
        {
            OnDie?.Invoke();
            isDead = true;
        }
    }
    private void CountDown()
    {
        if (invincibilityTimer > 0f && isHurt) invincibilityTimer -= Time.deltaTime;
        else
        {
            invincibilityTimer = 0f;
            isHurt = false;
        }
    }
}
