using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    private GridPosition _gridPosition;
    private GridSystem _gridSystem;
    private List<Unit> _units;

    public GridObject(GridPosition gridPosition, GridSystem gridSystem)
    {
        this._gridPosition = gridPosition;
        this._gridSystem = gridSystem;
        this._units = new List<Unit>();
    }

    public List<Unit> GetUnits()
    {
        return this._units;
    }
    public void RemoveUnit(Unit unit)
    {
        this._units.Remove(unit);
    }
    public void AddUnit(Unit unit)
    {
        this._units.Add(unit);
    }

    public bool HasAnyUnit()
    {
        return _units.Count > 0;
    }
    public override string ToString()
    {
        string unitString = "";

        foreach (Unit unit in this._units)
        {
            unitString += unit + "\n";
        }
        return _gridPosition.ToString() + "\n" + unitString;
    }
}
