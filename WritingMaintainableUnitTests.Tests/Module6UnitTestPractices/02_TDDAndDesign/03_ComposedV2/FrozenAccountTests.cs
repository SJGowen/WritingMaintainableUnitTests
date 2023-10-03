using System;
using WritingMaintainableUnitTests.Module6UnitTestPractices.BankingComposition;
using WritingMaintainableUnitTests.Tests.Common;
using WritingMaintainableUnitTests.Tests.Module6UnitTestPractices.TestDataBuilders.BankingComposition;

namespace WritingMaintainableUnitTests.Tests.Module6UnitTestPractices._02_TDDAndDesign._03_ComposedV2
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
            var expectedAccountName = AccountName.CreateFor("New frozen account name");
            _sut.AccountName.Should_be_equal_to(expectedAccountName);
        }

        private FrozenAccount _sut;
    }
}