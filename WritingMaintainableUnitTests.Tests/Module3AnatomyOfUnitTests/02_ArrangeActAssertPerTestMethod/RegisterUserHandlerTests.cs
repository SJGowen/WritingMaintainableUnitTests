using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module3AnatomyOfUnitTests.ArrangeActAssertPerTestMethodOrClass;

namespace WritingMaintainableUnitTests.Tests.Module3AnatomyOfUnitTests._02_ArrangeActAssertPerTestMethod
{
    [TestFixture]
    public class RegisterUserHandlerTests
    {
        [Test]
        public void RegisterUser_UnknownEmail_NewUserShouldBeRegisteredInTheSystem()
        {
            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByEmail(Arg.Any<string>()).ReturnsNull();
            var emailSender = Substitute.For<IEmailSender>();
            var sut = new RegisterUserHandler(userRepository, emailSender);
            
            var command = new RegisterUser("john@doe.com");
            sut.Handle(command);
            
            userRepository.Received().Save(Arg.Any<User>());
        }
        
        [Test]
        public void RegisterUser_UnknownEmail_ShouldSendConfirmationEmailToNewUser()
        {
            // Arrange
            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByEmail(Arg.Any<string>()).ReturnsNull();
            
            var emailSender = Substitute.For<IEmailSender>();
            var sut = new RegisterUserHandler(userRepository, emailSender);
            
            // Act
            var command = new RegisterUser("john@doe.com");
            sut.Handle(command);
            
            // Assert
            emailSender.Received().Send(Arg.Any<EmailMessage>());
        }
        
        [Test]
        public void RegisterUser_UnknownEmail_ShouldSucceed()
        {
            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByEmail(Arg.Any<string>()).ReturnsNull();
            var emailSender = Substitute.For<IEmailSender>();
            var sut = new RegisterUserHandler(userRepository, emailSender);
            
            var command = new RegisterUser("john@doe.com");
            var result = sut.Handle(command);
            
            Assert.That(result.IsSuccessful);
        }
       
        [Test]
        public void RegisterUser_KnownEmail_ShouldFail()
        {
            const string emailOfRegisteredUser = "john@doer.com";
            var registeredUser = new User(emailOfRegisteredUser);
            
            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByEmail(Arg.Any<string>()).Returns(registeredUser);
            var sut = new RegisterUserHandler(userRepository, null);
            
            var command = new RegisterUser(emailOfRegisteredUser);
            var result = sut.Handle(command);
            
            Assert.That(result.IsSuccessful, Is.False);
        }
    }
}