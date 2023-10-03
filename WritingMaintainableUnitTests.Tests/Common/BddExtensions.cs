using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DeepEqual.Syntax;
using NUnit.Framework;

namespace WritingMaintainableUnitTests.Tests.Common;

public static class BddExtensions
{
    public static void Should_be_equal_to<T>(this T actual, T expected)
    {
        Assert.That(actual, Is.EqualTo(expected));
    }

    public static void Should_be_deep_equal_to<T>(this T actual, T expected)
    {
        actual.ShouldDeepEqual(expected);
    }
    
    public static void Should_exist<T>(this T actual)
    {
        Assert.That(actual, Is.Not.Null);
    }
    
    public static void Should_contain<T>(this IEnumerable<T> items, Func<T, Boolean> predicate)
    {
        Assert.That(items.Any(predicate));
    }
    
    public static void Should_be_an_instance_of<Type>(this Object item)
    {
        Assert.That(item, Is.TypeOf(typeof(Type)));
    }
    
    public static void Should_be_false(this bool booleanValue)
    {
        Assert.That(booleanValue, Is.False);
    }
    
    public static void Should_be_true(this bool booleanValue)
    {
        Assert.That(booleanValue, Is.True);
    }
    
    public static TException Should_throw_an<TException>(this Action operation) where TException : Exception
    {
        var expectedException = GetExceptionWhilePerforming(operation);

        var message = $"Expected an exception of type '{typeof(TException)}' to be thrown.";
        Assert.That(expectedException, Is.TypeOf<TException>(), message);

        return (TException) expectedException;
    }

    private static Exception GetExceptionWhilePerforming(Action operation)
    {
        try
        {
            operation();
            return null;
        }
        catch(Exception ex)
        {
            return ex;
        }
    }
}