using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;

    [SerializeField] private int _health = 100;


    public void Damage(int amount)
    {
        _health -= amount;
        if (_health < 0)
        {
            _health = 0;
        }

        if (_health == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }
}
