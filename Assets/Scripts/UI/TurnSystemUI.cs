using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button _endTurnButton;
    [SerializeField] private TextMeshProUGUI _turnNumberText;

    private void Start()
    {
        _endTurnButton.onClick.AddListener(EndTurnButton);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        UpdateTurnText();
    }
    private void OnDestroy()
    {
        TurnSystem.Instance.OnTurnChanged -= TurnSystem_OnTurnChanged;
    }

    private void EndTurnButton()
    {
        TurnSystem.Instance.NextTurn();
    }

    private void UpdateTurnText()
    {
        int turnNumber = TurnSystem.Instance.GetTurnNumber;
        _turnNumberText.text = $"TURN: {turnNumber}";
    }

    private void TurnSystem_OnTurnChanged(object sender, System.EventArgs e)
    {
        UpdateTurnText();
    }

}
