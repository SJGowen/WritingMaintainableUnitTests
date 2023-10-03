using System;

namespace WritingMaintainableUnitTests.Module4DecouplingPatterns.Expenses
{
    public interface ICanApproveExpenses
    {
        bool CanApprove(decimal amountToApprove);
    }

    public class HeadOfDepartment : ICanApproveExpenses
    {
        private const decimal MaximumAmountToApprove = 1000m;

        public Guid Id { get; }

        public HeadOfDepartment(Guid id)
        {
            Id = id;
        }

        public bool CanApprove(decimal amountToApprove)
        {
            return amountToApprove <= MaximumAmountToApprove;
        }
    }

    public class ChiefFinancialOfficer : ICanApproveExpenses
    {
        public Guid Id { get; }

        public ChiefFinancialOfficer(Guid id)
        {
            Id = id;
        }

        public bool CanApprove(decimal amountToApprove)
        {
            return true;
        }
    }
}