using System;

namespace WritingMaintainableUnitTests.Module6UnitTestPractices.Banking
{
    public abstract class Account
    {
        public string AccountName { get; private set; }

        protected Account(string accountName)
        {
            ChangeAccountName(accountName);
        }

        public void ChangeAccountName(string newAccountName)
        {
            if (newAccountName.Length < 4)
                throw new ArgumentException("Incorrect length for account name.");

            AccountName = newAccountName;
        }
    }
}