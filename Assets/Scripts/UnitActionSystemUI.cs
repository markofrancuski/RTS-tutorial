using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private ActionButtonUI _actionButtonPrefab;
    [SerializeField] private Transform _actionButtonContainerTransform;

    [SerializeField] private List<ActionButtonUI> _actionButtons = new List<ActionButtonUI>();

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedUnitActionChanged += Instance_OnSelectedUnitActionChanged;
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedUnitActionChanged -= Instance_OnSelectedUnitActionChanged;
    }

    private void CreateUnitActionButtons()
    {
        foreach (Transform child in _actionButtonContainerTransform)
        {
            Destroy(child.gameObject);
        }

        _actionButtons.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit;

        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray)
        {
            ActionButtonUI actionButtonUI = Instantiate(_actionButtonPrefab, _actionButtonContainerTransform);
            actionButtonUI.SetBaseAction(baseAction);
            _actionButtons.Add(actionButtonUI);
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, System.EventArgs e)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }

    private void Instance_OnSelectedUnitActionChanged(object sender, System.EventArgs e)
    {
        UpdateSelectedVisual();
    }

    private void UpdateSelectedVisual()
    {
        foreach (ActionButtonUI actionButton in _actionButtons)
        {
            actionButton.UpdateSelectedVisual();
        }
    }

}
