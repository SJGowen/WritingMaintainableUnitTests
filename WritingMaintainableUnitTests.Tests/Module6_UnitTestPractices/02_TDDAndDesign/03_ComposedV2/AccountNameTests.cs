using System;
using WritingMaintainableUnitTests.Module6_UnitTestPractices.BankingComposition;
using WritingMaintainableUnitTests.Tests.Common;

// ReSharper disable InconsistentNaming

namespace WritingMaintainableUnitTests.Tests.Module6_UnitTestPractices._02_TDDAndDesign._03_ComposedV2
{
    [Specification]
    public class When_creating_a_new_account_name
    {
        [Because]
        public void Of()
        {
            _result = AccountName.CreateFor("New account name");
        }

        [Observation]
        public void Then_a_new_account_name_should_be_initialised_to_the_specified_value()
        {
            _result.Value.Should_be_equal_to("New account name");    
        }

        private AccountName _result;
    }
    
    [Specification]
    public class When_creating_a_new_account_name_that_violates_the_minimum_length_restriction
    {
        [Because]
        public void Of()
        {
            _createAccountName = () => AccountName.CreateFor("New");
        }
        
        [Observation]
        public void Then_creating_a_new_account_name_should_not_be_allowed()
        {
            _createAccountName.Should_throw_an<ArgumentException>();
        }

        private Action _createAccountName;
    }
}