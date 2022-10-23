using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnTurnChanged;
    public int GetTurnNumber => _turnNumber;

    private int _turnNumber = 1;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"There's more the one TurnSystem!", this);
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public void NextTurn()
    {
        _turnNumber++;
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }
}
