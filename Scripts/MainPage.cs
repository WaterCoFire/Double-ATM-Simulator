using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Manages the main UI page
public class MainPage : MonoBehaviour {
    public GameObject atmTerminal;

    public Button withdrawButton;
    public Button transferButton;
    public Button checkButton;
    public Button checkCloseButton;
    public Button recordButton;
    public Button quitButton;

    public int currentAccount;

    public TMP_Text balancePrompt;
    public GameObject checkPage;

    public TMP_Text welcomePrompt;

    public GameObject quittingPage;

    private ATM _atm;
    private bool _inCheckPage;
    public bool requireUpdate;

    private void Update() {
        if (!requireUpdate) return;

        // Update the welcome prompt with the current account number
        welcomePrompt.text = "Welcome, " + currentAccount + "!";

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

        _atm.button2.onClick.AddListener(OnRecordButtonClick);
        _atm.button3.onClick.AddListener(OnCheckButtonClick);
        _atm.button4.onClick.AddListener(OnWithdrawButtonClick);
        _atm.button7.onClick.AddListener(OnTransferButtonClick);
        _atm.button8.onClick.AddListener(OnQuitButtonClick);
    }

    // When "withdraw" operation clicked
    private void OnWithdrawButtonClick() {
        if (_inCheckPage) return;
        PlayPressSound();
        _atm.WithdrawSelected();
    }

    // When "transfer" operation clicked
    private void OnTransferButtonClick() {
        if (_inCheckPage) return;
        PlayPressSound();
        _atm.TransferSelected();
    }

    // When "record" operation clicked
    private void OnRecordButtonClick() {
        if (_inCheckPage) return;
        PlayPressSound();
        _atm.RecordSelected();
    }

    // When "check balance" operation clicked
    private void OnCheckButtonClick() {
        if (_inCheckPage) return;
        PlayPressSound();
        _inCheckPage = true;
        balancePrompt.text = _atm.ObtainBalance().ToString();
        checkPage.SetActive(true);
    }

    // When the close button of check balance window clicked
    private void OnCheckCloseButtonClick() {
        PlayPressSound();
        _inCheckPage = false;
        checkPage.SetActive(false);
    }

    // When "quit" operation clicked
    private void OnQuitButtonClick() {
        if (_inCheckPage) return;
        PlayPressSound();
        PlayQuittingSound();
        StartCoroutine(ToggleQuittingPage(3f));
    }

    // Play the press sound
    private void PlayPressSound() {
        _atm.PlayPressSound();
    }

    // Play the quitting sound
    private void PlayQuittingSound() {
        _atm.PlayQuittingSound();
    }

    // Display quitting page with a delay
    private IEnumerator ToggleQuittingPage(float delay) {
        quittingPage.SetActive(true);
        yield return new WaitForSeconds(delay);
        quittingPage.SetActive(false);

        _atm.AccountQuit();
    }

    private void Start() {
        _atm = atmTerminal.GetComponent<ATM>();

        // Add listeners to the buttons
        checkButton.onClick.AddListener(OnCheckButtonClick);
        checkCloseButton.onClick.AddListener(OnCheckCloseButtonClick);
        transferButton.onClick.AddListener(OnTransferButtonClick);
        withdrawButton.onClick.AddListener(OnWithdrawButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
        recordButton.onClick.AddListener(OnRecordButtonClick);
    }
}