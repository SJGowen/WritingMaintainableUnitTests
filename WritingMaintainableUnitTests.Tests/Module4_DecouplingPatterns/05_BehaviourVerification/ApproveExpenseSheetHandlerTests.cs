using System;
using System.Linq;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module4_DecouplingPatterns.Expenses;
using WritingMaintainableUnitTests.Tests.Common;
using WritingMaintainableUnitTests.Tests.Module4_DecouplingPatterns._03_TestDataBuilder;
// ReSharper disable InconsistentNaming

namespace WritingMaintainableUnitTests.Tests.Module4_DecouplingPatterns._05_BehaviourVerification
{
    [Specification]
    public class When_an_approver_attempts_to_approve_an_expense_sheet
    {
        private static readonly Guid ApproverId = new Guid("224FE5B8-EDBB-4F8B-8654-715C1C294CFD");
        private static readonly Guid ExpenseSheetId = new Guid("35BF01CB-4691-42B3-BC08-5E997B0F8415");
        
        [Establish]
        public void Context()
        {
            HeadOfDepartment headOfDepartment = Example.HeadOfDepartment().WithId(ApproverId);
            
            _expenseSheet = Example.ExpenseSheet()
                .WithId(ExpenseSheetId)
                .WithExpense(36.50m, new DateTime(2018, 11, 01), "Sushi dinner");

            var approverRepository = Substitute.For<IApproverRepository>();
            approverRepository.Get(ApproverId).Returns(headOfDepartment);

            _expenseSheetRepository = Substitute.For<IExpenseSheetRepository>();
            _expenseSheetRepository.Get(ExpenseSheetId).Returns(_expenseSheet);

            _sut = new ApproveExpenseSheetHandler(_expenseSheetRepository, approverRepository);    
        }

        [Because]
        public void Of()
        {
            var command = new ApproveExpenseSheet(ExpenseSheetId, ApproverId);
            _result = _sut.Handle(command);
        }

        [Observation]
        public void Then_the_approved_expense_sheet_should_be_saved__SPY()
        {
            var lastReceivedCall = _expenseSheetRepository.ReceivedCalls().Last();
            Assert.That(lastReceivedCall, Is.Not.Null);
            Assert.That(lastReceivedCall.GetMethodInfo().Name, Is.EqualTo("Save"));
            
            var savedExpenseSheet = lastReceivedCall.GetArguments().First() as ExpenseSheet;
            Assert.That(savedExpenseSheet, Is.Not.Null);
            Assert.That(savedExpenseSheet, Is.SameAs(_expenseSheet));
        }
        
        [Observation]
        public void Then_the_approved_expense_sheet_should_be_saved__MOCK()
        {
            _expenseSheetRepository.Received().Save(_expenseSheet);
        }
        
        [Observation]
        public void Then_the_operation_should_succeed()
        {
            Assert.That(_result.IsSuccessful);
        }

        private ExpenseSheet _expenseSheet;
        private IExpenseSheetRepository _expenseSheetRepository;
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
            var expenseSheet = Example.ExpenseSheet()
                .WithId(ExpenseSheetId)
                .WithExpense(36.50m, new DateTime(2018, 11, 01), "Sushi dinner");
            
            var approverRepository = Substitute.For<IApproverRepository>();
            approverRepository.Get(UnknownApproverId).Returns(null as ICanApproveExpenses);           
            
            var expenseSheetRepository = Substitute.For<IExpenseSheetRepository>();
            expenseSheetRepository.Get(ExpenseSheetId).Returns(expenseSheet);
            
            _sut = new ApproveExpenseSheetHandler(expenseSheetRepository, approverRepository);    
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
            HeadOfDepartment headOfDepartment = Example.HeadOfDepartment().WithId(ApproverId);
            
            var approverRepository = Substitute.For<IApproverRepository>();
            approverRepository.Get(ApproverId).Returns(headOfDepartment);           
            
            var expenseSheetRepository = Substitute.For<IExpenseSheetRepository>();
            expenseSheetRepository.Get(UnknownExpenseSheetId).Returns(null as ExpenseSheet);
                        
            _sut = new ApproveExpenseSheetHandler(expenseSheetRepository, approverRepository);    
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
            HeadOfDepartment headOfDepartment = Example.HeadOfDepartment().WithId(ApproverId);
            
            var expenseSheet = Example.ExpenseSheet()
                .WithId(ExpenseSheetId)
                .WithExpense(2500m, new DateTime(2018, 11, 01), "New MacBook Air");

            var approverRepository = Substitute.For<IApproverRepository>();
            approverRepository.Get(ApproverId).Returns(headOfDepartment);

            var expenseSheetRepository = Substitute.For<IExpenseSheetRepository>();
            expenseSheetRepository.Get(ExpenseSheetId).Returns(expenseSheet);

            _sut = new ApproveExpenseSheetHandler(expenseSheetRepository, approverRepository); 
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
}