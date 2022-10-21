using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition _gridPosition;
    private MoveAction _moveAction;
    private SpinAction _spinAction;

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
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
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

    public BaseAction[] GetBaseActionArray => _baseActionArray;

    public override string ToString()
    {
        return gameObject.name;
    }
}
