using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }

    [SerializeField] private Transform _gridSystemVisualSinglePrefab;

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
    }

    private void Update()
    {
        UpdateGridVisual();
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

    public void ShowVisuals(List<GridPosition> gridPositions)
    {
        foreach (GridPosition gridPosition in gridPositions)
        {
            _gridSystemVisualSingles[gridPosition.x, gridPosition.z].Show();
        }
    }

    public void UpdateGridVisual()
    {
        HideAllVisuals();
        BaseAction baseAction = UnitActionSystem.Instance.GetSelectedAction;
        ShowVisuals(baseAction.GetValidActionGridPositions());
    }
}
