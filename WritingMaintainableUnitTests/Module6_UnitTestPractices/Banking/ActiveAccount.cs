using System;

namespace WritingMaintainableUnitTests.Module6_UnitTestPractices.Banking
{
    public class ActiveAccount : Account
    { 
        public double Balance { get; private set; }
        public Guid ClientId { get; }
        
        internal ActiveAccount(Guid clientId, string accountName, double balance)
            : base(accountName)
        {
            Balance = balance;
            ClientId = clientId;
        }

        public static ActiveAccount OpenNewAccount(Guid clientId, string accountName)
        {
            return new ActiveAccount(clientId, accountName, 0);
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
    }
}