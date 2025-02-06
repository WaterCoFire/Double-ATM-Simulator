using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

// The class managing accounts
public class Account {
    // the attributes for the account
    private int balance;
    private int pin;
    private int accountNum;
    private readonly List<Record> records = new();

    // ReaderWriterLockSlim for thread safety
    private readonly ReaderWriterLockSlim rwLock = new();

    private const int LockTimeout = 100; // Lock timeout in milliseconds

    // a constructor that takes initial values for each of the attributes (balance, pin, accountNumber)
    public Account(int balance, int pin, int accountNum) {
        this.balance = balance;
        this.pin = pin;
        this.accountNum = accountNum;
    }

    // getter for balance
    public int getBalance() {
        try {
            rwLock.TryEnterReadLock(LockTimeout);
        } catch (LockRecursionException) {
            Debug.LogWarning("Locked!");
            return -1;
        }

        int balanceObtained = balance;
        rwLock.ExitReadLock();
        return balance;
    }

    // Called when withdrawing balances. Returns true if successful, false otherwise.
    public bool withdraw(int amount) {
        if (balance >= amount) {
            balance -= amount;
            Record record = new Record(amount);
            records.Add(record);
            return true;
        }

        return false;
    }

    // Called when transferring balances to others. Returns true if successful, false otherwise.
    public bool transfer(int amount, int otherAccount) {
        if (balance >= amount) {
            balance -= amount;
            Record record = new Record(1, otherAccount, amount);
            records.Add(record);
            return true;
        }

        return false;
    }

    // Called when transferring balances and this account is the receiver.
    public void addBalance(int amount, int otherAccount) {
        balance += amount;
        Record record = new Record(2, otherAccount, amount);
        records.Add(record);
    }

    // Checks the pin of the account. Returns true if correct, false otherwise.
    public bool checkPin(int pinEntered) {
        try {
            rwLock.TryEnterReadLock(LockTimeout);
        } catch (LockRecursionException) {
            Debug.LogWarning("Locked!");
            return false;
        }

        int pinObtained = pin;
        rwLock.ExitReadLock();
        return pinEntered == pinObtained;
    }

    // Get the account num
    public int getAccountNum() {
        return accountNum;
    }

    // Get all the records
    public List<Record> getRecords() {
        try {
            rwLock.TryEnterReadLock(LockTimeout);
        } catch (LockRecursionException) {
            Debug.LogWarning("Locked!");
            return null;
        }
        
        List<Record> recordList = new List<Record>(records);
        rwLock.ExitReadLock();
        return recordList;
    }

    // Get the read/write lock
    public ReaderWriterLockSlim getRWLock() {
        return rwLock;
    }
}