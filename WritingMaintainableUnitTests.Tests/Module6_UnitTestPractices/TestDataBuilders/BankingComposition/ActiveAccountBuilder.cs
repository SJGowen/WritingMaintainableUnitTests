using System;
using WritingMaintainableUnitTests.Module6_UnitTestPractices.BankingComposition;

namespace WritingMaintainableUnitTests.Tests.Module6_UnitTestPractices.TestDataBuilders.BankingComposition
{
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
            var accountName = AccountName.CreateFor(_accountName);
            return new ActiveAccount(_clientId, accountName, _balance);
        }
        
        public static implicit operator ActiveAccount(ActiveAccountBuilder builder)
        {
            return builder.Build();
        }
    }
}