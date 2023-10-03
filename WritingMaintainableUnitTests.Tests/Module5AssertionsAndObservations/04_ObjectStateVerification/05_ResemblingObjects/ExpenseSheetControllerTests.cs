using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using DeepEqual.Syntax;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using WritingMaintainableUnitTests.Module4DecouplingPatterns.Expenses;
using WritingMaintainableUnitTests.Module5AssertionsAndObservations;
using WritingMaintainableUnitTests.Tests.Common;

namespace WritingMaintainableUnitTests.Tests.Module5AssertionsAndObservations._04_ObjectStateVerification._05_ResemblingObjects;

[Specification]
public class When_handling_a_request_for_creating_a_new_expense_sheet 
{
    [Establish]
    public void Context()
    {
        _formModel = new CreateExpenseSheetFormModel
        {
            EmployeeId = new Guid("A881CF9F-888E-482B-A5EE-ACDD10D01CB2"),
            SubmissionDate = new DateTime(2019, 03, 15)
        };
        
        var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
        
        _commandHandler = fixture.Freeze<ICommandHandler<CreateExpenseSheet>>();
        _commandHandler.Handle(null).ReturnsForAnyArgs(Result.Success());
        
        _sut = fixture.Create<ExpenseSheetController>(); 
    }

    [Because]
    public void Of()
    {
        _result = _sut.Create(_formModel);
    }
    
    [Observation]
    public void Not_so_splendid_way_to_verify_the_dispatching_of_a_command_to_the_domain_as_an_indirect_output()
    {
        _commandHandler.Received().Handle(Arg.Is<CreateExpenseSheet>(command => 
            command.EmployeeId == _formModel.EmployeeId &&
            command.SubmissionDate == _formModel.SubmissionDate));
    }
    
    [Observation]
    public void Better_way_to_verify_the_dispatching_of_a_command_to_the_domain_as_an_indirect_output()
    {
        var expectedCommand = Resemblance
            .Between<CreateExpenseSheetFormModel, CreateExpenseSheet>()
            .WithSource(_formModel)
            .IgnoreOn(command => command.Id)
            .ToResemblingObject();

        _commandHandler.Received().Handle(expectedCommand);
    }
            
    [Observation]
    public void Then_HTTP_status_code_200_should_be_sent_back_to_the_client()
    {
        _result.Should_be_an_instance_of<OkResult>();        
    }

    private ICommandHandler<CreateExpenseSheet> _commandHandler;
    private CreateExpenseSheetFormModel _formModel;
    private IActionResult _result;
    private ExpenseSheetController _sut;
}

[Specification]
public class When_handling_a_request_for_creating_a_new_expense_sheet_that_does_not_meet_the_business_criteria
{
    [Establish]
    public void Context()
    {
        _invalidFormModel = new CreateExpenseSheetFormModel
        {
            EmployeeId = new Guid("4C458357-A90B-4A5F-8170-BA0D5C7A811D"),
            SubmissionDate = new DateTime(2019, 03, 31)
        };
        
        var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });            
        var commandHandler = fixture.Freeze<ICommandHandler<CreateExpenseSheet>>();

        var failure = Result.Failure(new DomainViolation("This is highly irregular"));
        commandHandler.Handle(null).ReturnsForAnyArgs(failure);
        
        _sut = fixture.Create<ExpenseSheetController>(); 
    }

    [Because]
    public void Of()
    {
        _result = _sut.Create(_invalidFormModel);
    }
    
    [Observation]
    public void Then_HTTP_status_code_400_should_be_sent_back_to_the_client()
    {
        _result.Should_be_an_instance_of<BadRequestResult>();        
    }
    
    private CreateExpenseSheetFormModel _invalidFormModel;
    private IActionResult _result;
    private ExpenseSheetController _sut;
}


#region Resemblance syntax

public static class Resemblance
{        
    public static Resemblance<TSource, TResemblingObject> Between<TSource, TResemblingObject>()
    {
        return new Resemblance<TSource, TResemblingObject>();
    }    
}

public class Resemblance<TSource, TDestination>
{
    private TSource _source;
    private readonly List<Expression<Func<TSource, object>>> _ignoredPropertiesOnSource;
    private readonly List<Expression<Func<TDestination, object>>> _ignoredPropertiesOnDestination;

    public Resemblance()
    {
        _ignoredPropertiesOnSource = new List<Expression<Func<TSource, object>>>();
        _ignoredPropertiesOnDestination = new List<Expression<Func<TDestination, object>>>();
    }

    public Resemblance<TSource, TDestination> WithSource(TSource source)
    {
        _source = source;
        return this;
    }
    
    public Resemblance<TSource, TDestination> IgnoreOn(Expression<Func<TSource, object>> property)
    {
        _ignoredPropertiesOnSource.Add(property);
        return this;
    }
    
    public Resemblance<TSource, TDestination> IgnoreOn(Expression<Func<TDestination, object>> property)
    {
        _ignoredPropertiesOnDestination.Add(property);
        return this;
    }
    
    public TDestination ToResemblingObject()
    {
        if(_source == null)
        {
            const string errorMessage = "An instance of the source object must be specified through the 'WithSource' method.";
            throw new InvalidOperationException(errorMessage);
        }
        
        Predicate<TDestination> predicate =
            destination =>
            {
                var comparison = _source.WithDeepEqual(destination);

                comparison = _ignoredPropertiesOnSource.Aggregate(comparison, 
                    (aggregate, ignoredProperty) => aggregate.IgnoreSourceProperty(ignoredProperty));
                
                comparison = _ignoredPropertiesOnDestination.Aggregate(comparison, 
                    (aggregate, ignoredProperty) => aggregate.IgnoreDestinationProperty(ignoredProperty));
                
                return comparison.Compare();
            };

        return Arg.Is<TDestination>(resemblingObject => predicate(resemblingObject));
    }
}

#endregion
