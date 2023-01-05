using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _actionPointsText;
    [SerializeField] private Unit _unit;
    [SerializeField] private Image _healthBarImage;
    [SerializeField] private HealthSystem _healthSystem;

    private void Start()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        _healthSystem.OnDamaged += HealthSystem_OnDamaged;
        UpdateActionPointsText();
        UpdateHealthBarImage();
    }


    private void OnDestroy()
    {
        Unit.OnAnyActionPointsChanged -= Unit_OnAnyActionPointsChanged;
        _healthSystem.OnDamaged -= HealthSystem_OnDamaged;
    }

    private void Unit_OnAnyActionPointsChanged(object sender, System.EventArgs args)
    {
        UpdateActionPointsText();
    }

    private void UpdateActionPointsText()
    {
        _actionPointsText.text = _unit.GetActionPoints.ToString();
    }
    private void UpdateHealthBarImage()
    {
        _healthBarImage.fillAmount = _healthSystem.GetHealthNormalized();
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        UpdateHealthBarImage();
    }
}
