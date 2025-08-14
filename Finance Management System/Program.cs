﻿using System;
using System.Collections.Generic;

public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

public interface ITransactionProcessor
{
    void Process(Transaction transaction);
}

public class BankTransferProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"Bank Transfer: Processing ${transaction.Amount} for {transaction.Category}");
    }
}

public class MobileMoneyProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"Mobile Money: Processing ${transaction.Amount} for {transaction.Category}");
    }
}

public class CryptoWalletProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"Crypto Wallet: Processing ${transaction.Amount} for {transaction.Category}");
    }
}

public class Account
{
    public string AccountNumber { get; }
    public decimal Balance { get; protected set; }

    public Account(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }

    public virtual void ApplyTransaction(Transaction transaction)
    {
        Balance -= transaction.Amount;
    }
}

public sealed class SavingsAccount : Account
{
    public SavingsAccount(string accountNumber, decimal initialBalance) 
        : base(accountNumber, initialBalance)
    {
    }

    public override void ApplyTransaction(Transaction transaction)
    {
        if (transaction.Amount > Balance)
        {
            Console.WriteLine("Insufficient funds");
        }
        else
        {
            Balance -= transaction.Amount;
            Console.WriteLine($"Transaction applied. Updated balance: ${Balance}");
        }
    }
}

public class FinanceApp
{
    private List<Transaction> _transactions = new List<Transaction>();

    public void Run()
    {
        var savingsAccount = new SavingsAccount("SAV001", 1000m);

        var transaction1 = new Transaction(1, DateTime.Now, 150m, "Groceries");
        var transaction2 = new Transaction(2, DateTime.Now, 200m, "Utilities");
        var transaction3 = new Transaction(3, DateTime.Now, 75m, "Entertainment");

        var mobileProcessor = new MobileMoneyProcessor();
        var bankProcessor = new BankTransferProcessor();
        var cryptoProcessor = new CryptoWalletProcessor();

        mobileProcessor.Process(transaction1);
        savingsAccount.ApplyTransaction(transaction1);
        _transactions.Add(transaction1);

        bankProcessor.Process(transaction2);
        savingsAccount.ApplyTransaction(transaction2);
        _transactions.Add(transaction2);

        cryptoProcessor.Process(transaction3);
        savingsAccount.ApplyTransaction(transaction3);
        _transactions.Add(transaction3);

        Console.WriteLine($"\nFinal account balance: ${savingsAccount.Balance}");
        Console.WriteLine($"Total transactions processed: {_transactions.Count}");
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Finance Management System ===");
        var app = new FinanceApp();
        app.Run();
    }
}