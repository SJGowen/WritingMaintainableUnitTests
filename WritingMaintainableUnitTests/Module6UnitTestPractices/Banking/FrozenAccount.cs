using System;

namespace WritingMaintainableUnitTests.Module6UnitTestPractices.Banking;

public class FrozenAccount : Account
{
    public double Balance { get; }
    public Guid ClientId { get; }

    internal FrozenAccount(Guid clientId, string accountName, double balance)
        : base(accountName)
    {
        ClientId = clientId;
        Balance = balance;
    }

    public ActiveAccount Reactivate()
    {
        return new ActiveAccount(ClientId, AccountName, Balance);
    }
}