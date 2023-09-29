using System;
using WritingMaintainableUnitTests.Module6_UnitTestPractices.Banking;
using WritingMaintainableUnitTests.Tests.Common;
using WritingMaintainableUnitTests.Tests.Module6_UnitTestPractices.TestDataBuilders.Banking;

// ReSharper disable InconsistentNaming

namespace WritingMaintainableUnitTests.Tests.Module6_UnitTestPractices._02_TDDAndDesign._01_Inheritance
{
    [Specification]
    public class When_reactivating_a_frozen_account
    {
        [Establish]
        public void Context()
        {
            _sut = Example.FrozenAccount()
                .WithAccountName("My account name")
                .WithBalance(1000)
                .WithClientId(new Guid("9A329CA6-1889-4049-BD37-250200F10D9D"));
        }
        
        [Because]
        public void Of()
        {
            _result = _sut.Reactivate();
        }
        
        [Observation]
        public void Then_the_account_should_be_active()
        {
            _result.Should_be_an_instance_of<ActiveAccount>();
        }
        
        [Observation]
        public void Then_the_details_of_the_frozen_account_should_be_transferred_to_the_active_account()
        {
            var expectedActiveAccount = Example.ActiveAccount()
                .WithAccountName("My account name")
                .WithBalance(1000)
                .WithClientId(new Guid("9A329CA6-1889-4049-BD37-250200F10D9D"));
            
            _result.Should_be_deep_equal_to(expectedActiveAccount);
        }

        private FrozenAccount _sut;
        private ActiveAccount _result;
    }
    
    [Specification]
    public class When_changing_the_name_of_a_frozen_account
    {
        [Establish]
        public void Context()
        {
            _sut = Example.FrozenAccount();
        }

        [Because]
        public void Of()
        { 
            _sut.ChangeAccountName("New frozen account name");
        }

        [Observation]
        public void Then_the_account_name_should_be_changed()
        { 
            _sut.AccountName.Should_be_equal_to("New frozen account name");
        }

        private FrozenAccount _sut;
    }

    [Specification]
    public class When_changing_the_name_of_a_frozen_account_to_a_name_that_violates_the_minimum_length_restriction
    {
        [Establish]
        public void Context()
        {
            _sut = Example.FrozenAccount();
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

        private FrozenAccount _sut;
        private Action _changeAccountName;
    }
}