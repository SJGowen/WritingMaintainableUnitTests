using System;

namespace WritingMaintainableUnitTests.Module6UnitTestPractices.BankingComposition;

public class ActiveAccount
{
    public AccountName AccountName { get; private set; }
    public double Balance { get; private set; }
    public Guid ClientId { get; }

    internal ActiveAccount(Guid clientId, AccountName accountName, double balance)
    {
        AccountName = accountName;
        Balance = balance;
        ClientId = clientId;
    }

    public static ActiveAccount OpenNewAccount(Guid clientId, string accountName)
    {
        var typedAccountName = AccountName.CreateFor(accountName);
        return new ActiveAccount(clientId, typedAccountName, 0);
    }

    public void Deposit(double amount)
    {
        Balance += amount;
    }

    public void Withdraw(double amount)
    {
        Balance -= amount;
    }

    public FrozenAccount Freeze()
    {
        return new FrozenAccount(ClientId, AccountName, Balance);
    }

    public void ChangeAccountName(string newAccountName)
    {
        AccountName = AccountName.CreateFor(newAccountName);
    }
}