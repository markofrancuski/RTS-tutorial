using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    [SerializeField] private Animator _animator;

    private Vector3 _targetPosition;
    private float _stopDistance = 0.1f;

    [SerializeField] private float _rotateSpeed = 10f;
    [SerializeField] private float _moveSpeed = 4f;

    [SerializeField] private int _maxMoveDistance = 4;

    #region Unity Methods

    protected override void Awake()
    {
        base.Awake();
        _targetPosition = transform.position;
    }

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;

        if (Vector3.Distance(_targetPosition, transform.position) > _stopDistance)
        {
            transform.position += moveDirection * _moveSpeed * Time.deltaTime;
            _animator.SetBool("IsWalking", true);
        }
        else
        {
            _animator.SetBool("IsWalking", false);
            ActionComplete();
        }

        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed);
    }

    #endregion Unity Methods

    public override void TakeAction(BaseParameters baseParams)
    {
        MoveBaseParameters moveParams = baseParams as MoveBaseParameters;

        _targetPosition = LevelGrid.Instance.GetWorldPosition(moveParams.GridPosition);
        ActionStart(baseParams.OnActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositions()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetGridPosition();

        for (int x = -_maxMoveDistance; x <= _maxMoveDistance; x++)
        {
            for (int z = -_maxMoveDistance; z <= _maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                if (unitGridPosition == testGridPosition)
                {
                    continue;
                }
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }
                validGridPositions.Add(testGridPosition);
            }
        }


        return validGridPositions;
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public class MoveBaseParameters : BaseParameters
    {
        public GridPosition GridPosition;

        public MoveBaseParameters(Action onActionComplete, GridPosition gridPosition) : base(onActionComplete)
        {
            this.GridPosition = gridPosition;
        }
    }

}