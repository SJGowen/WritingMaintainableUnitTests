namespace WritingMaintainableUnitTests.Module1_TypesOfTests.CascadingFailure
{
    public class AuthorizationService
    {
        private readonly UserContext _userContext;

        public AuthorizationService(UserContext userContext)
        {
            _userContext = userContext;
        }
        
        public bool IsAllowed(CustomerAction customerAction)
        {
            if(_userContext.Role == UserRole.Unknown)
                return false;
            
            // ...
            
            return true;
        }
    }

    public enum CustomerAction
    {
        ChangeEmail = 0,
        MakePreferred = 1,
        // ...
    }
}