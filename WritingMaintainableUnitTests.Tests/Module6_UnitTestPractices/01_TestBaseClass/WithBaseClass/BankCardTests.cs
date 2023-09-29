using System;
using WritingMaintainableUnitTests.Module6_UnitTestPractices.Banking;
using WritingMaintainableUnitTests.Tests.Common;
using WritingMaintainableUnitTests.Tests.Module6_UnitTestPractices.TestDataBuilders.Banking;
// ReSharper disable InconsistentNaming

namespace WritingMaintainableUnitTests.Tests.Module6_UnitTestPractices._01_TestBaseClass.WithBaseClass
{
    [Specification]
    public class When_issuing_a_new_bank_card
    {
        [Because]
        public void Of()
        {
            _result = BankCard.IssueNewBankCard();
        }

        [Observation]
        public void Then_the_bank_card_should_be_active()
        {
            _result.Blocked.Should_be_false();
        }

        private BankCard _result;
    }
    
    [Specification]
    public class When_a_bank_card_is_reported_stolen
        : Bank_card_specification
    {
        [Because]
        public void Of()
        {
            SUT.ReportStolen();
        }
        
        [Observation]
        public void Then_the_bank_card_should_be_blocked()
        {
            SUT.Blocked.Should_be_true();
        }
    }
    
    [Specification]
    public class When_a_bank_card_is_expired
        : Bank_card_specification
    {
        [Because]
        public void Of()
        {
            SUT.Expire();
        }
        
        [Observation]
        public void Then_the_bank_card_should_be_blocked()
        {
            SUT.Blocked.Should_be_true();
        }
    }
    
    [Specification]
    public class When_making_a_payment
        : Bank_card_payment_specification
    {
        [Because]
        public void Of()
        {
            SUT.MakePayment(FromAccount, ToAccount, 354.76);        
        }

        [Observation]
        public void Then_the_specified_amount_should_be_withdrawn_from_the_source_account()
        {
            FromAccount.Balance.Should_be_equal_to(1645.24);
        }
        
        [Observation]
        public void Then_the_specified_amount_should_be_deposited_to_the_target_account()
        {
            ToAccount.Balance.Should_be_equal_to(1354.76);
        }
    }
    
    [Specification]
    public class When_making_a_payment_using_a_blocked_bank_card
        : Bank_card_payment_specification
    {
        [Because]
        public void Of()
        {
            _makePayment = () => SUTBlocked.MakePayment(FromAccount, ToAccount, 162.88);
        }

        [Observation]
        public void Then_the_payment_should_not_be_allowed()
        {
            _makePayment.Should_throw_an<InvalidOperationException>();
        }
        
        private Action _makePayment;
    }

    public abstract class Bank_card_specification
    {
        [Establish]
        public void BaseContext()
        {
            SUT = Example.BankCard();
            SUTBlocked = Example.BankCard().AsBlocked();
        }

        protected BankCard SUT { get; private set; }
        protected BankCard SUTBlocked { get; private set; }
    }
    
    public abstract class Bank_card_payment_specification : Bank_card_specification
    {
        [Establish]
        public void PaymentContext()
        {
            FromAccount = Example.ActiveAccount()
                .WithAccountName("From account")
                .WithBalance(2000);
            
            ToAccount = Example.ActiveAccount()
                .WithAccountName("To account")
                .WithBalance(1000);
        }
        
        protected ActiveAccount FromAccount { get; private set; }
        protected ActiveAccount ToAccount { get; private set; }
    }
}