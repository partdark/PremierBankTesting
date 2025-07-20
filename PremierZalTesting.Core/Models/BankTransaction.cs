using System;

namespace PremierBankTesting.Bank.Models;

public class BankTransaction
{
    public BankTransaction(Guid id, decimal amount, string comment, DateTime timestamp, string userEmail, bool isProcessed = false)
    {
        Id = id;
        Amount = amount;
        Comment = comment;
        Timestamp = timestamp;
        UserEmail = userEmail;
        IsProcessed = isProcessed;

    }

    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string Comment { get; set; }
    public DateTime Timestamp { get; set; }
    public string UserEmail { get; set; }

    public bool IsProcessed { get; set; } = false;
}