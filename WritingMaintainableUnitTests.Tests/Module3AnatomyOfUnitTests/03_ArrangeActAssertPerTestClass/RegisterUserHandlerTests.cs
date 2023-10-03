using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module3AnatomyOfUnitTests.ArrangeActAssertPerTestMethodOrClass;
using WritingMaintainableUnitTests.Tests.Common;

namespace WritingMaintainableUnitTests.Tests.Module3AnatomyOfUnitTests._03_ArrangeActAssertPerTestClass
{
    [TestFixture]
    public class When_registering_a_user_with_an_unknown_email
    {
        [OneTimeSetUp]
        public void Arrange()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _userRepository.GetByEmail(Arg.Any<string>()).ReturnsNull();
            
            _emailSender = Substitute.For<IEmailSender>();
            
            _sut = new RegisterUserHandler(_userRepository, _emailSender);
        }

        [OneTimeSetUp]
        public void Act()
        {
            var command = new RegisterUser("john@doe.com");
            _result = _sut.Handle(command);    
        }

        // Asserts
        [Test]
        public void Then_a_new_user_should_be_registered_in_the_system()
        {
            _userRepository.Received().Save(Arg.Any<User>());       
        }

        [Test]
        public void Then_a_confirmation_email_should_be_send_to_the_new_user()
        {
            _emailSender.Received().Send(Arg.Any<EmailMessage>());
        }

        [Test]
        public void Then_the_operation_should_succeed()
        {
            Assert.That(_result.IsSuccessful);   
        }

        private IEmailSender _emailSender;
        private IUserRepository _userRepository;
        private RegisterUserHandler _sut;
        private Result _result;
    }

    [Specification]
    public class When_registering_a_user_with_a_known_email
    {
        private const string EmailOfRegisteredUser = "john@doer.com";
        
        [Establish] 
        public void Context()
        {
            var registeredUser = new User(EmailOfRegisteredUser);
            
            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByEmail(Arg.Any<string>()).Returns(registeredUser);
            
            _sut = new RegisterUserHandler(userRepository, null);
        }   
        
        [Because] 
        public void Of()
        {
            var command = new RegisterUser(EmailOfRegisteredUser);
            _result = _sut.Handle(command);
        }
        
        [Observation] 
        public void Then_the_operation_should_fail()
        {            
            Assert.That(_result.IsSuccessful, Is.False);
        }
        
        private RegisterUserHandler _sut;
        private Result _result;
    }
}