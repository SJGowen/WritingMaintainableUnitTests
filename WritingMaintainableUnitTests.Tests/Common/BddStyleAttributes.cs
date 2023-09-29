using NUnit.Framework;

namespace WritingMaintainableUnitTests.Tests.Common
{
    public class SpecificationAttribute : TestFixtureAttribute
    {}

    public class EstablishAttribute : OneTimeSetUpAttribute
    {}

    public class BecauseAttribute : OneTimeSetUpAttribute
    {}

    public class ObservationAttribute : TestAttribute
    {}
}