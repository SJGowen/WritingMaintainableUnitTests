using Machine.Specifications;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using WritingMaintainableUnitTests.Module3_AnatomyOfUnitTests.ArrangeActAssertPerTestMethodOrClass;
using Result = WritingMaintainableUnitTests.Module3_AnatomyOfUnitTests.ArrangeActAssertPerTestMethodOrClass.Result;
// ReSharper disable InconsistentNaming

namespace WritingMaintainableUnitTests.Tests.Module3_AnatomyOfUnitTests._04_MSpecExample
{
    [Subject("Registering a user")]
    public class When_registering_a_user_with_an_unknown_email
    {
        Establish context = () =>
        {
            _userRepository = Substitute.For<IUserRepository>();
            _userRepository.GetByEmail(Arg.Any<string>()).ReturnsNull();

            _emailSender = Substitute.For<IEmailSender>();

            _sut = new RegisterUserHandler(_userRepository, _emailSender);
        };

        Because of = () =>
        {
            var command = new RegisterUser("john@doe.com");
            _result = _sut.Handle(command);
        };

        It should_register_a_new_user_in_the_system = () =>
        {
            _userRepository.Received().Save(Arg.Any<User>());       
        };

        It should_send_a_confirmation_email_to_the_new_user = () =>
        {
            _emailSender.Received().Send(Arg.Any<EmailMessage>());
        };

        It should_complete_the_operation_successfully = () =>
        {
            _result.IsSuccessful.ShouldBeTrue(); 
        };

        private static IEmailSender _emailSender;
        private static IUserRepository _userRepository;
        private static RegisterUserHandler _sut;
        private static Result _result;
    }

    [Subject("Registering a user")]
    public class When_registering_a_user_with_a_known_email
    {
        private const string EmailOfRegisteredUser = "john@doer.com";
        
        Establish context = () =>
        {
            var registeredUser = new User(EmailOfRegisteredUser);
            
            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByEmail(Arg.Any<string>()).Returns(registeredUser);
            
            _sut = new RegisterUserHandler(userRepository, null);
        };  
        
        Because of = () =>
        {
            var command = new RegisterUser(EmailOfRegisteredUser);
            _result = _sut.Handle(command);
        };
        
        It should_fail_the_operation = () =>
        {   
            _result.IsSuccessful.ShouldBeFalse();
        };
        
        private static RegisterUserHandler _sut;
        private static Result _result;
    }
}