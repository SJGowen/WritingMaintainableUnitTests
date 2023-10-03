using NSubstitute;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module1_TypesOfTests.BehaviourVerification;
// ReSharper disable InconsistentNaming

namespace WritingMaintainableUnitTests.Tests.Module1_TypesOfTests.BehaviourVerification;

[TestFixture]
public class When_registering_a_new_user
{
    [Test]
    public void Then_the_new_user_should_be_registered_in_the_system()
    {
        var userRepository = Substitute.For<IUserRepository>();
        var emailSender = Substitute.For<IEmailSender>();
        
        var sut = new RegisterUserHandler(userRepository, emailSender);
        
        var command = new RegisterUser("john@doe.com");
        sut.Handle(command);
        
        userRepository.Received().Save(Arg.Any<User>());
    }

    [Test]
    public void Then_a_confirmation_email_should_be_sent()
    {
        var userRepository = Substitute.For<IUserRepository>();
        var emailSender = Substitute.For<IEmailSender>();
        
        var sut = new RegisterUserHandler(userRepository, emailSender);
        
        var command = new RegisterUser("john@doe.com");
        sut.Handle(command);
        
        emailSender.Received().Send(Arg.Any<EmailMessage>()); 
    }
}