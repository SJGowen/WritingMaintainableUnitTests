using System;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers;
using Castle.Windsor;
using NSubstitute;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module4DecouplingPatterns.Expenses;
using WritingMaintainableUnitTests.Tests.Common;
using WritingMaintainableUnitTests.Tests.Module4DecouplingPatterns._03_TestDataBuilder;

namespace WritingMaintainableUnitTests.Tests.Module4DecouplingPatterns._08_AutoMockingContainer;

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

        _container = new AutoMockingContainer<ApproveExpenseSheetHandler>();
        _container.Dependency<IApproverRepository>().Get(ApproverId).Returns(headOfDepartment);
        _container.Dependency<IExpenseSheetRepository>().Get(ExpenseSheetId).Returns(_expenseSheet);

        _sut = _container.SubjectUnderTest();    
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
        _container.Dependency<IExpenseSheetRepository>().Received().Save(_expenseSheet);
    }
    
    [Observation]
    public void Then_the_operation_should_succeed()
    {
        Assert.That(_result.IsSuccessful);
    }

    private ExpenseSheet _expenseSheet;
    private Result _result;
    private ApproveExpenseSheetHandler _sut;
    private AutoMockingContainer<ApproveExpenseSheetHandler> _container;
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
        
        var container = new AutoMockingContainer<ApproveExpenseSheetHandler>();
        container.Dependency<IApproverRepository>().Get(UnknownApproverId).Returns(null as ICanApproveExpenses);                       
        container.Dependency<IExpenseSheetRepository>().Get(ExpenseSheetId).Returns(expenseSheet);

        _sut = container.SubjectUnderTest();
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
        
        var container = new AutoMockingContainer<ApproveExpenseSheetHandler>();            
        container.Dependency<IApproverRepository>().Get(ApproverId).Returns(headOfDepartment);                       
        container.Dependency<IExpenseSheetRepository>().Get(UnknownExpenseSheetId).Returns(null as ExpenseSheet);
                    
        _sut = container.SubjectUnderTest();    
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

        var container = new AutoMockingContainer<ApproveExpenseSheetHandler>();            
        container.Dependency<IApproverRepository>().Get(ApproverId).Returns(headOfDepartment);
        container.Dependency<IExpenseSheetRepository>().Get(ExpenseSheetId).Returns(expenseSheet);

        _sut = container.SubjectUnderTest();
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

public class AutoMockingContainer<TSubjectUnderTest>
    where TSubjectUnderTest : class
{
    private readonly IWindsorContainer _container;
    
    public AutoMockingContainer()
    {
        _container = new WindsorContainer();
        _container.Register(Component.For<ILazyComponentLoader>().ImplementedBy<LazySubstituteLoader>());
        _container.Register(Component.For<TSubjectUnderTest>());
    }

    public TSubjectUnderTest SubjectUnderTest()
    {
        return _container.Resolve<TSubjectUnderTest>();
    }

    public TDependency Dependency<TDependency>() where TDependency : class
    {
        return _container.Resolve<TDependency>();
    }

    public void RegisterManualDependency<TDependency>(TDependency dependency) where TDependency : class
    {
        _container.Register(Component.For<TDependency>().Instance(dependency));
    }
}

public class LazySubstituteLoader : ILazyComponentLoader
{
    public IRegistration Load(string name, Type service, Arguments arguments)
    {
        return Component.For(service).Instance(Substitute.For(new[] { service }, null));
    }
}