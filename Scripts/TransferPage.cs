using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Manages the transfer UI page
public class TransferPage : MonoBehaviour {
    public GameObject atmTerminal;
    public GameObject accountSelectionPage;
    public TMP_InputField accountInputField;
    public Button accountConfirmButton;
    public Button accountBackButton;

    public GameObject amountSelectionPage;

    public Button transfer10Button;
    public Button transfer20Button;
    public Button transfer50Button;
    public Button transfer100Button;
    public Button transfer200Button;
    public Button transfer500Button;
    public Button transferGivenAmountButton;
    public Button transferBackButton;

    public TMP_Text accountWarningPrompt;
    public TMP_Text amountWarningPrompt;
    public GameObject successPrompt;
    public GameObject processingPrompt;
    public TMP_Text balancePrompt;
    public TMP_Text receiverPrompt;
    public TMP_InputField manualAmountInputField;

    private ATM _atm;
    private bool _accountSelected;
    private Account _currentReceiverAccount;
    public bool occupied;
    public bool requireUpdate;

    // Initialise the page
    public void Init() {
        ResetUI();
        _accountSelected = false;
        requireUpdate = true;
    }

    // Reset the UI elements
    private void ResetUI() {
        accountInputField.text = "";
        successPrompt.SetActive(false);
        processingPrompt.SetActive(false);
        accountSelectionPage.SetActive(true);
        amountSelectionPage.SetActive(false);
        accountWarningPrompt.gameObject.SetActive(false);
        amountWarningPrompt.gameObject.SetActive(false);
    }

    // Add action listeners for "fixed amount" buttons
    private void AddTransferListener(Button button, int amount) {
        button.onClick.AddListener(() => RequestTransfer(amount));
    }

    // Action listener for account input confirm button
    private void OnAccountConfirmButtonClick() {
        if (!ValidateAccountInput(out int account)) {
            ShowAccountNotFoundWarning();
            return;
        }

        if (account == _atm.GetCurrentAccount().getAccountNum()) {
            ShowSelfTransferWarning();
            return;
        }

        Account[] allAccounts = _atm.GetAllAccounts();
        foreach (Account acc in allAccounts) {
            if (acc.getAccountNum() == account) {
                _atm.PlaySuccessSound();
                _currentReceiverAccount = acc;
                UpdateInfo();
                accountSelectionPage.SetActive(false);
                amountSelectionPage.SetActive(true);
                _accountSelected = true;
                requireUpdate = true;
                return;
            }
        }

        ShowAccountNotFoundWarning();
    }

    // Validate the account input
    private bool ValidateAccountInput(out int account) {
        account = 0;
        if (!int.TryParse(accountInputField.text, out account)) {
            Debug.LogWarning("Invalid input!");
            return false;
        }

        return true;
    }

    // When the "manual amount" button clicked
    private void OnTransferGivenAmountClicked() {
        if (!int.TryParse(manualAmountInputField.text, out int givenAmount)) {
            ShowInvalidAmountWarning();
            return;
        }

        RequestTransfer(givenAmount);
    }

    // Request for a transfer
    private void RequestTransfer(int amount) {
        _atm.PlayPressSound();
        ShowProcessing();

        _atm.Transfer(_currentReceiverAccount, amount, (result) => {
            if (result) {
                ShowSuccess();
                balancePrompt.text = "Your Balance: " + _atm.ObtainBalance();

                _atm.SynchroniseData();
            } else {
                if (occupied) {
                    processingPrompt.SetActive(false);
                    occupied = false;
                    return;
                }

                ShowInsufficientBalanceWarning();
            }
        });
    }

    // Called when the back button from the amount selection page is clicked
    private void OnBackButtonClick() {
        _atm.PlayPressSound();
        _atm.TransferFinished();
    }

    // Insufficient balance warning
    private void ShowInsufficientBalanceWarning() {
        ShowWarning(amountWarningPrompt, "Insufficient balance!");
    }

    // Invalid amount warning
    private void ShowInvalidAmountWarning() {
        ShowWarning(amountWarningPrompt, "Please input a valid amount!");
    }

    // Account not found warning
    private void ShowAccountNotFoundWarning() {
        ShowWarning(accountWarningPrompt, "Account not found!");
    }

    // Transfer receiver is the current account itself not warning
    private void ShowSelfTransferWarning() {
        ShowWarning(accountWarningPrompt, "You cannot transfer to yourself!");
    }

    // Show warning message
    private void ShowWarning(TMP_Text warningPrompt, string message) {
        _atm.PlayFailureSound();
        successPrompt.SetActive(false);
        processingPrompt.SetActive(false);
        warningPrompt.text = message;
        warningPrompt.gameObject.SetActive(true);
    }

    // Display the success status
    private void ShowSuccess() {
        successPrompt.SetActive(true);
        processingPrompt.SetActive(false);
        amountWarningPrompt.gameObject.SetActive(false);
        _atm.PlaySuccessSound();
    }

    // Display the processing status
    private void ShowProcessing() {
        processingPrompt.SetActive(true);
        successPrompt.SetActive(false);
        amountWarningPrompt.gameObject.SetActive(false);
    }

    // Update the balance display
    public void UpdateInfo() {
        balancePrompt.text = "Your Balance: " + _atm.ObtainBalance();
        receiverPrompt.text = "To: " + _currentReceiverAccount.getAccountNum();
    }

    // Initialisation
    private void Start() {
        _atm = atmTerminal.GetComponent<ATM>();

        transferBackButton.onClick.AddListener(OnBackButtonClick);
        transferGivenAmountButton.onClick.AddListener(OnTransferGivenAmountClicked);
        AddTransferListeners();
        accountConfirmButton.onClick.AddListener(OnAccountConfirmButtonClick);
        accountBackButton.onClick.AddListener(OnBackButtonClick);

        accountSelectionPage.SetActive(true);
        amountSelectionPage.SetActive(false);

        successPrompt.SetActive(false);
        accountWarningPrompt.gameObject.SetActive(false);
        amountWarningPrompt.gameObject.SetActive(false);

        // Initialise the balance display
        balancePrompt.text = "Your Balance: " + _atm.ObtainBalance();
    }

    // Add transfer amount button listeners
    private void AddTransferListeners() {
        AddTransferListener(transfer10Button, 10);
        AddTransferListener(transfer20Button, 20);
        AddTransferListener(transfer50Button, 50);
        AddTransferListener(transfer100Button, 100);
        AddTransferListener(transfer200Button, 200);
        AddTransferListener(transfer500Button, 500);
    }

    private void Update() {
        if (!requireUpdate) return;

        // Manages the action listeners of physical buttons
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

        if (_accountSelected) {
            AddTransferListener(_atm.button1, 10);
            AddTransferListener(_atm.button2, 50);
            AddTransferListener(_atm.button3, 200);
            _atm.button4.onClick.AddListener(OnTransferGivenAmountClicked);
            AddTransferListener(_atm.button5, 20);
            AddTransferListener(_atm.button6, 100);
            AddTransferListener(_atm.button7, 500);
            _atm.button8.onClick.AddListener(OnBackButtonClick);
        } else {
            _atm.button4.onClick.AddListener(OnAccountConfirmButtonClick);
            _atm.button8.onClick.AddListener(OnBackButtonClick);
        }
    }
}