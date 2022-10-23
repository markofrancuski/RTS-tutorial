using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _selectedVisualGO;

    private BaseAction _baseAction;

    public void SetBaseAction(BaseAction baseAction)
    {
        _baseAction = baseAction;

        _textMeshPro.text = baseAction.GetActionName().ToUpper();

        _button.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction;
        _selectedVisualGO.SetActive(selectedAction == _baseAction);
    }
}