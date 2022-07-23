using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Unit _unit;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.T))
        {
            _unit.GetMoveAction().GetValidActionGridPositions();
        }
    }
}
