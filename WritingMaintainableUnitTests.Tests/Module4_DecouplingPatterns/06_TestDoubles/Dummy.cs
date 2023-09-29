using System;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module4_DecouplingPatterns.Expenses;
using WritingMaintainableUnitTests.Tests.Common;
// ReSharper disable InconsistentNaming

namespace WritingMaintainableUnitTests.Tests.Module4_DecouplingPatterns._06_TestDoubles
{
    [Specification]
    public class When_creating_a_new_expense_sheet_using_a_non_existent_command
    {
        [Establish]
        public void Context()
        {
            var dummyEmployeeRepository = new DummyEmployeeRepository();
            var dummyExpenseSheetRepository = new DummyExpenseSheetRepository();
            
            _sut = new CreateExpenseSheetHandler(dummyEmployeeRepository, dummyExpenseSheetRepository);
        }

        [Because]
        public void Of()
        {
            _createExpenseSheet = () => _sut.Handle(null);
        }
        
        [Observation]
        public void Then_an_exception_should_be_thrown()
        {
            Assert.That(_createExpenseSheet, Throws.ArgumentNullException);
        }
        
        private CreateExpenseSheetHandler _sut;
        private TestDelegate _createExpenseSheet;
    }

    public class DummyEmployeeRepository : IEmployeeRepository
    {
        public Employee Get(Guid id)
        {
            throw new NotImplementedException("The Get method of the EmployeeRepository shouldn't get called.");
        }
    }

    public class DummyExpenseSheetRepository : IExpenseSheetRepository
    {
        public ExpenseSheet Get(Guid id)
        {
            throw new NotImplementedException("The Get method of the ExpenseSheetRepository shouldn't get called.");
        }

        public void Save(ExpenseSheet expenseSheet)
        {
            throw new NotImplementedException("The Save method of the ExpenseSheetRepository shouldn't get called.");
        }
    }
}