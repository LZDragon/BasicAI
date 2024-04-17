using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void OnKilled_Delegate();
public delegate void UpdateHealth_Delegate(float newHealth);
public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100.0f;

    private float health;
    public float Health
    {
        get => health;
        protected set
        {
            health = value;
            HandleHealthUpdated.Invoke(health);
        }
    }

    public OnKilled_Delegate HandleOnKilled;
    public UpdateHealth_Delegate HandleHealthUpdated;

    private void Awake()
    {
        health = maxHealth;
    }

    public void TakeDamage(float inDamage)
    {
        Health -= inDamage;
        if (Health <= 0)
        {
             HandleOnKilled.Invoke();
        }
    }

    
}