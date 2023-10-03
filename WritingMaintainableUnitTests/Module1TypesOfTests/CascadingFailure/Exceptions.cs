using System;

namespace WritingMaintainableUnitTests.Module1TypesOfTests.CascadingFailure;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message)
        : base(message)
    { }
}

public class UnknownCustomerException : Exception
{
    public UnknownCustomerException(string message)
        : base(message)
    { }
}