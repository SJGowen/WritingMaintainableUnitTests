using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module4DecouplingPatterns.Expenses;
using WritingMaintainableUnitTests.Module5AssertionsAndObservations;
using WritingMaintainableUnitTests.Tests.Common;
using WritingMaintainableUnitTests.Tests.Module4DecouplingPatterns._03_TestDataBuilder;

namespace WritingMaintainableUnitTests.Tests.Module5AssertionsAndObservations._04_ObjectStateVerification._02_EqualityComparer
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
                
            Assert.That(_viewModel, Is.EqualTo(expectedViewModel).Using(new ExpenseSheetViewModelEqualityComparer()));
        }
        
        private Employee _employee;
        private ExpenseSheet _expenseSheet;
        private ExpenseSheetViewModelMapper _sut;
        private ExpenseSheetViewModel _viewModel;
    }

    #region Equality comparers

    public class ExpenseSheetViewModelEqualityComparer : IEqualityComparer<ExpenseSheetViewModel>
    {
        private readonly ExpenseModelEqualityComparer _expenseModelEqualityComparer;

        public ExpenseSheetViewModelEqualityComparer()
        {
            _expenseModelEqualityComparer = new ExpenseModelEqualityComparer();
        }
        
        public bool Equals(ExpenseSheetViewModel one, ExpenseSheetViewModel other)
        {
            if(ReferenceEquals(one, other)) return true;
            if(null == one) return false;
            if(null == other) return false;
            
            return
                one.EmployeeName == other.EmployeeName &&
                one.Expenses.SequenceEqual(other.Expenses, _expenseModelEqualityComparer) &&
                one.Id == other.Id &&
                one.Status == other.Status &&
                one.SubmissionDate == other.SubmissionDate;
        }

        public int GetHashCode(ExpenseSheetViewModel viewModel)
        {
            var hashCode = 51 ^
                           viewModel.EmployeeName.GetHashCode() ^
                           viewModel.Id.GetHashCode() ^
                           viewModel.Status.GetHashCode() ^
                           viewModel.SubmissionDate.GetHashCode();

            return viewModel.Expenses.Aggregate(hashCode, (result, expenseModel) =>
            {
                result = result ^ _expenseModelEqualityComparer.GetHashCode(expenseModel);
                return result;
            });
        }
    }

    public class ExpenseModelEqualityComparer : IEqualityComparer<ExpenseModel>
    {
        public bool Equals(ExpenseModel one, ExpenseModel other)
        {
            if(ReferenceEquals(one, other)) return true;
            if(null == one) return false;
            if(null == other) return false;
            
            return 
                one.Amount == other.Amount &&
                one.Date == other.Date &&
                one.Description == other.Description; 
        }

        public int GetHashCode(ExpenseModel model)
        {
            return 36 ^
                   model.Amount.GetHashCode() ^
                   model.Date.GetHashCode() ^
                   model.Description.GetHashCode();
        }
    }

    #endregion
}