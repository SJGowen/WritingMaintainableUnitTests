using WritingMaintainableUnitTests.Module3AnatomyOfUnitTests.ArrangeActAssertPerTestMethodOrClass;

namespace WritingMaintainableUnitTests.Module3AnatomyOfUnitTests.ArrangeActAssertPerTestMethodOrClass
{
    public class RegisterUserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;

        public RegisterUserHandler(
            IUserRepository userRepository,
            IEmailSender emailSender)
        {
            _userRepository = userRepository;
            _emailSender = emailSender;
        }

        public Result Handle(RegisterUser command)
        {
            var user = _userRepository.GetByEmail(command.Email);
            if (null != user)
            {
                var domainViolation = $"The user with email '{command.Email}' already exists.";
                return Result.Failure(domainViolation);
            }

            user = new User(command.Email);
            _userRepository.Save(user);

            var emailMessage = new EmailMessage(user.Email, "Confirm email", "...");
            _emailSender.Send(emailMessage);

            return Result.Success();
        }
    }

    public class RegisterUser
    {
        public string Email { get; }

        public RegisterUser(string email)
        {
            Email = email;
        }
    }
}