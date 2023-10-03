using System;
using DeepEqual.Syntax;
using WritingMaintainableUnitTests.Module4DecouplingPatterns.Expenses;
using WritingMaintainableUnitTests.Module5AssertionsAndObservations;
using WritingMaintainableUnitTests.Tests.Common;
using WritingMaintainableUnitTests.Tests.Module4DecouplingPatterns._03_TestDataBuilder;

namespace WritingMaintainableUnitTests.Tests.Module5AssertionsAndObservations._04_ObjectStateVerification._04_DeepEqual
{
    [Specification]
    public class When_mapping_a_view_model_from_an_expense_sheet
    {
        [Establish]
        public void Context()
        {
            _expenseSheet = Example.ExpenseSheet()
                .WithId(new Guid("407A310F-50E0-43B2-AD27-D6E1E6D5D291"))
                .WithSubmissionDate(new DateTime(2019, 02, 20))
                .WithExpense(62, new DateTime(2019, 02, 06), "Fancy ice-tea");

            _employee = Example.Employee()
                .WithFirstName("Jon")
                .WithLastName("Snow");
            
            _sut = new ExpenseSheetViewModelMapper();
        }

        [Because]
        public void Of()
        {
            _viewModel = _sut.MapFrom(_expenseSheet, _employee);
        }
        
        [Observation]
        public void Then_all_the_data_should_be_correctly_mapped()
        {
            var expectedViewModel = new ExpenseSheetViewModel
            {
                EmployeeName = "Jon Snow",
                Expenses = new[]
                {
                    new ExpenseModel
                    {
                        Amount = 62,
                        Date = new DateTime(2019, 02, 06),
                        Description = "Fancy ice-tea"
                    }
                },
                Id = new Guid("407A310F-50E0-43B2-AD27-D6E1E6D5D291"),
                Status = "Requested",
                SubmissionDate = new DateTime(2019, 02, 20)
            };
 
            _viewModel.ShouldDeepEqual(expectedViewModel);
        }
        
        private Employee _employee;
        private ExpenseSheet _expenseSheet;
        private ExpenseSheetViewModelMapper _sut;
        private ExpenseSheetViewModel _viewModel;
    }
}