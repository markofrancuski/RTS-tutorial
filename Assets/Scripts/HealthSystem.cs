using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnDamaged;

    private int HealthMax;
    [SerializeField] private int _health = 100;

    private void Awake()
    {
        HealthMax = _health;
    }

    public void Damage(int amount)
    {
        _health -= amount;
        if (_health < 0)
        {
            _health = 0;
        }

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (_health == 0)
        {
            Die();
        }
    }

    public float GetHealthNormalized()
    {
        return (float) _health / HealthMax;
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }
}
