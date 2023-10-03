namespace WritingMaintainableUnitTests.Module1TypesOfTests.CascadingFailure;

public class ChangeCustomerEmailHandler
{
    private readonly AuthorizationService _authorizationService;
    private readonly ICustomerRepository _customerRepository;

    public ChangeCustomerEmailHandler(
        AuthorizationService authorizationService,
        ICustomerRepository customerRepository)
    {
        _authorizationService = authorizationService;
        _customerRepository = customerRepository;
    }

    public void Handle(ChangeCustomerEmail command)
    {
        if (!_authorizationService.IsAllowed(command.Action))
            ThrowUnauthorizedException(command.CustomerId);

        var customer = _customerRepository.Get(command.CustomerId);
        if (null == customer)
            ThrowUnknownCustomerException(command.CustomerId);

        customer.ChangeEmail(command.NewEmail);
        _customerRepository.Save(customer);
    }

    private static void ThrowUnknownCustomerException(int customerId)
    {
        var errorMessage = $"The customer with ID ${customerId} is not known by the system and therefore it's email could not be changed.";
        throw new UnknownCustomerException(errorMessage);
    }

    private static void ThrowUnauthorizedException(int customerId)
    {
        var errorMessage = $"Not authorized to make customer (ID: {customerId}) a preferred customer.";
        throw new UnauthorizedException(errorMessage);
    }
}

public class ChangeCustomerEmail
{
    public CustomerAction Action { get; }
    public int CustomerId { get; }
    public string NewEmail { get; }

    public ChangeCustomerEmail(int customerId, string newEmail)
    {
        Action = CustomerAction.ChangeEmail;
        CustomerId = customerId;
        NewEmail = newEmail;
    }
}