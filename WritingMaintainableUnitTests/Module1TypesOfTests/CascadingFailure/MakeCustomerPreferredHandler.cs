namespace WritingMaintainableUnitTests.Module1TypesOfTests.CascadingFailure;

public class MakeCustomerPreferredHandler
{
    private readonly AuthorizationService _authorizationService;
    private readonly ICustomerRepository _customerRepository;

    public MakeCustomerPreferredHandler(
        AuthorizationService authorizationService,
        ICustomerRepository customerRepository)
    {
        _authorizationService = authorizationService;
        _customerRepository = customerRepository;
    }

    public void Handle(MakeCustomerPreferred command)
    {
        if (!_authorizationService.IsAllowed(command.Action))
            ThrowUnauthorizedException(command.CustomerId);

        var customer = _customerRepository.Get(command.CustomerId);
        if (null == customer)
            ThrowUnknownCustomerException(command.CustomerId);

        customer.MakePreferred();
        _customerRepository.Save(customer);
    }

    private static void ThrowUnauthorizedException(int customerId)
    {
        var errorMessage = $"Not authorized to make customer (ID: {customerId}) a preferred customer.";
        throw new UnauthorizedException(errorMessage);
    }

    private static void ThrowUnknownCustomerException(int customerId)
    {
        var errorMessage = $"The customer with ID ${customerId} is not known by the system and therefore could not be made a preferred customer.";
        throw new UnknownCustomerException(errorMessage);
    }
}

public class MakeCustomerPreferred
{
    public CustomerAction Action { get; }
    public int CustomerId { get; }

    public MakeCustomerPreferred(int customerId)
    {
        Action = CustomerAction.MakePreferred;
        CustomerId = customerId;
    }
}