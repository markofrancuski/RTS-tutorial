using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private ActionButtonUI _actionButtonPrefab;
    [SerializeField] private Transform _actionButtonContainerTransform;
    [SerializeField] private TextMeshProUGUI _actionPointsText;

    [SerializeField] private List<ActionButtonUI> _actionButtons = new List<ActionButtonUI>();

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedUnitActionChanged += UnitActionSystem_OnSelectedUnitActionChanged;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;


        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedUnitActionChanged -= UnitActionSystem_OnSelectedUnitActionChanged;
        UnitActionSystem.Instance.OnActionStarted -= UnitActionSystem_OnActionStarted;
        TurnSystem.Instance.OnTurnChanged -= TurnSystem_OnTurnChanged;
        Unit.OnAnyActionPointsChanged -= Unit_OnAnyActionPointsChanged;
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

    private void UpdateSelectedVisual()
    {
        foreach (ActionButtonUI actionButton in _actionButtons)
        {
            actionButton.UpdateSelectedVisual();
        }
    }
    private void UpdateActionPoints()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit;

        _actionPointsText.text = $"Action Points: {selectedUnit.GetActionPoints}";
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, System.EventArgs e)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }
    private void UnitActionSystem_OnSelectedUnitActionChanged(object sender, System.EventArgs e)
    {
        UpdateSelectedVisual();
    }
    private void UnitActionSystem_OnActionStarted(object sender, System.EventArgs e)
    {
        UpdateActionPoints();
    }
    private void TurnSystem_OnTurnChanged(object sender, System.EventArgs e)
    {
        UpdateActionPoints();
    }
    private void Unit_OnAnyActionPointsChanged(object sender, System.EventArgs e)
    {
        UpdateActionPoints();
    }

}
