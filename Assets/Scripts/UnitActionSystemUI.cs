using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{

    [SerializeField] private ActionButtonUI _actionButtonPrefab;
    [SerializeField] private Transform _actionButtonContainerTransform;

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        CreateUnitActionButtons();
    }
    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
    }

    private void CreateUnitActionButtons()
    {
        foreach (Transform child in _actionButtonContainerTransform)
        {
            Destroy(child.gameObject);
        }

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit;

        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray)
        {
            ActionButtonUI actionButtonUI = Instantiate(_actionButtonPrefab, _actionButtonContainerTransform);
            actionButtonUI.SetBaseAction(baseAction);
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, System.EventArgs e)
    {
        CreateUnitActionButtons();
    }

}
