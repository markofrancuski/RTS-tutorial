using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootAction : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs
    {
        public Unit TargetUnit;
        public Unit ShootingUnit;
    }


    private enum State
    {
        Aiming, 
        Shooting,
        Cooloff
    }

    private State _currentState;
    private int _maxShootDistance = 7;
    private float _stateTimer;
    private bool _canShootBullet;

    private Unit _targetUnit;


    private void Update()
    {
        if (!_isActive)
        {
            return;
        }
        _stateTimer -= Time.deltaTime;

        switch (_currentState)
        {
            case State.Aiming:
                Vector3 moveDirection = (_targetUnit.GetWorldPosition() - _unit.GetWorldPosition()).normalized;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
                break;
            case State.Shooting:
                if (_canShootBullet)
                {
                    Shoot();
                }
                break;
            case State.Cooloff:
                break;
            default:
                break;
        }

        if (_stateTimer <= 0f)
        {
            HandleNextState();
        }
    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPositions()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetGridPosition();

        for (int x = -_maxShootDistance; x <= _maxShootDistance; x++)
        {
            for (int z = -_maxShootDistance; z <= _maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > _maxShootDistance)
                {
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // No units on this position.
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (targetUnit.IsEnemy == _unit.IsEnemy)
                {
                    //Units are on the same team.
                    continue;
                }

                validGridPositions.Add(testGridPosition);
            }
        }


        return validGridPositions;
    }

    public override void TakeAction(BaseParameters baseParams)
    {
        ActionStart(baseParams.OnActionComplete);

        ShootParameters shootParams = baseParams as ShootParameters;
        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(shootParams.GridPosition);
        _canShootBullet = true;

        _currentState = State.Aiming;
        float aimingStateTime = 1f;
        _stateTimer = aimingStateTime;
    }

    private void HandleNextState()
    {
        switch (_currentState)
        {
            case State.Aiming:
                    _currentState = State.Shooting;
                    float shootingStateTime = 0.1f;
                    _stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                    _currentState = State.Cooloff;
                    float coolOffStateTime = 0.5f;
                    _stateTimer = coolOffStateTime;
                break;
            case State.Cooloff:
                ActionComplete();
                break;
            default:
                break;
        }
    }

    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            TargetUnit = _targetUnit,
            ShootingUnit = _unit
        });

        _targetUnit.Damage(40);
        _canShootBullet = false;
    }


    public class ShootParameters: BaseParameters
    {
        public GridPosition GridPosition;

        public ShootParameters(Action onActionComplete, GridPosition gridPosition) : base(onActionComplete)
        {
            this.GridPosition = gridPosition;
        }
    }
}
