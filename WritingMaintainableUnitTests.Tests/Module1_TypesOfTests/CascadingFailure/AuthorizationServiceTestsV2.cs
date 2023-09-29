using NUnit.Framework;
using WritingMaintainableUnitTests.Module1_TypesOfTests.CascadingFailure;
// ReSharper disable InconsistentNaming

namespace WritingMaintainableUnitTests.Tests.Module1_TypesOfTests.CascadingFailure
{
    [TestFixture]
    public class When_verifying_whether_a_customer_action_is_allowed_V2
    {        
        [Test]
        public void Then_it_should_be_allowed_for_a_user_with_a_known_role_to_change_email_of_a_customer()
        {
            var helpDeskUser = new UserContext(UserRole.HelpDeskStaff);
            var sut = new AuthorizationServiceV2(helpDeskUser);

            var changeCustomerEmailIsAllowed = sut.IsAllowed(CustomerAction.ChangeEmail);
            Assert.That(changeCustomerEmailIsAllowed);
        }
        
        [Test]
        public void Then_it_should_not_be_allowed_for_a_helpdesk_user_to_make_a_customer_preferred()
        {
            var helpDeskUser = new UserContext(UserRole.HelpDeskStaff);
            var sut = new AuthorizationServiceV2(helpDeskUser);

            var makeCustomerPreferredIsAllowed = sut.IsAllowed(CustomerAction.MakePreferred);
            Assert.That(makeCustomerPreferredIsAllowed, Is.False);
        }
        
        [Test]
        public void Then_it_should_not_be_allowed_for_a_user_with_an_unknown_role()
        {
            var userWithUnknownRole = new UserContext(UserRole.Unknown);
            var sut = new AuthorizationServiceV2(userWithUnknownRole);

            var customerActionIsAllowed = sut.IsAllowed(CustomerAction.ChangeEmail);
            Assert.That(customerActionIsAllowed, Is.False);
        }
    }
}