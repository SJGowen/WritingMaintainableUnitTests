using System;

namespace WritingMaintainableUnitTests.Module6UnitTestPractices.BankingComposition
{
    public readonly struct AccountName
    {
        public string Value { get; }

        private AccountName(string value)
        {
            Value = value;
        }

        public static AccountName CreateFor(string accountName)
        {
            if (accountName.Length < 4)
                throw new ArgumentException("Incorrect length for account name.");

            return new AccountName(accountName);
        }
    }
}