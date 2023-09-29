using System;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module4_DecouplingPatterns.Expenses;
using WritingMaintainableUnitTests.Tests.Common;
using WritingMaintainableUnitTests.Tests.Module4_DecouplingPatterns._03_TestDataBuilder;
// ReSharper disable InconsistentNaming

namespace WritingMaintainableUnitTests.Tests.Module4_DecouplingPatterns._06_TestDoubles
{
    [Specification]
    public class When_creating_a_new_expense_sheet__mock_example
    {
        [Establish]
        public void Context()
        {
            var employee = Example.Employee().WithId(new Guid("680D0C0A-E445-4344-B67A-363589E2746A"));
            var stubEmployeeRepository = new StubEmployeeRepository(employee);

            var expenseSheetToBeSaved = Example.ExpenseSheet()
                .WithId(new Guid("4048A482-1CBA-45AA-8709-6409B9FD32E3"))
                .WithEmployee(employee)
                .WithSubmissionDate(new DateTime(2018, 11, 11));
                
            _mockExpenseSheetRepository = new MockExpenseSheetRepository(null);
            _mockExpenseSheetRepository.ExpectSaveToBeCalled(expenseSheetToBeSaved);
            
            _sut = new CreateExpenseSheetHandler(stubEmployeeRepository, _mockExpenseSheetRepository);
        }

        [Because]
        public void Of()
        {
            var command = new CreateExpenseSheet(
                new Guid("4048A482-1CBA-45AA-8709-6409B9FD32E3"), 
                new Guid("680D0C0A-E445-4344-B67A-363589E2746A"), 
                new DateTime(2018, 11, 11));

            _result = _sut.Handle(command);
        }
        
        [Observation]
        public void Then_the_approved_expense_sheet_should_be_saved()
        {
            _mockExpenseSheetRepository.Verify();
        }

        [Observation]
        public void Then_the_operation_should_succeed()
        {
            Assert.That(_result.IsSuccessful);
        }

        private MockExpenseSheetRepository _mockExpenseSheetRepository;
        private CreateExpenseSheetHandler _sut;
        private Result _result;
    }
    
    public class MockExpenseSheetRepository : IExpenseSheetRepository
    {
        private readonly ExpenseSheet _result;
        private ExpenseSheet _expectedExpenseSheetToBeSaved;
        private ExpenseSheet _savedExpenseSheet;
        
        public MockExpenseSheetRepository(ExpenseSheet result)
        {
            _result = result;
        }
        
        public ExpenseSheet Get(Guid id)
        {
            return _result;
        }

        public void Save(ExpenseSheet expenseSheet)
        {
            _savedExpenseSheet = expenseSheet;
        }

        public void ExpectSaveToBeCalled(ExpenseSheet expenseSheetToBeSaved)
        {
            _expectedExpenseSheetToBeSaved = expenseSheetToBeSaved;
        }
        
        public void Verify()
        {
            if(null == _expectedExpenseSheetToBeSaved)
                return;
            
            Assert.That(_savedExpenseSheet, Is.Not.Null);
            Assert.That(_savedExpenseSheet.Id, Is.EqualTo(_expectedExpenseSheetToBeSaved.Id));
            Assert.That(_savedExpenseSheet.EmployeeId, Is.EqualTo(_expectedExpenseSheetToBeSaved.EmployeeId));
            Assert.That(_savedExpenseSheet.SubmissionDate, Is.EqualTo(_expectedExpenseSheetToBeSaved.SubmissionDate));
        }
    }
}