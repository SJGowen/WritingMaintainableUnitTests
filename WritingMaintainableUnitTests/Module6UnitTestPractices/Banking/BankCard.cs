using System;

namespace WritingMaintainableUnitTests.Module6UnitTestPractices.Banking
{
    public class BankCard
    {
        public bool Blocked { get; private set; }

        internal BankCard(bool blocked)
        {
            Blocked = blocked;
        }

        public static BankCard IssueNewBankCard()
        {
            return new BankCard(false);
        }

        public void ReportStolen()
        {
            Blocked = true;
        }

        public void Expire()
        {
            Blocked = true;
        }

        public void MakePayment(ActiveAccount fromAccount, ActiveAccount toAccount, double amount)
        {
            if (Blocked)
                throw new InvalidOperationException("Making payment is not allowed.");

            fromAccount.Withdraw(amount);
            toAccount.Deposit(amount);
        }
    }
}