using System;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float _totalSpinAmount;

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }
        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
        _totalSpinAmount += spinAddAmount;
        if (_totalSpinAmount >= 360)
        {
            _isActive = false;
            _onActionComplete();
        }
    }

    public override void TakeAction(BaseParameters baseParams)
    {
        _onActionComplete = baseParams.OnActionComplete;
        _isActive = true;
        _totalSpinAmount = 0;
    }

    public override string GetActionName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidActionGridPositions()
    {
        GridPosition unitGridPosition = _unit.GetGridPosition();
        return new List<GridPosition>
        {
            unitGridPosition
        };
    }

    public class SpinBaseParameters : BaseParameters
    {
        public SpinBaseParameters(Action onActionComplete) : base(onActionComplete)
        {

        }
    }
}
