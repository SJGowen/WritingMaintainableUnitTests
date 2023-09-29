using System;
using System.Collections.Generic;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module4_DecouplingPatterns.Expenses;
using WritingMaintainableUnitTests.Tests.Common;
using WritingMaintainableUnitTests.Tests.Module4_DecouplingPatterns._03_TestDataBuilder;
// ReSharper disable InconsistentNaming

namespace WritingMaintainableUnitTests.Tests.Module4_DecouplingPatterns._06_TestDoubles
{
    [Specification]
    public class When_creating_a_new_expense_sheet__fake_example
    {
        [Establish]
        public void Context()
        {
            var employee = Example.Employee().WithId(new Guid("680D0C0A-E445-4344-B67A-363589E2746A"));
            var stubEmployeeRepository = new StubEmployeeRepository(employee);
                
            _fakeExpenseSheetRepository = new FakeExpenseSheetRepository();            
            _sut = new CreateExpenseSheetHandler(stubEmployeeRepository, _fakeExpenseSheetRepository);
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
        public void Then_the_operation_should_succeed()
        {
            Assert.That(_result.IsSuccessful);
        }

        private FakeExpenseSheetRepository _fakeExpenseSheetRepository;
        private CreateExpenseSheetHandler _sut;
        private Result _result;
    }
    
    public class FakeExpenseSheetRepository : IExpenseSheetRepository
    {
        private readonly Dictionary<Guid, ExpenseSheet> _expenseSheets;
        
        public FakeExpenseSheetRepository()
        {
            _expenseSheets = new Dictionary<Guid, ExpenseSheet>();
        }
        
        public ExpenseSheet Get(Guid id)
        {
            var isFound = _expenseSheets.TryGetValue(id, out var expenseSheet);
            return isFound ? expenseSheet : null;
        }

        public void Save(ExpenseSheet expenseSheet)
        {
            var current = Get(expenseSheet.Id);
            var currentVersion = current?.Version ?? 0;
            
            if(currentVersion != expenseSheet.Version)
                throw new OptimisticConcurrencyException<ExpenseSheet>(expenseSheet.Id, currentVersion, expenseSheet.Version);

            expenseSheet.Version += 1;
            _expenseSheets[expenseSheet.Id] = expenseSheet;
        }
    }
}