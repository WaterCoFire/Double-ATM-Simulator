using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

// Manages each ATM used in this project.
public class ATM : MonoBehaviour {
    public GameObject atmManager;
    public GameObject loginPage;
    public GameObject mainMenuPage;
    public GameObject withdrawPage;
    public GameObject transferPage;
    public GameObject recordPage;

    public GameObject occupiedPage;

    private Account _activeAccount; // Current account
    private AtmDataManager _dataManager;
    private bool _withdrawPageInitialised;
    private const int LockTimeout = 100; // Lock timeout in milliseconds

    // Paths to all the sound effects
    private string _pressSoundPath = "ButtonPress";
    private string _successSoundPath = "Success";
    private string _failureSoundPath = "Failure";
    private string _cashOutSoundPath = "CashOut";
    private string _enterPromptSoundPath = "EnterPrompt";
    private string _quittingSoundPath = "Quitting";

    private bool _inWithdraw;
    private bool _inTransfer;

    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public Button button6;
    public Button button7;
    public Button button8;

    // Get all the accounts
    public Account[] GetAllAccounts() {
        return _dataManager.GetAllAccounts();
    }

    // Initialisation
    private void Start() {
        _dataManager = atmManager.GetComponent<AtmDataManager>();

        loginPage.SetActive(true); // Display Login page only
        loginPage.GetComponent<LoginPage>().requireUpdate = true;
        mainMenuPage.SetActive(false);
    }

    // Log into an account
    public void AccountLogin(Account account) {
        _activeAccount = account; // Set current account
        loginPage.SetActive(false);
        mainMenuPage.SetActive(true);
        mainMenuPage.GetComponent<MainPage>().currentAccount = account.getAccountNum();
        mainMenuPage.GetComponent<MainPage>().requireUpdate = true;
    }

    // Log out
    public void AccountQuit() {
        _activeAccount = null;
        mainMenuPage.SetActive(false);
        loginPage.SetActive(true);

        loginPage.GetComponent<LoginPage>().Refresh();
        loginPage.GetComponent<LoginPage>().requireUpdate = true;
    }

    // Called when the withdraw button is clicked
    public void WithdrawSelected() {
        _inWithdraw = true;
        mainMenuPage.SetActive(false);
        withdrawPage.SetActive(true);
        // Detect if it is the first time it is called; if it is, an initialisation is needed
        if (_withdrawPageInitialised) {
            withdrawPage.GetComponent<WithdrawPage>().UpdateBalance();
        } else {
            _withdrawPageInitialised = true;
        }

        withdrawPage.GetComponent<WithdrawPage>().requireUpdate = true;
    }

    // Called when quitting from the withdraw page to the main page
    public void WithdrawFinished() {
        _inWithdraw = false;
        withdrawPage.SetActive(false);
        mainMenuPage.SetActive(true);

        mainMenuPage.GetComponent<MainPage>().requireUpdate = true;
    }

    // Called when the transfer button is clicked
    public void TransferSelected() {
        _inTransfer = true;
        mainMenuPage.SetActive(false);
        transferPage.SetActive(true);

        transferPage.GetComponent<TransferPage>().Init();
    }

    // Called when quitting from the transfer page to the main page
    public void TransferFinished() {
        _inTransfer = false;
        transferPage.SetActive(false);
        mainMenuPage.SetActive(true);

        mainMenuPage.GetComponent<MainPage>().requireUpdate = true;
    }

    // Called when the record button is clicked
    public void RecordSelected() {
        mainMenuPage.SetActive(false);
        recordPage.SetActive(true);
        recordPage.GetComponent<RecordPage>().Init();
    }

    // Called when quitting from the record page to the main page
    public void RecordFinished() {
        recordPage.SetActive(false);
        mainMenuPage.SetActive(true);

        mainMenuPage.GetComponent<MainPage>().requireUpdate = true;
    }

    // Withdraw balance
    public void Withdraw(int amount, System.Action<bool> callback) {
        // Start the coroutine to operate withdraw
        StartCoroutine(PerformWithdraw(amount, callback));
    }

    // Withdraw operation
    private IEnumerator PerformWithdraw(int amount, System.Action<bool> callback) {
        try {
            _activeAccount.getRWLock().TryEnterWriteLock(LockTimeout);
        } catch (LockRecursionException) {
            Debug.LogWarning("Locked!");
            withdrawPage.GetComponent<WithdrawPage>().occupied = true;
            ShowOccupiedWarning();
            callback(false);
            yield break;
        }

        bool result;
        try {
            result = _activeAccount.withdraw(amount);
            // A fixed delay of 1 sec
            yield return new WaitForSeconds(1.0f);
        } finally {
            _activeAccount.getRWLock().ExitWriteLock();
        }

        if (result) {
            Debug.Log("Withdraw SUCCESS: " + amount);
        } else {
            Debug.Log("Withdraw FAILED: " + amount);
        }

        // Call back the result
        callback(result);
    }

    // Transfer balance to another account
    public void Transfer(Account receiverAccount, int amount, System.Action<bool> callback) {
        // Start the coroutine to operate transfer
        StartCoroutine(PerformTransfer(receiverAccount, amount, callback));
    }

    // Transfer operation
    private IEnumerator PerformTransfer(Account receiverAccount, int amount, System.Action<bool> callback) {
        try {
            _activeAccount.getRWLock().TryEnterWriteLock(LockTimeout);
        } catch (LockRecursionException) {
            Debug.LogWarning("Locked!");
            transferPage.GetComponent<TransferPage>().occupied = true;
            ShowOccupiedWarning();
            callback(false);
            yield break;
        }

        bool result;
        try {
            result = _activeAccount.transfer(amount, receiverAccount.getAccountNum());
            if (result) {
                receiverAccount.addBalance(amount, _activeAccount.getAccountNum());
            }

            // A fixed delay of 1 sec
            yield return new WaitForSeconds(1.0f);
        } finally {
            _activeAccount.getRWLock().ExitWriteLock();
        }

        if (result) {
            Debug.Log("Withdraw SUCCESS: " + amount);
        } else {
            Debug.Log("Withdraw FAILED: " + amount);
        }

        // Call back the result
        callback(result);
    }

    // Get the balance of the current account
    public int ObtainBalance() {
        return _activeAccount.getBalance();
    }

    // Get the account num of the current account
    public Account GetCurrentAccount() {
        return _activeAccount;
    }

    // Play sound effect when button pressed
    public void PlayPressSound() {
        AudioManager.instance.PlayClickSound(_pressSoundPath);
    }

    // Play sound effect when a success operation is made
    public void PlaySuccessSound() {
        AudioManager.instance.PlayClickSound(_successSoundPath);
    }

    // Play sound effect when an error is made
    public void PlayFailureSound() {
        AudioManager.instance.PlayClickSound(_failureSoundPath);
    }

    // Play sound effect when withdrawing cash
    public void PlayCashOutSound() {
        AudioManager.instance.PlayClickSound(_cashOutSoundPath);
    }

    // Play sound effect when entering account & pin
    public void PlayEnterPromptSound() {
        AudioManager.instance.PlayClickSound(_enterPromptSoundPath);
    }

    // Play sound effect when logging out
    public void PlayQuittingSound() {
        AudioManager.instance.PlayClickSound(_quittingSoundPath);
    }

    public void ShowOccupiedWarning() {
        StartCoroutine(ToggleOccupiedWarning(2f));
    }

    private IEnumerator ToggleOccupiedWarning(float delay) {
        occupiedPage.SetActive(true);
        PlayFailureSound();
        yield return new WaitForSeconds(delay);
        occupiedPage.SetActive(false);
        
        SynchroniseData();
    }

    // Update the data
    public void UpdateData() {
        if (_inTransfer) {
            transferPage.GetComponent<TransferPage>().UpdateInfo();
        }

        if (_inWithdraw) {
            withdrawPage.GetComponent<WithdrawPage>().UpdateBalance();
        }
    }

    // Synchronise data among all ATM
    public void SynchroniseData() {
        _dataManager.SynchroniseData();
    }
}