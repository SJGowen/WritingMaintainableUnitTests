namespace WritingMaintainableUnitTests.Module1_TypesOfTests.BehaviourVerification;

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
    
    public void Handle(RegisterUser command)
    {
        var user = new User(command.Email);
        _userRepository.Save(user);
        
        var emailMessage = new EmailMessage(user.Email, "Confirm email", "...");
        _emailSender.Send(emailMessage);
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