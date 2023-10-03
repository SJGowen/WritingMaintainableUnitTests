using System;
using System.Collections.Generic;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module4DecouplingPatterns.Expenses;
using WritingMaintainableUnitTests.Tests.Common;
using WritingMaintainableUnitTests.Tests.Module4DecouplingPatterns._03_TestDataBuilder;

namespace WritingMaintainableUnitTests.Tests.Module4DecouplingPatterns._04_StateVerification
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
            
            ExpenseSheet expenseSheet = Example.ExpenseSheet()
                .WithId(ExpenseSheetId)
                .WithExpense(36.50m, new DateTime(2018, 11, 01), "Sushi dinner");

            _objectRelationalMapper = new FakeObjectRelationalMapper();
            _objectRelationalMapper.Save(ApproverId, headOfDepartment);
            _objectRelationalMapper.Save(ExpenseSheetId, expenseSheet);
            
            var approverRepository = new ApproverRepository(_objectRelationalMapper);            
            var expenseSheetRepository = new ExpenseSheetRepository(_objectRelationalMapper);

            _sut = new ApproveExpenseSheetHandler(expenseSheetRepository, approverRepository);    
        }

        [Because]
        public void Of()
        {
            var command = new ApproveExpenseSheet(ExpenseSheetId, ApproverId);
            _result = _sut.Handle(command);
        }

        [Observation]
        public void Then_the_expense_sheet_should_be_approved()
        {
            var expenseSheet = _objectRelationalMapper.Get<ExpenseSheet>(ExpenseSheetId);
            Assert.That(expenseSheet.Status, Is.EqualTo(ExpenseSheetStatus.Approved));
        }
        
        [Observation]
        public void Then_the_operation_should_succeed()
        {
            Assert.That(_result.IsSuccessful);
        }

        private IObjectRelationalMapper _objectRelationalMapper;
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
            ExpenseSheet expenseSheet = Example.ExpenseSheet()
                .WithId(ExpenseSheetId)
                .WithExpense(36.50m, new DateTime(2018, 11, 01), "Sushi dinner");
            
            var objectRelationalMapper = new FakeObjectRelationalMapper();
            objectRelationalMapper.Save(ExpenseSheetId, expenseSheet);
            
            var approverRepository = new ApproverRepository(objectRelationalMapper);            
            var expenseSheetRepository = new ExpenseSheetRepository(objectRelationalMapper);
            
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
            
            var objectRelationalMapper = new FakeObjectRelationalMapper();
            objectRelationalMapper.Save(ApproverId, headOfDepartment);
            
            var approverRepository = new ApproverRepository(objectRelationalMapper);            
            var expenseSheetRepository = new ExpenseSheetRepository(objectRelationalMapper);
                        
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
            
            ExpenseSheet expenseSheet = Example.ExpenseSheet()
                .WithId(ExpenseSheetId)
                .WithExpense(2500m, new DateTime(2018, 11, 01), "New MacBook Air");

            _objectRelationalMapper = new FakeObjectRelationalMapper();
            _objectRelationalMapper.Save(ApproverId, headOfDepartment);
            _objectRelationalMapper.Save(ExpenseSheetId, expenseSheet);
            
            var approverRepository = new ApproverRepository(_objectRelationalMapper);            
            var expenseSheetRepository = new ExpenseSheetRepository(_objectRelationalMapper);

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

        private IObjectRelationalMapper _objectRelationalMapper;
        private Result _result;
        private ApproveExpenseSheetHandler _sut;
    }

    #region Fakes
    
    public class FakeObjectRelationalMapper : IObjectRelationalMapper
    {
        private readonly Dictionary<object, object> _identityMap;

        public FakeObjectRelationalMapper()
        {
            _identityMap = new Dictionary<object, object>();
        }
        
        public TEntity Get<TEntity>(object id)
        {
            var isFound = _identityMap.TryGetValue(id, out var approver);
            return (TEntity) (isFound ? approver : null);
        }

        public void Save<TEntity>(object id, TEntity entity)
        {
            _identityMap[id] = entity;
        }
    }

    #endregion
}