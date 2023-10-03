using System;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module4DecouplingPatterns.Expenses;
using WritingMaintainableUnitTests.Tests.Common;
using WritingMaintainableUnitTests.Tests.Module4DecouplingPatterns._03_TestDataBuilder;

namespace WritingMaintainableUnitTests.Tests.Module4DecouplingPatterns._06_TestDoubles
{
    [Specification]
    public class When_creating_a_new_expense_sheet__spy_example
    {
        [Establish]
        public void Context()
        {
            var employee = Example.Employee().WithId(new Guid("680D0C0A-E445-4344-B67A-363589E2746A"));
            
            var stubEmployeeRepository = new StubEmployeeRepository(employee);
            _spyExpenseSheetRepository = new SpyExpenseSheetRepository(null);
            
            _sut = new CreateExpenseSheetHandler(stubEmployeeRepository, _spyExpenseSheetRepository);
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
            var savedExpenseSheet = _spyExpenseSheetRepository.SavedExpenseSheet;
            
            Assert.That(savedExpenseSheet, Is.Not.Null);
            Assert.That(savedExpenseSheet.Id, Is.EqualTo(new Guid("4048A482-1CBA-45AA-8709-6409B9FD32E3")));
            Assert.That(savedExpenseSheet.EmployeeId, Is.EqualTo(new Guid("680D0C0A-E445-4344-B67A-363589E2746A")));
            Assert.That(savedExpenseSheet.SubmissionDate, Is.EqualTo(new DateTime(2018, 11, 11)));
        }

        [Observation]
        public void Then_the_operation_should_succeed()
        {
            Assert.That(_result.IsSuccessful);
        }

        private SpyExpenseSheetRepository _spyExpenseSheetRepository;
        private CreateExpenseSheetHandler _sut;
        private Result _result;
    }
    
    public class SpyExpenseSheetRepository : IExpenseSheetRepository
    {
        private readonly ExpenseSheet _result;
        
        public ExpenseSheet SavedExpenseSheet { get; private set; }
        
        public SpyExpenseSheetRepository(ExpenseSheet result)
        {
            _result = result;
        }
        
        public ExpenseSheet Get(Guid id)
        {
            return _result;
        }

        public void Save(ExpenseSheet expenseSheet)
        {
            SavedExpenseSheet = expenseSheet;
        }
    }
}