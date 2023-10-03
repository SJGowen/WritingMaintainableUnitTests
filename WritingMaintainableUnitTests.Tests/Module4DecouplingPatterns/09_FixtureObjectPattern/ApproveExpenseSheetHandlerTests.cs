using System;
using NSubstitute;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module4DecouplingPatterns.Expenses;
using WritingMaintainableUnitTests.Tests.Common;
using WritingMaintainableUnitTests.Tests.Module4DecouplingPatterns._03_TestDataBuilder;

namespace WritingMaintainableUnitTests.Tests.Module4DecouplingPatterns._09_FixtureObjectPattern
{
    [Specification]
    public class When_an_approver_attempts_to_approve_an_expense_sheet
    {
        private static readonly Guid ApproverId = new Guid("224FE5B8-EDBB-4F8B-8654-715C1C294CFD");
        private static readonly Guid ExpenseSheetId = new Guid("35BF01CB-4691-42B3-BC08-5E997B0F8415");
        
        [Establish]
        public void Context()
        {
            _fixture = new ApproveExpenseSheetHandlerFixture();

            _fixture.SubjectUnderTestCanRetrieve(
                Example.HeadOfDepartment()
                    .WithId(ApproverId)
                    .Build());

            _expenseSheet = _fixture.SubjectUnderTestCanRetrieve(
                Example.ExpenseSheet()
                    .WithId(ExpenseSheetId)
                    .WithExpense(36.50m, new DateTime(2018, 11, 01), "Sushi dinner"));

            _sut = _fixture.CreateSubjectUnderTest();    
        }

        [Because]
        public void Of()
        {
            var command = new ApproveExpenseSheet(ExpenseSheetId, ApproverId);
            _result = _sut.Handle(command);
        }

        [Observation]
        public void Then_the_approved_expense_sheet_should_be_saved()
        {
            _fixture.ExpenseSheetRepository.WasToldToSave(_expenseSheet);
        }
        
        [Observation]
        public void Then_the_operation_should_succeed()
        {
            Assert.That(_result.IsSuccessful);
        }

        private ExpenseSheet _expenseSheet;
        private ApproveExpenseSheetHandlerFixture _fixture;
        private Result _result;
        private ApproveExpenseSheetHandler _sut;
    }

    [Specification]
    public class When_an_unknown_approver_attempts_to_approve_an_expense_sheet
    {
        private static readonly Guid UnknownApproverId = new Guid("18D99F4E-4F41-474B-AED9-2B01BD49CD93");
        private static readonly Guid ExpenseSheetId = new Guid("35BF01CB-4691-42B3-BC08-5E997B0F8415");
        
        [Establish]
        public void Context()
        {
            var fixture = new ApproveExpenseSheetHandlerFixture();
            
            fixture.SubjectUnderTestCanRetrieve(null as HeadOfDepartment);

            fixture.SubjectUnderTestCanRetrieve(
                Example.ExpenseSheet()
                    .WithId(ExpenseSheetId)
                    .WithExpense(36.50m, new DateTime(2018, 11, 01), "Sushi dinner"));
            
            _sut = fixture.CreateSubjectUnderTest();
        }

        [Because]
        public void Of()
        {
            var command = new ApproveExpenseSheet(ExpenseSheetId, UnknownApproverId);
            _result = _sut.Handle(command);
        }
        
        [Observation]
        public void Then_the_operation_should_fail()
        {
            Assert.That(_result.IsSuccessful, Is.False);
        }

        private Result _result;
        private ApproveExpenseSheetHandler _sut;
    }

    [Specification]
    public class When_an_approver_attempts_to_approve_an_unknown_expense_sheet
    {
        private static readonly Guid ApproverId = new Guid("224FE5B8-EDBB-4F8B-8654-715C1C294CFD");
        private static readonly Guid UnknownExpenseSheetId = new Guid("85FFF5F2-D217-4335-A18F-69E687999B62");
        
        [Establish]
        public void Context()
        {
            var fixture = new ApproveExpenseSheetHandlerFixture();

            fixture.SubjectUnderTestCanRetrieve(
                Example.HeadOfDepartment()
                    .WithId(ApproverId)
                    .Build());

            fixture.SubjectUnderTestCanRetrieve(null as ExpenseSheet);

            _sut = fixture.CreateSubjectUnderTest();   
        }

        [Because]
        public void Of()
        {
            var command = new ApproveExpenseSheet(UnknownExpenseSheetId, ApproverId);
            _result = _sut.Handle(command);
        }
        
        [Observation]
        public void Then_the_operation_should_fail()
        {
            Assert.That(_result.IsSuccessful, Is.False);
        }

        private Result _result;
        private ApproveExpenseSheetHandler _sut;
    }

    [Specification]
    public class When_an_approver_attempts_to_approve_an_expense_sheet_which_results_in_a_domain_violation
    {
        private static readonly Guid ApproverId = new Guid("224FE5B8-EDBB-4F8B-8654-715C1C294CFD");
        private static readonly Guid ExpenseSheetId = new Guid("35BF01CB-4691-42B3-BC08-5E997B0F8415");
        
        [Establish]
        public void Context()
        {            
            var fixture = new ApproveExpenseSheetHandlerFixture();

            fixture.SubjectUnderTestCanRetrieve(
                Example.HeadOfDepartment()
                    .WithId(ApproverId)
                    .Build());

            fixture.SubjectUnderTestCanRetrieve(
                Example.ExpenseSheet()
                    .WithId(ExpenseSheetId)
                    .WithExpense(2500m, new DateTime(2018, 11, 01), "New MacBook Air"));

            _sut = fixture.CreateSubjectUnderTest();   
        }

        [Because]
        public void Of()
        {
            var command = new ApproveExpenseSheet(ExpenseSheetId, ApproverId);
            _result = _sut.Handle(command);
        }

        [Observation]
        public void Then_the_operation_should_fail()
        {
            Assert.That(_result.IsSuccessful, Is.False);
        }

        private Result _result;
        private ApproveExpenseSheetHandler _sut;
    }

    public class ApproveExpenseSheetHandlerFixture
    {
        private IApproverRepository ApproverRepository { get; }
        public IExpenseSheetRepository ExpenseSheetRepository { get; }

        public ApproveExpenseSheetHandlerFixture()
        {
            ApproverRepository = Substitute.For<IApproverRepository>();
            ExpenseSheetRepository = Substitute.For<IExpenseSheetRepository>();
        }

        public ApproveExpenseSheetHandler CreateSubjectUnderTest()
        {
            return new ApproveExpenseSheetHandler(ExpenseSheetRepository, ApproverRepository);
        }
        
        public HeadOfDepartment SubjectUnderTestCanRetrieve(HeadOfDepartment headOfDepartment)
        {
            ApproverRepository.Get(Guid.Empty).ReturnsForAnyArgs(headOfDepartment);
            return headOfDepartment;
        }

        public ExpenseSheet SubjectUnderTestCanRetrieve(ExpenseSheet expenseSheet)
        {
            ExpenseSheetRepository.Get(Guid.Empty).ReturnsForAnyArgs(expenseSheet);
            return expenseSheet;
        }
    }
    
    public static class ExpenseSheetRepositoryMockExtensions
    {
        public static void WasToldToSave(this IExpenseSheetRepository expenseSheetRepository, ExpenseSheet expenseSheet)
        {
            expenseSheetRepository.Received().Save(expenseSheet);
        }
    }
}