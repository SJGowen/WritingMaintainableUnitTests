using System;
using System.Collections.Generic;
using System.Linq;
using WritingMaintainableUnitTests.Module4_DecouplingPatterns.Expenses;

namespace WritingMaintainableUnitTests.Module5_AssertionsAndObservations
{
    public class ExpenseSheetViewModel
    {
        public string EmployeeName { get; set; }
        public IEnumerable<ExpenseModel> Expenses { get; set; }
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTime SubmissionDate { get; set; }

        public ExpenseSheetViewModel()
        {
            Expenses = Enumerable.Empty<ExpenseModel>();
        }

        #region Equals/GetHashCode/ToString

        public override bool Equals(object obj)
        {
            var other = (ExpenseSheetViewModel) obj;
            if(null == obj)
                return false;

            return
                EmployeeName == other.EmployeeName &&
                Expenses.SequenceEqual(other.Expenses) &&
                Id == other.Id &&
                Status == other.Status &&
                SubmissionDate == other.SubmissionDate;
        }

        public override int GetHashCode()
        {
            var hashCode = 51 ^
                EmployeeName.GetHashCode() ^
                Id.GetHashCode() ^
                Status.GetHashCode() ^
                SubmissionDate.GetHashCode();

            return Expenses.Aggregate(hashCode, (result, expenseModel) =>
            {
                result = result ^ expenseModel.GetHashCode();
                return result;
            });
        }

        // Adding this implementation provides more diagnostic information when a unit test fails
        /*
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append($"Employee name: {EmployeeName}{Environment.NewLine}");
            builder.Append($"Id: {Id}{Environment.NewLine}");
            builder.Append($"Status: {Status}{Environment.NewLine}");
            builder.Append($"Submission date: {SubmissionDate}{Environment.NewLine}");

            builder.Append($"Expenses: [{Environment.NewLine}");
            Expenses.ToList().ForEach(expense => builder.Append($"\t {expense.ToString()}"));
            builder.Append($"{Environment.NewLine}]");

            return builder.ToString();
        }
        */

        #endregion
    }

    public class ExpenseModel
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        
        #region Equals/GetHashCode/ToString

        public override bool Equals(object obj)
        {
            var other = obj as ExpenseModel;
            if(null == other)
                return false;

            return 
                Amount == other.Amount &&
                Date == other.Date &&
                Description == other.Description;
        }

        public override int GetHashCode()
        {
            return 36 ^
                Amount.GetHashCode() ^
                Date.GetHashCode() ^
                Description.GetHashCode();
        }

        // Adding this implementation provides more diagnostic information when a unit test fails
        /*
        public override string ToString()
        {
            return $"Amount: {Amount}, Date: {Date}, Description: {Description}";
        }
        */

        #endregion
    }
    
    public class ExpenseSheetViewModelMapper
    {
        public ExpenseSheetViewModel MapFrom(ExpenseSheet expenseSheet, Employee employee)
        {
            var employeeName = $"{employee.FirstName} {employee.LastName}";
            var expenses = expenseSheet.Expenses.Select(MapExpense);
            
            return new ExpenseSheetViewModel
            {
                EmployeeName = employeeName,
                Expenses = expenses,
                Id = expenseSheet.Id,
                Status = expenseSheet.Status.ToString(),
                SubmissionDate = expenseSheet.SubmissionDate
            };
        }

        private static ExpenseModel MapExpense(Expense expense)
        {
            return new ExpenseModel
            {
                Amount = expense.Amount,
                Date = expense.Date,
                Description = expense.Description
            };
        }
    }
}