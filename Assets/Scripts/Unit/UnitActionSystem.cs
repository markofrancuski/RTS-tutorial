using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedUnitActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;

    public Unit GetSelectedUnit => _selectedUnit;
    public BaseAction GetSelectedAction => _selectedAction;
    public bool IsBusy => _isBusy;

    [SerializeField] private BaseAction _selectedAction;
    [SerializeField] private Unit _selectedUnit;
    [SerializeField] private LayerMask _unitLayerMask;

    private bool _isBusy;

    #region Unity Methods
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"There's more the one UnitActionSystem!", this);
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        SetSelectedUnit(_selectedUnit);
    }
    private void Update()
    {
        if (_isBusy)
        {
            return;
        }

        if (!TurnSystem.Instance.IsPlayerTurn)
        {
            return;
        }

        // Mouse is over UI Element.
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (TryHandleUnitSelection())
        {
            return;
        }

        HandleSelectedAction();
    }

    #endregion Unity Methods

    #region Public Methods

    public void SetSelectedAction(BaseAction baseAction)
    {
        _selectedAction = baseAction;
        OnSelectedUnitActionChanged?.Invoke(this, EventArgs.Empty);
    }

    #endregion Public Methods

    #region Private Methods
    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _unitLayerMask))
            {
                if(raycastHit.transform.TryGetComponent(out Unit unit))
                {
                    // Already Selected
                    if (unit == _selectedUnit)
                    {
                        return false;
                    }
                    if (unit.IsEnemy)
                    {
                        return false;
                    }

                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }

        return false;
    }
    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (!_selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                return;
            }

            if (!_selectedUnit.TrySpendActionPointsToTakeAction(_selectedAction))
            {
                Debug.Log($"{_selectedUnit.name} cannot take action {_selectedAction.GetActionName()}", this);
                return;
            }

            BaseAction.BaseParameters baseParams = new BaseAction.BaseParameters(ClearBusy);
            switch (_selectedAction)
            {
                case MoveAction moveAction:
                    baseParams = new MoveAction.MoveBaseParameters(ClearBusy, mouseGridPosition);
                    break;
                case SpinAction spinAction:
                    baseParams = new SpinAction.SpinBaseParameters(ClearBusy);
                    break;
                case ShootAction shootAction:
                    baseParams = new ShootAction.ShootParameters(ClearBusy, mouseGridPosition);
                    break;
                default:
                    break;
            }

            SetBusy();
            _selectedAction.TakeAction(baseParams);

            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }
    }
    private void SetSelectedUnit(Unit unit)
    {
        _selectedUnit = unit;
        SetSelectedAction(unit.GetMoveAction());
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SetBusy()
    {
        _isBusy = true;
        OnBusyChanged?.Invoke(this, _isBusy);
    }

    private void ClearBusy()
    {
        _isBusy = false;
        OnBusyChanged?.Invoke(this, _isBusy);
    }


    #endregion Private Methods

}
