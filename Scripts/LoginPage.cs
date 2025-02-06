using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Manages the Login UI page
public class LoginPage : MonoBehaviour {
    public GameObject atmTerminal;

    public Button loginButton;
    public TMP_InputField accountInputField;
    public TMP_InputField pinInputField;
    public TMP_Text prompt;

    private ATM _atm;
    private string _realPin;
    private string _previousPinInput;

    public bool requireUpdate;

    // When the "login" button is clicked
    private void OnLoginButtonClick() {
        _atm.PlayPressSound();

        // Check the account & pin inputted
        if (!ValidateInput(out int account, out int pin)) {
            return;
        }

        // Check for correctness
        Account[] allAccounts = _atm.GetAllAccounts();
        for (int i = 0; i < allAccounts.Length; i++) {
            if (allAccounts[i].getAccountNum() == account) {
                if (allAccounts[i].checkPin(pin)) {
                    _atm.AccountLogin(allAccounts[i]); // Login successfully
                    prompt.gameObject.SetActive(false);
                    return;
                }

                ShowWarning("Wrong Password!");
                return;
            }
        }

        ShowWarning("Account Not Found!");
    }

    // Validates the input fields and sets the account and pin
    private bool ValidateInput(out int account, out int pin) {
        account = 0;
        pin = 0;

        if (!int.TryParse(accountInputField.text, out account)) {
            Debug.LogWarning("Invalid account input!");
            ShowWarning("Invalid Input!");
            return false;
        }

        if (!int.TryParse(_realPin, out pin)) {
            Debug.LogWarning("Invalid pin input!");
            Debug.Log(_realPin);
            ShowWarning("Invalid Input!");
            return false;
        }

        return true;
    }

    // Displays a warning message
    private void ShowWarning(string message) {
        _atm.PlayFailureSound();
        prompt.text = message;
        prompt.gameObject.SetActive(true);
    }

    private void Start() {
        prompt.gameObject.SetActive(false);
        _atm = atmTerminal.GetComponent<ATM>();
        loginButton.onClick.AddListener(OnLoginButtonClick);

        _atm.PlayEnterPromptSound();

        _realPin = "";
        pinInputField.contentType =
            TMP_InputField.ContentType.Standard; // Set pin InputField to standard to allow custom handling
        pinInputField.onValueChanged.AddListener(OnPinValueChanged); // Add listener for value change
    }

    private void Update() {
        if (!requireUpdate) return;

        // Manages the action listeners of physical buttons
        _atm.button1.onClick.RemoveAllListeners();
        _atm.button2.onClick.RemoveAllListeners();
        _atm.button3.onClick.RemoveAllListeners();
        _atm.button4.onClick.RemoveAllListeners();
        _atm.button5.onClick.RemoveAllListeners();
        _atm.button6.onClick.RemoveAllListeners();
        _atm.button7.onClick.RemoveAllListeners();
        _atm.button8.onClick.RemoveAllListeners();

        _atm.button8.onClick.AddListener(OnLoginButtonClick);

        requireUpdate = false;
    }

    // Display "*" instead of real inputs in PIN input field
    private void OnPinValueChanged(string text) {
        if (_previousPinInput == null) {
            _previousPinInput = "";
        }

        if (text.Length < _previousPinInput.Length) {
            if (_realPin.Length > 0) {
                _realPin = _realPin.Substring(0, _realPin.Length - 1);
            }
        } else if (text.Length > _previousPinInput.Length) {
            string newChars = text.Substring(_previousPinInput.Length);
            _realPin += newChars.Replace("*", "");
        }

        // Create a masked string with the same length as the real password
        string maskedText = new string('*', _realPin.Length);

        pinInputField.onValueChanged.RemoveListener(OnPinValueChanged);
        pinInputField.text = maskedText;
        pinInputField.caretPosition = maskedText.Length;
        _previousPinInput = maskedText;
        pinInputField.onValueChanged.AddListener(OnPinValueChanged);
    }

    // Clears the input fields
    public void Refresh() {
        _atm.PlayEnterPromptSound();
        accountInputField.text = "";
        pinInputField.text = "";
        _realPin = "";
        _previousPinInput = "";

        pinInputField.onValueChanged.RemoveListener(OnPinValueChanged);
        pinInputField.text = "";
        pinInputField.onValueChanged.AddListener(OnPinValueChanged);
    }
}