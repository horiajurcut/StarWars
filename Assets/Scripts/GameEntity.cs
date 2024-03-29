﻿using UnityEngine;

public class GameEntity : MonoBehaviour, IDamageable
{
    public float initialHealth;

    public event System.Action OnDeath;

    public float health { get; protected set; }
    protected bool dead;

    protected virtual void Start()
    {
        health = initialHealth;
    }

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        TakeDamage(damage);
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f && !dead)
        {
            Die();
        }
    }

    [ContextMenu("Self Destruct")]
    public virtual void Die()
    {
        if (dead == true) return;

        dead = true;
        if (OnDeath != null)
        {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }
}
