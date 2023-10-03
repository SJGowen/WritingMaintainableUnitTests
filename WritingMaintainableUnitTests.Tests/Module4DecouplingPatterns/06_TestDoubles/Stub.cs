using System;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module4DecouplingPatterns.Expenses;
using WritingMaintainableUnitTests.Tests.Common;

namespace WritingMaintainableUnitTests.Tests.Module4DecouplingPatterns._06_TestDoubles
{
    [Specification]
    public class When_creating_a_new_expense_sheet_for_an_unknown_employee 
    {
        [Establish]
        public void Context()
        {
            Employee unknownEmployee = null;
            
            var stubEmployeeRepository = new StubEmployeeRepository(unknownEmployee);
            var dummyExpenseSheetRepository = new DummyExpenseSheetRepository();
            
            _sut = new CreateExpenseSheetHandler(stubEmployeeRepository, dummyExpenseSheetRepository);
        }
        
        [Because]
        public void Of()
        {
            var command = new CreateExpenseSheet(Guid.NewGuid(), Guid.NewGuid(), new DateTime(2018, 11, 11));
            _result = _sut.Handle(command);
        }
        
        [Observation]
        public void Then_the_operation_should_fail()
        {
            Assert.That(_result.IsSuccessful, Is.False);
        }

        private CreateExpenseSheetHandler _sut;
        private Result _result;
    }
   
    public class StubEmployeeRepository : IEmployeeRepository
    {
        private readonly Employee _result;

        public StubEmployeeRepository(Employee result)
        {
            _result = result;
        }
        
        public Employee Get(Guid id)
        {
            return _result;
        }
    }
}