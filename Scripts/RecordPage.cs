using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

// Manages the transaction record UI page
public class RecordPage : MonoBehaviour {
    public GameObject atmTerminal;

    public Button backButton;

    public ScrollRect recordScrollView;
    public Transform content;
    public GameObject transactionItemPrefab; // Prefab for each transaction display

    private List<Record> records = new();
    private ATM _atm;
    private float _cumulativeHeight;
    private float _recordHeight = 60f;
    private float _padding = 10f;
    public bool requireUpdate;

    // Initialise the page
    public void Init() {
        _atm = atmTerminal.GetComponent<ATM>();
        records = _atm.GetCurrentAccount().getRecords();

        // Empty the board first
        foreach (Transform child in recordScrollView.content.transform) {
            Destroy(child.gameObject);
        }

        _cumulativeHeight = 0f;

        foreach (var record in records) {
            AddTransactionItem(record);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
        recordScrollView.verticalNormalizedPosition = 0f;

        requireUpdate = true;
    }

    // Add a transaction item to the record list
    private void AddTransactionItem(Record record) {
        GameObject newItem = Instantiate(transactionItemPrefab, content);
        RectTransform itemTransform = newItem.GetComponent<RectTransform>();
        itemTransform.anchoredPosition = new Vector2(0f, -_cumulativeHeight);
        _cumulativeHeight += _recordHeight + _padding;

        TMP_Text itemText = newItem.GetComponentInChildren<TMP_Text>();
        itemText.text = GetRecordText(record);
    }

    // Get the display text for a transaction record
    private string GetRecordText(Record record) {
        switch (record.getType()) {
            case 0:
                return $"Withdraw: ${record.getAmount()}";
            case 1:
                return $"Transfer to: {record.getRelatedAccount()} with amount: ${record.getAmount()}";
            case 2:
                return $"Receive from: {record.getRelatedAccount()} with amount: ${record.getAmount()}";
            default:
                return "Unknown transaction type";
        }
    }

    private void Update() {
        if (!requireUpdate) return;

        // Manage the action listeners of physical buttons
        UpdateButtonListeners();

        requireUpdate = false;
    }

    // Updates the action listeners of the ATM buttons
    private void UpdateButtonListeners() {
        _atm.button1.onClick.RemoveAllListeners();
        _atm.button2.onClick.RemoveAllListeners();
        _atm.button3.onClick.RemoveAllListeners();
        _atm.button4.onClick.RemoveAllListeners();
        _atm.button5.onClick.RemoveAllListeners();
        _atm.button6.onClick.RemoveAllListeners();
        _atm.button7.onClick.RemoveAllListeners();
        _atm.button8.onClick.RemoveAllListeners();

        _atm.button8.onClick.AddListener(OnBackButtonClick);
    }

    // When the back to main page button is clicked
    private void OnBackButtonClick() {
        PlayPressSound();
        _atm.RecordFinished();
    }

    private void Start() {
        _atm = atmTerminal.GetComponent<ATM>();
        backButton.onClick.AddListener(OnBackButtonClick);
    }

    // Play the press sound
    private void PlayPressSound() {
        _atm.PlayPressSound();
    }
}