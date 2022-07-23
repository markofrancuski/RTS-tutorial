using UnityEngine;

public class GridSystem
{
    private int _width;
    private int _height;
    private float _cellSize;
    private GridObject[,] _gridObjectArray;

    #region Contructors
    
    public GridSystem(int width, int height, float cellSize)
    {
        this._height = height;
        this._width = width;
        this._cellSize = cellSize;

        _gridObjectArray = new GridObject[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                _gridObjectArray[x,z] = new GridObject(gridPosition, this);
            }
        }
    }

    #endregion Contructors
    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);

                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * _cellSize;
    }
    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / _cellSize),
            Mathf.RoundToInt(worldPosition.z / _cellSize)
        );
    }

    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return _gridObjectArray[gridPosition.x, gridPosition.z];
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return
            gridPosition.x >= 0 &&
            gridPosition.z >= 0 &&
            gridPosition.x < _width &&
            gridPosition.z < _height;
    }
}
