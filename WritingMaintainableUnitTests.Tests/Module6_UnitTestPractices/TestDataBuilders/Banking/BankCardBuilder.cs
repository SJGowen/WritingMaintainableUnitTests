using WritingMaintainableUnitTests.Module6_UnitTestPractices.Banking;

namespace WritingMaintainableUnitTests.Tests.Module6_UnitTestPractices.TestDataBuilders.Banking
{
    public class BankCardBuilder
    {
        private bool _blocked;

        public BankCardBuilder()
        {
            _blocked = false;
        }
        
        public BankCardBuilder AsBlocked()
        {
            _blocked = true;
            return this;
        }
        
        public BankCard Build()
        {
            return new BankCard(_blocked);
        }
        
        public static implicit operator BankCard(BankCardBuilder builder)
        {
            return builder.Build();
        }
    }
}