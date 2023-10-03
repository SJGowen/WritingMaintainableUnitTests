using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module1TypesOfTests.CascadingFailure;

namespace WritingMaintainableUnitTests.Tests.Module1TypesOfTests.CascadingFailure;

[TestFixture]
public class When_an_authorized_user_changes_the_email_of_a_customer
{
    [Test]
    public void Then_the_email_of_the_specified_customer_should_be_changed()
    {
        var customer = new Customer(354, "john@doe.com");
        var command = new ChangeCustomerEmail(354, "john@foo.com");

        var userContext = new UserContext(UserRole.HelpDeskStaff);
        var authorizationService = new AuthorizationService(userContext);

        var customerRepository = Substitute.For<ICustomerRepository>();
        customerRepository.Get(Arg.Any<int>()).Returns(customer);

        var sut = new ChangeCustomerEmailHandler(authorizationService, customerRepository);
        sut.Handle(command);

        Assert.That(customer.Email, Is.EqualTo("john@foo.com"));
    }

    [Test]
    public void Then_the_system_should_henceforth_use_the_new_email_for_contacting_the_customer()
    {
        var customer = new Customer(354, "john@doe.com");
        var command = new ChangeCustomerEmail(354, "john@foo.com");

        var userContext = new UserContext(UserRole.HelpDeskStaff);
        var authorizationService = new AuthorizationService(userContext);

        var customerRepository = Substitute.For<ICustomerRepository>();
        customerRepository.Get(Arg.Any<int>()).Returns(customer);

        var sut = new ChangeCustomerEmailHandler(authorizationService, customerRepository);
        sut.Handle(command);

        customerRepository.Received().Save(customer);
    }
}

[TestFixture]
public class When_an_unauthorized_user_attempts_to_change_the_email_of_a_customer
{
    [Test]
    public void Then_an_exception_should_be_thrown()
    {
        var command = new ChangeCustomerEmail(354, "john@foo.com");

        var userContext = new UserContext(UserRole.Unknown);
        var authorizationService = new AuthorizationService(userContext);

        var customerRepository = Substitute.For<ICustomerRepository>();

        var sut = new ChangeCustomerEmailHandler(authorizationService, customerRepository);
        TestDelegate changeCustomerEmail = () => sut.Handle(command);

        Assert.That(changeCustomerEmail, Throws.InstanceOf<UnauthorizedException>());
    }
}

[TestFixture]
public class When_an_authorized_user_attempts_to_change_the_email_of_a_customer_that_is_not_known_by_the_system
{
    [Test]
    public void Then_an_exception_should_be_thrown()
    {
        var command = new ChangeCustomerEmail(354, "john@foo.com");

        var userContext = new UserContext(UserRole.BackOfficeManager);
        var authorizationService = new AuthorizationService(userContext);

        var customerRepository = Substitute.For<ICustomerRepository>();
        customerRepository.Get(Arg.Any<int>()).ReturnsNull();

        var sut = new ChangeCustomerEmailHandler(authorizationService, customerRepository);
        TestDelegate changeCustomerEmail = () => sut.Handle(command);

        Assert.That(changeCustomerEmail, Throws.InstanceOf<UnknownCustomerException>());
    }
}