using System;
using WritingMaintainableUnitTests.Module4DecouplingPatterns.Expenses;
using WritingMaintainableUnitTests.Module5AssertionsAndObservations;
using WritingMaintainableUnitTests.Tests.Common;
using WritingMaintainableUnitTests.Tests.Module4DecouplingPatterns._03_TestDataBuilder;

namespace WritingMaintainableUnitTests.Tests.Module5AssertionsAndObservations._03_SingleAssertPerTest;

[Specification]
public class When_mapping_a_view_model_from_an_expense_sheet
{
    [Establish]
    public void Context()
    {
        _expenseSheet = Example.ExpenseSheet()
            .WithId(new Guid("407A310F-50E0-43B2-AD27-D6E1E6D5D291"))
            .WithSubmissionDate(new DateTime(2019, 02, 20))
            .WithExpense(62, new DateTime(2019, 02,06), "Fancy ice-tea");

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
    public void Then_the_name_of_employee_who_submitted_the_expense_sheet_should_be_mapped()
    {
        _viewModel.EmployeeName.Should_be_equal_to("Jon Snow");
    }
    
    [Observation]
    public void Then_the_identifier_of_the_expense_sheet_should_be_mapped()
    {
        _viewModel.Id.Should_be_equal_to(new Guid("407A310F-50E0-43B2-AD27-D6E1E6D5D291"));
    }
    
    [Observation]
    public void Then_the_status_of_the_expense_sheet_should_be_mapped()
    {
        _viewModel.Status.Should_be_equal_to("Requested");
    }
    
    [Observation]
    public void Then_the_submission_date_of_the_expense_sheet_should_be_mapped()
    {
        _viewModel.SubmissionDate.Should_be_equal_to(new DateTime(2019, 02, 20));
    }
    
    [Observation]
    public void Then_the_expenses_of_the_expense_sheet_should_be_mapped()
    {
        _viewModel.Expenses.Should_contain(expense => 
            expense.Amount == 62 && 
            expense.Date == new DateTime(2019, 02, 06) && 
            expense.Description == "Fancy ice-tea");
    }
    
    private Employee _employee;
    private ExpenseSheet _expenseSheet;
    private ExpenseSheetViewModelMapper _sut;
    private ExpenseSheetViewModel _viewModel;
}