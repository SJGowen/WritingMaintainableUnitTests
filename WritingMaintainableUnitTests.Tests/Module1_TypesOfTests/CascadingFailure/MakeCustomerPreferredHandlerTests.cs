using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module1_TypesOfTests.CascadingFailure;
// ReSharper disable InconsistentNaming

namespace WritingMaintainableUnitTests.Tests.Module1_TypesOfTests.CascadingFailure
{
    [TestFixture]
    public class When_an_authorized_user_makes_a_customer_preferred
    {        
        [Test]
        public void Then_the_specified_customer_should_be_made_a_preferred_customer()
        {
            var customer = new Customer(354, "john@doe.com");
            var command = new MakeCustomerPreferred(354);
            
            var userContext = new UserContext(UserRole.HelpDeskStaff);
            var authorizationService = new AuthorizationService(userContext);

            var customerRepository = Substitute.For<ICustomerRepository>();
            customerRepository.Get(Arg.Any<int>()).Returns(customer);
            
            var sut = new MakeCustomerPreferredHandler(authorizationService, customerRepository);
            sut.Handle(command); 
            
            Assert.That(customer.Type, Is.EqualTo(CustomerType.Preferred));
        }
        
        [Test]
        public void Then_the_specified_customer_should_henceforth_be_treated_as_a_preferred_customer_by_the_system()
        {
            var customer = new Customer(354, "john@doe.com");
            var command = new MakeCustomerPreferred(354);
            
            var userContext = new UserContext(UserRole.HelpDeskStaff);
            var authorizationService = new AuthorizationService(userContext);

            var customerRepository = Substitute.For<ICustomerRepository>();
            customerRepository.Get(Arg.Any<int>()).Returns(customer);
            
            var sut = new MakeCustomerPreferredHandler(authorizationService, customerRepository);
            sut.Handle(command); 
            
            customerRepository.Received().Save(customer);
        }
    }
    
    [TestFixture]
    public class When_an_unauthorized_user_attempts_to_make_a_customer_preferred
    {        
        [Test]
        public void Then_an_exception_should_be_thrown()
        {
            var command = new MakeCustomerPreferred(354);
            
            var userContext = new UserContext(UserRole.Unknown);
            var authorizationService = new AuthorizationService(userContext);
            
            var customerRepository = Substitute.For<ICustomerRepository>();
            
            var sut = new MakeCustomerPreferredHandler(authorizationService, customerRepository);
            TestDelegate makeCustomerPreferred = () => sut.Handle(command);
            
            Assert.That(makeCustomerPreferred, Throws.InstanceOf<UnauthorizedException>());    
        }
    }
    
    [TestFixture]
    public class When_an_authorized_user_attempts_to_make_a_customer_preferred_that_is_not_known_by_the_system
    {               
        [Test]
        public void Then_an_exception_should_be_thrown()
        {
            var command = new MakeCustomerPreferred(354);
            
            var userContext = new UserContext(UserRole.BackOfficeManager);
            var authorizationService = new AuthorizationService(userContext);
            
            var customerRepository = Substitute.For<ICustomerRepository>();
            customerRepository.Get(Arg.Any<int>()).ReturnsNull();
            
            var sut = new MakeCustomerPreferredHandler(authorizationService, customerRepository);
            TestDelegate makeCustomerPreferred = () => sut.Handle(command);
            
            Assert.That(makeCustomerPreferred, Throws.InstanceOf<UnknownCustomerException>());
        }
    }
}