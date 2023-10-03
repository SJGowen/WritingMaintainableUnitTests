using System;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module4DecouplingPatterns.Expenses;
using WritingMaintainableUnitTests.Tests.Common;

namespace WritingMaintainableUnitTests.Tests.Module4DecouplingPatterns._02_ObjectMother
{
    [Specification]
    public class When_creating_a_new_expense_sheet
    {
        [Because]
        public void Of()
        {
            _sut = ExpenseSheetExamples.ExpenseSheetWithoutExpenses();
        }
        
        [Observation]
        public void Then_it_should_have_an_identifier()
        {
            Assert.That(_sut.Id, Is.EqualTo(new Guid("F0A6C1DD-AB38-4625-BFE4-1E8A7717CDDD")));
        }
        
        [Observation]
        public void Then_it_should_be_created_for_particular_employee()
        {
            Assert.That(_sut.EmployeeId, Is.EqualTo(new Guid("9822E457-D608-4307-AB6E-C253466F57CD")));
        }
        
        [Observation]
        public void Then_it_should_have_the_date_of_submission()
        {
            Assert.That(_sut.SubmissionDate, Is.EqualTo(new DateTime(2018, 10, 31)));
        }
        
        [Observation]
        public void Then_it_should_have_the_status_requested()
        {
            Assert.That(_sut.Status, Is.EqualTo(ExpenseSheetStatus.Requested));
        }

        private ExpenseSheet _sut;
    }
    
    [Specification]
    public class When_calculating_the_total_of_an_expense_sheet
    {
        [Establish]
        public void Context()
        {
            _sut = ExpenseSheetExamples.ExpenseSheetWithTwoSmallExpenses();
        }
        
        [Because]
        public void Of()
        {
            _result = _sut.CalculateTotal();
        }

        [Observation]
        public void Then_the_total_amount_of_all_expenses_on_the_sheet_should_be_returned()
        {
            Assert.That(_result, Is.EqualTo(79.56m));
        }
        
        private decimal _result;
        private ExpenseSheet _sut;
    }

    [Specification]
    public class When_the_head_of_the_department_approves_an_expense_sheet_with_a_total_amount_that_is_allowed
    {
        [Establish]
        public void Context()
        {
            _approver = ApproverExamples.HeadOfDepartment();
            _sut = ExpenseSheetExamples.ExpenseSheetWithASingleModerateExpense();
        }

        [Because]
        public void Of()
        {
            _sut.Approve(_approver);
        }

        [Observation]
        public void Then_the_status_should_transition_to_approved()
        {
            Assert.That(_sut.Status, Is.EqualTo(ExpenseSheetStatus.Approved));    
        }

        private HeadOfDepartment _approver;
        private ExpenseSheet _sut;
    }
    
    [Specification]
    public class When_the_head_of_the_department_approves_an_expense_sheet_with_a_total_amount_that_is_higher_than_allowed
    {
        [Establish]
        public void Context()
        {
            _approver = ApproverExamples.HeadOfDepartment();
            _sut = ExpenseSheetExamples.ExpenseSheetWithASingleLargeExpense();
        }
    
        [Because]
        public void Of()
        {
            _sut.Approve(_approver);
        }
    
        [Observation]
        public void Then_this_should_result_in_a_domain_violation()
        {
            Assert.That(_sut.Violations, Contains.Item(ExpenseSheetViolations.NotAllowedToApprove(5684.24m)));
        }
        
        [Observation]
        public void Then_the_status_should_NOT_transition()
        {
            Assert.That(_sut.Status, Is.EqualTo(ExpenseSheetStatus.Requested));    
        }
    
        private HeadOfDepartment _approver;
        private ExpenseSheet _sut;
    }
    
    [Specification]
    public class When_the_chief_financial_officer_approves_an_expense_sheet_with_a_large_total_amount
    {
        [Establish]
        public void Context()
        {
            _approver = ApproverExamples.ChiefFinancialOfficer();
            _sut = ExpenseSheetExamples.ExpenseSheetWithASingleLargeExpense();
        }
    
        [Because]
        public void Of()
        {
            _sut.Approve(_approver);
        }
    
        [Observation]
        public void Then_the_status_should_transition_to_approved()
        {
            Assert.That(_sut.Status, Is.EqualTo(ExpenseSheetStatus.Approved));    
        }
    
        private ChiefFinancialOfficer _approver;
        private ExpenseSheet _sut;
    }

    #region Object mothers

    public static class ExpenseSheetExamples
    {
        public static ExpenseSheet ExpenseSheetWithoutExpenses()
        {
            var employee = EmployeeExamples.AverageEmployee();
            
            var expenseSheet = new ExpenseSheet(
                new Guid("F0A6C1DD-AB38-4625-BFE4-1E8A7717CDDD"), 
                employee, 
                new DateTime(2018, 10, 31));
            
            return expenseSheet;
        }
        
        public static ExpenseSheet ExpenseSheetWithTwoSmallExpenses()
        {
            var employee = EmployeeExamples.AverageEmployee();
            
            var expenseSheet = new ExpenseSheet(Guid.NewGuid(), employee, new DateTime(2018, 10, 31));
            expenseSheet.AddExpense(66.57m, new DateTime(2018, 10, 01), "Lunch at Giovanni's");
            expenseSheet.AddExpense(12.99m, new DateTime(2018, 10, 05), "Awesome paperclips");

            return expenseSheet;
        }
        
        public static ExpenseSheet ExpenseSheetWithASingleModerateExpense()
        {
            var employee = EmployeeExamples.AverageEmployee();

            var expenseSheet = new ExpenseSheet(Guid.NewGuid(), employee, new DateTime(2018, 10, 31));
            expenseSheet.AddExpense(123.88m, new DateTime(2018, 10, 12), "Plane ticket to and from Berlin");

            return expenseSheet;
        }
        
        public static ExpenseSheet ExpenseSheetWithASingleLargeExpense()
        {
            var employee = EmployeeExamples.AverageEmployee();

            var expenseSheet = new ExpenseSheet(Guid.NewGuid(), employee, new DateTime(2018, 10, 31));
            expenseSheet.AddExpense(5684.24m, new DateTime(2018, 10, 24), "Exuberant party");

            return expenseSheet;
        }
    }

    public static class EmployeeExamples
    {
        public static Employee AverageEmployee()
        {
            var address = new Address("Spooner Street", "31", "2060ABC", "Quahog");
            var bankInformation = new BankInformation("ING", "BE68844010370034");
            return new Employee(new Guid("9822E457-D608-4307-AB6E-C253466F57CD"), "Peter", "Griffin", address, bankInformation);   
        }
    }

    public static class ApproverExamples
    {
        public static HeadOfDepartment HeadOfDepartment()
        {
            return new HeadOfDepartment(Guid.NewGuid());
        }

        public static ChiefFinancialOfficer ChiefFinancialOfficer()
        {
            return new ChiefFinancialOfficer(Guid.NewGuid());
        }
    }

    #endregion
}