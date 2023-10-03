using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module3AnatomyOfUnitTests.ArrangeActAssertPerTestMethodOrClass;

namespace WritingMaintainableUnitTests.Tests.Module3AnatomyOfUnitTests._02_ArrangeActAssertPerTestMethod;

[TestFixture]
public class RegisterUserHandlerTests_DRY_Two
{
    private const string EmailOfRegisteredUser = "john@deere.com";
    
    [SetUp]
    public void SetUp()
    {
        // Arrange
        _userRepository = Substitute.For<IUserRepository>();
        _emailSender = Substitute.For<IEmailSender>();
        _sut = new RegisterUserHandler(_userRepository, _emailSender);
        
        _userRepository.GetByEmail(Arg.Any<string>()).ReturnsNull();
        
        // Act
        var command = new RegisterUser(EmailOfRegisteredUser);
        _result = _sut.Handle(command);
    }
    
    [Test]
    public void RegisterUser_UnknownEmail_NewUserShouldBeRegisteredInTheSystem()
    {            
        // Assert
        _userRepository.Received().Save(Arg.Any<User>());
    }
    
    [Test]
    public void RegisterUser_UnknownEmail_ShouldSendConfirmationEmailToNewUser()
    {           
        // Assert
        _emailSender.Received().Send(Arg.Any<EmailMessage>());
    }
    
    [Test]
    public void RegisterUser_UnknownEmail_ShouldSucceed()
    {                        
        // Assert
        Assert.That(_result.IsSuccessful);
    }
    
    [Test]
    public void RegisterUser_KnownEmail_ShouldFail()
    {
        // Arrange some more
        var registeredUser = new User(EmailOfRegisteredUser);            
        _userRepository.GetByEmail(Arg.Any<string>()).Returns(registeredUser);

        // Act
        var command = new RegisterUser(EmailOfRegisteredUser);
        var result = _sut.Handle(command);
        
        // Assert
        Assert.That(result.IsSuccessful, Is.False);
    }

    private IEmailSender _emailSender;
    private RegisterUserHandler _sut;
    private IUserRepository _userRepository;
    private Result _result;
}