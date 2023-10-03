using System;
using System.Linq;

namespace WritingMaintainableUnitTests.Module4DecouplingPatterns.Expenses
{
    public class ApproveExpenseSheetHandler : ICommandHandler<ApproveExpenseSheet>
    {
        private readonly IExpenseSheetRepository _expenseSheetRepository;
        private readonly IApproverRepository _approverRepository;

        public ApproveExpenseSheetHandler(
            IExpenseSheetRepository expenseSheetRepository,
            IApproverRepository approverRepository)
        {
            _expenseSheetRepository = expenseSheetRepository;
            _approverRepository = approverRepository;
        }

        public Result Handle(ApproveExpenseSheet command)
        {
            var approver = _approverRepository.Get(command.ApproverId);
            if (null == approver)
            {
                var message = $"The approver with identifier '{command.ExpenseSheetId}' could not be found.";
                return Result.Failure(new DomainViolation(message));
            }

            var expenseSheet = _expenseSheetRepository.Get(command.ExpenseSheetId);
            if (null == expenseSheet)
            {
                var message = $"The expense sheet with identifier '{command.ExpenseSheetId}' could not be found.";
                return Result.Failure(new DomainViolation(message));
            }

            expenseSheet.Approve(approver);

            if (expenseSheet.Violations.Any())
                return Result.Failure(expenseSheet.Violations.ToArray());

            _expenseSheetRepository.Save(expenseSheet);
            return Result.Success();
        }
    }

    public class ApproveExpenseSheet
    {
        public Guid ApproverId { get; }
        public Guid ExpenseSheetId { get; }

        public ApproveExpenseSheet(Guid expenseSheetId, Guid approverId)
        {
            ExpenseSheetId = expenseSheetId;
            ApproverId = approverId;
        }
    }
}