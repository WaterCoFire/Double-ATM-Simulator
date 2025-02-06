// // Manages the transaction records
//
// public class Record {
//     // Constants to represent transaction types
//     public const int Withdraw = 0;
//     public const int TransferToOthers = 1;
//     public const int ReceiveTransfer = 2;
//
//     // Private fields to store record details
//     private int _type; // 0-withdraw 1-transfer to others 2-receive transfer
//     private int _relatedAccount;
//     private int _amount;
//
//     // Constructor for withdraw records
//     public Record(int amount) {
//         _type = Withdraw;
//         _amount = amount;
//     }
//
//     // Constructor for transfer-related records
//     public Record(int type, int relatedAccount, int amount) {
//         _type = type;
//         _relatedAccount = relatedAccount;
//         _amount = amount;
//     }
//
//     // Properties to access record details
//     public int Type {
//         get { return _type; }
//     }
//
//     public int Amount {
//         get { return _amount; }
//     }
//
//     public int RelatedAccount {
//         get { return _relatedAccount; }
//     }
// }

// Manages the transaction records
public class Record {
    private int type; // 0-withdraw 1-transfer to others 2-receive transfer
    private int relatedAccount;
    private int amount;

    public Record(int amount) {
        // if with only an amount, this is considered a withdraw so there is not related account
        type = 0;
        this.amount = amount;
    }

    public Record(int type, int relatedAccount, int amount) {
        // transfer-related
        this.type = type;
        this.relatedAccount = relatedAccount;
        this.amount = amount;
    }

    // Getters for attributes
    public int getType() {
        return type;
    }

    public int getAmount() {
        return amount;
    }

    public int getRelatedAccount() {
        return relatedAccount;
    }
}