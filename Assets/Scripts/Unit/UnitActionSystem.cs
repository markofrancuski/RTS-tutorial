using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged;

    public Unit GetSelectedUnit => _selectedUnit;
    public BaseAction GetSelectedAction => _selectedAction;

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

            BaseAction.BaseParameters baseParams = new BaseAction.BaseParameters(ClearBusy);
            switch (_selectedAction)
            {
                case MoveAction moveAction:
                    baseParams = new MoveAction.MoveBaseParameters(ClearBusy, mouseGridPosition);
                    break;
                case SpinAction spinAction:
                    baseParams = new SpinAction.SpinBaseParameters(ClearBusy);
                    break;
                default:
                    break;
            }

            SetBusy();
            _selectedAction.TakeAction(baseParams);
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
    }

    private void ClearBusy()
    {
        _isBusy = false;
    }

    #endregion Private Methods

}
