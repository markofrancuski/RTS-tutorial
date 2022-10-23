using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Action _onActionComplete;

    protected Unit _unit;
    protected bool _isActive;

    protected virtual void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositions = GetValidActionGridPositions();
        return validGridPositions.Contains(gridPosition);
    }
    public abstract List<GridPosition> GetValidActionGridPositions();
    public abstract void TakeAction(BaseParameters baseParams);
    public abstract string GetActionName();

    public virtual int GetActionPointsCost()
    {
        return 1;
    }

    public class BaseParameters
    {
        public Action OnActionComplete;

        public BaseParameters(Action onActionComplete)
        {
            this.OnActionComplete = onActionComplete;
        }
    }
}
