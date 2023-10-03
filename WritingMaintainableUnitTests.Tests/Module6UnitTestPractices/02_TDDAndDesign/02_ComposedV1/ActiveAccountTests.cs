using System;
using WritingMaintainableUnitTests.Module6UnitTestPractices.BankingComposition;
using WritingMaintainableUnitTests.Tests.Common;
using WritingMaintainableUnitTests.Tests.Module6UnitTestPractices.TestDataBuilders.BankingComposition;

namespace WritingMaintainableUnitTests.Tests.Module6UnitTestPractices._02_TDDAndDesign._02_ComposedV1
{
    [Specification]
    public class When_opening_a_new_account
    {
        [Because]
        public void Of()
        {
            _result = ActiveAccount.OpenNewAccount(new Guid("6F3B08A4-A9ED-4C24-A2A6-3F932998B297"), "New account name");
        }

        [Observation]
        public void Then_a_new_account_should_be_opened_for_the_specified_client()
        {
            _result.ClientId.Should_be_equal_to(new Guid("6F3B08A4-A9ED-4C24-A2A6-3F932998B297"));
        }
        
        [Observation]
        public void Then_a_new_account_should_be_opened_with_the_specified_account_name()
        {
            var expectedAccountName = AccountName.CreateFor("New account name");
            _result.AccountName.Should_be_equal_to(expectedAccountName);
        }
        
        [Observation]
        public void Then_the_balance_of_the_new_account_should_be_zero()
        {
            _result.Balance.Should_be_equal_to(0);
        }
        
        private ActiveAccount _result;
    }
    
    [Specification]
    public class When_opening_a_new_account_with_a_name_that_violates_the_minimum_length_restriction
    {
        [Because]
        public void Of()
        {
            _openNewAccount = () => ActiveAccount.OpenNewAccount(new Guid("6F3B08A4-A9ED-4C24-A2A6-3F932998B297"), "New");
        }

        [Observation]
        public void Then_opening_a_new_account_should_not_be_allowed()
        {
            _openNewAccount.Should_throw_an<ArgumentException>();
        }
        
        private Action _openNewAccount;
    }
    
    [Specification]
    public class When_making_a_deposit_to_an_active_account
    {
        [Establish]
        public void Context()
        {
            _sut = Example.ActiveAccount()
                .WithBalance(1000);
        }

        [Because]
        public void Of()
        {
            _sut.Deposit(360.48);
        }

        [Observation]
        public void Then_the_balance_should_be_increased_with_the_specified_amount()
        {
            _sut.Balance.Should_be_equal_to(1360.48);
        }

        private ActiveAccount _sut;
    }
    
    [Specification]
    public class When_making_a_withdrawal_from_an_active_account
    {
        [Establish]
        public void Context()
        {
            _sut = Example.ActiveAccount()
                .WithBalance(1000);
        }
        
        [Because]
        public void Of()
        {
            _sut.Withdraw(212.87);            
        }
        
        [Observation]
        public void Then_the_balance_should_be_decreased_with_the_specified_amount()
        {
            _sut.Balance.Should_be_equal_to(787.13);
        }

        private ActiveAccount _sut;
    }
    
    [Specification]
    public class When_freezing_an_active_account
    {
        [Establish]
        public void Context()
        {
            _sut = Example.ActiveAccount()
                .WithAccountName("My account name")
                .WithBalance(1000)
                .WithClientId(new Guid("9A329CA6-1889-4049-BD37-250200F10D9D"));
        }
        
        [Because]
        public void Of()
        {
            _result = _sut.Freeze();
        }
        
        [Observation]
        public void Then_the_account_should_be_frozen()
        {
            _result.Should_be_an_instance_of<FrozenAccount>();
        }
        
        [Observation]
        public void Then_the_details_of_the_active_account_should_be_transferred_to_the_frozen_account()
        {
            var expectedFrozenAccount = Example.FrozenAccount()
                .WithAccountName("My account name")
                .WithBalance(1000)
                .WithClientId(new Guid("9A329CA6-1889-4049-BD37-250200F10D9D"));
            
            _result.Should_be_deep_equal_to(expectedFrozenAccount);
        }

        private ActiveAccount _sut;
        private FrozenAccount _result;
    }
    
    [Specification]
    public class When_changing_the_name_of_an_active_account
    {
        [Establish]
        public void Context()
        {
            _sut = Example.ActiveAccount();
        }
        
        [Because]
        public void Of()
        {
            _sut.ChangeAccountName("New active account name");
        }
        
        [Observation]
        public void Then_the_account_name_should_be_changed()
        {
            var expectedAccountName = AccountName.CreateFor("New active account name");
            _sut.AccountName.Should_be_equal_to(expectedAccountName);
        }

        private ActiveAccount _sut;
    }
    
    [Specification]
    public class When_changing_the_name_of_an_active_account_to_a_name_that_violates_the_minimum_length_restriction
    {
        [Establish]
        public void Context()
        {
            _sut = Example.ActiveAccount();
        }

        [Because]
        public void Of()
        {
            _changeAccountName = () => _sut.ChangeAccountName("New");
        }
        
        [Observation]
        public void Then_changing_the_name_should_not_be_allowed()
        {
            _changeAccountName.Should_throw_an<ArgumentException>();
        }

        private ActiveAccount _sut;
        private Action _changeAccountName;
    }
}