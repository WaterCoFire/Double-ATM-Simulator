using System;
using System.Collections.Concurrent;
using UnityEngine;

// Made By Group 11:
// Jieshen Cai 2542682
// Siyu Yan 2543301
// Siqi Peng 2542671

// Manages the initialization of the two ATM as well as all the data (e.g., accounts)
public class AtmDataManager : MonoBehaviour {
    public GameObject atmUIPrefab;

    private Account[] _allAccounts = new Account[3];
    private ConcurrentQueue<Action> _taskQueue = new();

    private ATM _atm1; // Store the first ATM
    private ATM _atm2; // Store the second ATM
    private bool _isMouseOnLeftSide;

    private void Start() {
        // The accounts used
        _allAccounts[0] = new Account(300, 1111, 111111);
        _allAccounts[1] = new Account(750, 2222, 222222);
        _allAccounts[2] = new Account(3000, 3333, 333333);

        // Create two ATMs
        CreateATMWindow(0);
        CreateATMWindow(1);
    }

    private void CreateATMWindow(int index) {
        // Enqueue the task to create the ATM window to be executed in the main thread
        _taskQueue.Enqueue(() => {
            // Create a game object of the set prefab
            GameObject atmInstance = Instantiate(atmUIPrefab);
            atmInstance.transform.SetParent(this.transform, false);

            if (index == 0) _atm1 = atmInstance.GetComponentInChildren<ATM>();
            else _atm2 = atmInstance.GetComponentInChildren<ATM>();

            // Set the position of each ATM's UI window
            atmInstance.transform.position = new Vector3(-1059 + index * 956, -53, 0);
        });
    }

    private void Update() {
        // Process any remaining tasks in the queue
        while (_taskQueue.TryDequeue(out var task)) {
            task.Invoke();
        }

        // Judge the mouse's position on the screen to decide which ATM is currently active
        Vector3 mousePosition = Input.mousePosition;
        float screenWidth = Screen.width;
        _isMouseOnLeftSide = mousePosition.x < screenWidth / 2;

        if (_isMouseOnLeftSide) {
            _atm1.GetComponent<KeyboardInputSound>().isMouseActive = true;
            _atm2.GetComponent<KeyboardInputSound>().isMouseActive = false;
        } else {
            _atm1.GetComponent<KeyboardInputSound>().isMouseActive = false;
            _atm2.GetComponent<KeyboardInputSound>().isMouseActive = true;
        }
    }

    // Obtain all the accounts
    public Account[] GetAllAccounts() {
        return _allAccounts;
    }
    
    // Synchronise data
    public void SynchroniseData() {
        _atm1.UpdateData();
        _atm2.UpdateData();
    }
}