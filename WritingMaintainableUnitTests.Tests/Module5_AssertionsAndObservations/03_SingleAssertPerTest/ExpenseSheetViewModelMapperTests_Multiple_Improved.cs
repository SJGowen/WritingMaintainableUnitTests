using System;
using System.Linq;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module5_AssertionsAndObservations;
using WritingMaintainableUnitTests.Tests.Module4_DecouplingPatterns._03_TestDataBuilder;
// ReSharper disable InconsistentNaming

namespace WritingMaintainableUnitTests.Tests.Module5_AssertionsAndObservations._03_SingleAssertPerTest
{
    [TestFixture]
    public class ExpenseSheetViewModelMapperTests_Multiple
    {
        [Test]
        public void Mapping_a_view_model_from_an_expense_sheet()
        {
            var expenseSheet = Example.ExpenseSheet()
                .WithId(new Guid("407A310F-50E0-43B2-AD27-D6E1E6D5D291"))
                .WithSubmissionDate(new DateTime(2019, 02, 20))
                .WithExpense(62, new DateTime(2019, 02, 06), "Fancy ice-tea");

            var employee = Example.Employee()
                .WithFirstName("Jon")
                .WithLastName("Snow");
            
            var sut = new ExpenseSheetViewModelMapper();
            var viewModel = sut.MapFrom(expenseSheet, employee);
            
            Assert.Multiple(() =>
            {
                Assert.That(viewModel.EmployeeName, Is.EqualTo("Jon Snow"));
                Assert.That(viewModel.Id, Is.EqualTo(new Guid("407A310F-50E0-43B2-AD27-D6E1E6D5D291")));
                Assert.That(viewModel.Status, Is.EqualTo("Requested"));
                Assert.That(viewModel.SubmissionDate, Is.EqualTo(new DateTime(2019, 02, 20)));
            
                Assert.That(viewModel.Expenses.Any(expense =>
                    expense.Amount == 62 && 
                    expense.Date == new DateTime(2019, 02, 06) && 
                    expense.Description == "Fancy ice-tea"));    
            });
        }
    }
}