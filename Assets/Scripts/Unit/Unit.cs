using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public static event EventHandler OnAnyActionPointsChanged;

    private GridPosition _gridPosition;
    private MoveAction _moveAction;
    private SpinAction _spinAction;
    private int _actionPoints = 2;
    [SerializeField] private int _actionPointsMax = 2;

    private BaseAction[] _baseActionArray;

    #region Unity Methods

    private void Awake()
    {
        _moveAction = GetComponent<MoveAction>();
        _spinAction = GetComponent<SpinAction>();
        _baseActionArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
        _actionPoints = _actionPointsMax;
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != _gridPosition) 
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, _gridPosition, newGridPosition);
            _gridPosition = newGridPosition;
        }
    }

    #endregion Unity Methods
    public BaseAction[] GetBaseActionArray => _baseActionArray;
    public int GetActionPoints => _actionPoints;
    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }
    public MoveAction GetMoveAction()
    {
        return _moveAction;
    }
    public SpinAction GetSpinAction()
    {
        return _spinAction;
    }
    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (!CanSpendActionPointsToTakeAction(baseAction))
        {
            return false;
        }
        SpendActionPoints(baseAction.GetActionPointsCost());
        return true;

    }

    private bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (_actionPoints >= baseAction.GetActionPointsCost())
        {
            return true;
        }
        return false;
    }
    private void SpendActionPoints(int amount)
    {
        _actionPoints -= amount;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }
    private void TurnSystem_OnTurnChanged(object sender, System.EventArgs e)
    {
        _actionPoints = _actionPointsMax;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public override string ToString()
    {
        return gameObject.name;
    }
}
