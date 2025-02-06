using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Manages the withdraw UI page
public class WithdrawPage : MonoBehaviour {
    public GameObject atmTerminal;
    public Button withdraw10Button;
    public Button withdraw20Button;
    public Button withdraw50Button;
    public Button withdraw100Button;
    public Button withdraw200Button;
    public Button withdraw500Button;
    public Button withdrawGivenAmountButton;
    public Button backButton;

    public GameObject successPrompt;
    public GameObject processingPrompt;
    public TMP_Text warningPrompt;
    public TMP_Text balancePrompt;
    public TMP_InputField manualAmountInputField;

    private ATM _atm;
    public bool occupied;
    public bool requireUpdate;

    // Adding an action listener for each of the "withdraw fixed amount" buttons
    private void AddWithdrawListener(Button button, int amount) {
        button.onClick.AddListener(() => RequestWithdraw(amount));
    }

    // Called when the "withdraw manual amount" button is clicked
    private void OnWithdrawGivenAmountClicked() {
        if (int.TryParse(manualAmountInputField.text, out int givenAmount)) {
            RequestWithdraw(givenAmount);
        } else {
            Debug.LogWarning("请输入有效的取款金额");
            ShowInvalidAmountWarning();
        }
    }

    // Request a withdraw operation. The ATM.Withdraw() function will be called.
    private void RequestWithdraw(int amount) {
        _atm.PlayPressSound();
        ShowProcessing();

        _atm.Withdraw(amount, (result) => {
            if (result) {
                ShowSuccess();
                Debug.Log("SUCCESS");
                UpdateBalance();
                
                _atm.SynchroniseData();
            } else {
                if (occupied) {
                    processingPrompt.SetActive(false);
                    occupied = false;
                    return;
                }

                Debug.Log("FAILED");
                ShowInsufficientBalanceWarning();
            }
        });
    }

    // Called when the back button is clicked
    private void OnBackButtonClick() {
        _atm.PlayPressSound();
        ResetUI();
        _atm.WithdrawFinished();
    }

    // Reset the UI elements
    private void ResetUI() {
        successPrompt.SetActive(false);
        processingPrompt.SetActive(false);
        warningPrompt.gameObject.SetActive(false);
    }

    // Insufficient balance warning
    private void ShowInsufficientBalanceWarning() {
        ShowWarning("Insufficient balance!");
    }

    // Invalid amount warning
    private void ShowInvalidAmountWarning() {
        ShowWarning("Please input a valid amount!");
    }

    // Show a warning message
    private void ShowWarning(string message) {
        _atm.PlayFailureSound();
        successPrompt.SetActive(false);
        processingPrompt.SetActive(false);
        warningPrompt.text = message;
        warningPrompt.gameObject.SetActive(true);
    }

    // Display the success status
    private void ShowSuccess() {
        _atm.PlaySuccessSound();
        _atm.PlayCashOutSound();
        successPrompt.SetActive(true);
        processingPrompt.SetActive(false);
        warningPrompt.gameObject.SetActive(false);
    }

    // Display the processing status
    private void ShowProcessing() {
        processingPrompt.SetActive(true);
        successPrompt.SetActive(false);
        warningPrompt.gameObject.SetActive(false);
    }

    // Update the balance display
    public void UpdateBalance() {
        int balance = _atm.ObtainBalance();
        if (balance == -1) {
            ShowWarning("Account is temporarily locked...");
        } else {
            balancePrompt.text = "Your Balance: " + balance;
        }
    }

    // Initialisation
    private void Start() {
        _atm = atmTerminal.GetComponent<ATM>();

        backButton.onClick.AddListener(OnBackButtonClick);
        withdrawGivenAmountButton.onClick.AddListener(OnWithdrawGivenAmountClicked);
        AddWithdrawListeners();

        ResetUI();

        // Initialise the balance display
        UpdateBalance();
    }

    // Add withdraw amount button listeners
    private void AddWithdrawListeners() {
        AddWithdrawListener(withdraw10Button, 10);
        AddWithdrawListener(withdraw20Button, 20);
        AddWithdrawListener(withdraw50Button, 50);
        AddWithdrawListener(withdraw100Button, 100);
        AddWithdrawListener(withdraw200Button, 200);
        AddWithdrawListener(withdraw500Button, 500);
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

        AddWithdrawListener(_atm.button1, 10);
        AddWithdrawListener(_atm.button2, 50);
        AddWithdrawListener(_atm.button3, 200);
        _atm.button4.onClick.AddListener(OnWithdrawGivenAmountClicked);
        AddWithdrawListener(_atm.button5, 20);
        AddWithdrawListener(_atm.button6, 100);
        AddWithdrawListener(_atm.button7, 500);
        _atm.button8.onClick.AddListener(OnBackButtonClick);
    }
}