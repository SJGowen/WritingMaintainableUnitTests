using System;

namespace WritingMaintainableUnitTests.Module6UnitTestPractices.BankingComposition;

public class FrozenAccount
{
    public AccountName AccountName { get; private set; }
    public double Balance { get; }
    public Guid ClientId { get; }

    internal FrozenAccount(Guid clientId, AccountName accountName, double balance)
    {
        AccountName = accountName;
        ClientId = clientId;
        Balance = balance;
    }

    public ActiveAccount Reactivate()
    {
        return new ActiveAccount(ClientId, AccountName, Balance);
    }

    public void ChangeAccountName(string newAccountName)
    {
        AccountName = AccountName.CreateFor(newAccountName);
    }
}