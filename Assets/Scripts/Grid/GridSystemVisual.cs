using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }

    [System.Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }
    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        RedSoft,
        Yellow
    }

    [SerializeField] private Transform _gridSystemVisualSinglePrefab;
    [SerializeField] private List<GridVisualTypeMaterial> _gridVisualTypeMaterials;

    private GridSystemVisualSingle[,] _gridSystemVisualSingles;

    #region Unity Methods

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"There's more the one LevelGrid!", this);
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

    }

    private void Start()
    {
        _gridSystemVisualSingles = new GridSystemVisualSingle[
            LevelGrid.Instance.GetWidth(),
            LevelGrid.Instance.GetHeight()
        ];

        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x,z);
                Transform instantiatedTransform = 
                    Instantiate(_gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                _gridSystemVisualSingles[x, z] = instantiatedTransform.GetComponent<GridSystemVisualSingle>();
                _gridSystemVisualSingles[x, z].Hide();
            }
        }

        UpdateGridVisual();

        UnitActionSystem.Instance.OnSelectedUnitActionChanged += UnitActionSystem_OnSelectedUnitActionChanged;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitActionChanged -= UnitActionSystem_OnSelectedUnitActionChanged;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition -= LevelGrid_OnAnyUnitMovedGridPosition;
    }
    
    #endregion Unity Methods

    public void HideAllVisuals()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                _gridSystemVisualSingles[x, z].Hide();
            }
        }
    }

    public void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();

        for (int x = x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range)
                {
                    continue;
                }

                gridPositionList.Add(testGridPosition);
            }
        }

        ShowVisuals(gridPositionList, gridVisualType);
    }
    public void ShowVisuals(List<GridPosition> gridPositions, GridVisualType gridVisualType)
    {
        foreach (GridPosition gridPosition in gridPositions)
        {
            _gridSystemVisualSingles[gridPosition.x, gridPosition.z].Show(GetGridVisualTypeMaterial(gridVisualType));
        }
    }

    public void UpdateGridVisual()
    {
        HideAllVisuals();
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit;
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction;

        GridVisualType gridVisualType;

        switch (selectedAction)
        {
            default:
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Blue;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootDistance(), GridVisualType.RedSoft);
                break;
        }

        List<GridPosition> positions = selectedAction.GetValidActionGridPositions();
        ShowVisuals(positions, gridVisualType);
    }

    private Material GetGridVisualTypeMaterial(GridVisualType type)
    {
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in _gridVisualTypeMaterials)
        {
            if (gridVisualTypeMaterial.gridVisualType == type)
            {
                return gridVisualTypeMaterial.material;
            }
        }

        Debug.LogError($"Could not find GridVisualTypeMaterial for GridVisualType {type}", this);
        return null;
    }
    private void UnitActionSystem_OnSelectedUnitActionChanged(object sender, System.EventArgs e)
    {
        UpdateGridVisual();
    }
    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, System.EventArgs e)
    {
        UpdateGridVisual();
    }
}
