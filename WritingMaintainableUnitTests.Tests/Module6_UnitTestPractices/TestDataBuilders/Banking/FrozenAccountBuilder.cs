using System;
using WritingMaintainableUnitTests.Module6_UnitTestPractices.Banking;

namespace WritingMaintainableUnitTests.Tests.Module6_UnitTestPractices.TestDataBuilders.Banking
{
    public class FrozenAccountBuilder
    {
        private string _accountName;
        private double _balance;
        private Guid _clientId;

        public FrozenAccountBuilder()
        {
            _accountName = "Frozen account name";
            _balance = 0;
            _clientId = new Guid("6D785546-751F-461D-A72A-BB5A148C2437");
        }

        public FrozenAccountBuilder WithAccountName(string accountName)
        {
            _accountName = accountName;
            return this;
        }

        public FrozenAccountBuilder WithBalance(double balance)
        {
            _balance = balance;
            return this;
        }
        
        public FrozenAccountBuilder WithClientId(Guid clientId)
        {
            _clientId = clientId;
            return this;
        }
        
        public FrozenAccount Build()
        {
            return new FrozenAccount(_clientId, _accountName, _balance);
        }
        
        public static implicit operator FrozenAccount(FrozenAccountBuilder builder)
        {
            return builder.Build();
        }
    }
}