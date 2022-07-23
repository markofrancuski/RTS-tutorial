using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro _gridVisualText;
    private GridObject _gridObject;

    private void Update()
    {
        UpdateVisual();
    }

    public void SetGridObject(GridObject gridObject)
    {
        this._gridObject = gridObject;
    }

    public void UpdateVisual()
    {
        if (this._gridObject != null)
        {
            return;
        }
        _gridVisualText.text = _gridObject.ToString();
    }
}
