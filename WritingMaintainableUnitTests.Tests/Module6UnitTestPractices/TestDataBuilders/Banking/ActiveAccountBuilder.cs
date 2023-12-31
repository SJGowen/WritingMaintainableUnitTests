using System;
using WritingMaintainableUnitTests.Module6UnitTestPractices.Banking;

namespace WritingMaintainableUnitTests.Tests.Module6UnitTestPractices.TestDataBuilders.Banking;

public class ActiveAccountBuilder
{
    private string _accountName;
    private double _balance;
    private Guid _clientId;

    public ActiveAccountBuilder()
    {
        _accountName = "Active account name";
        _balance = 0;
        _clientId = new Guid("0E6401DD-362C-4977-AF44-B24E6E12F259");
    }

    public ActiveAccountBuilder WithAccountName(string accountName)
    {
        _accountName = accountName;
        return this;
    }

    public ActiveAccountBuilder WithBalance(double balance)
    {
        _balance = balance;
        return this;
    }
    
    public ActiveAccountBuilder WithClientId(Guid clientId)
    {
        _clientId = clientId;
        return this;
    }
    
    public ActiveAccount Build()
    {
        return new ActiveAccount(_clientId, _accountName, _balance);
    }
    
    public static implicit operator ActiveAccount(ActiveAccountBuilder builder)
    {
        return builder.Build();
    }
}