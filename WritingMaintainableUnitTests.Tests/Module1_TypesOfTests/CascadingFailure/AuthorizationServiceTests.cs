using NUnit.Framework;
using WritingMaintainableUnitTests.Module1_TypesOfTests.CascadingFailure;
// ReSharper disable InconsistentNaming

namespace WritingMaintainableUnitTests.Tests.Module1_TypesOfTests.CascadingFailure
{
    [TestFixture]
    public class When_verifying_whether_a_customer_action_is_allowed
    {        
        [Test]
        public void Then_it_should_be_allowed_for_a_user_with_a_known_role()
        {
            var helpDeskUser = new UserContext(UserRole.HelpDeskStaff);
            var sut = new AuthorizationService(helpDeskUser);

            var customerActionIsAllowed = sut.IsAllowed(CustomerAction.MakePreferred);
            Assert.That(customerActionIsAllowed);
        }
        
        [Test]
        public void Then_it_should_not_be_allowed_for_a_user_with_an_unknown_role()
        {
            var userWithUnknownRole = new UserContext(UserRole.Unknown);
            var sut = new AuthorizationService(userWithUnknownRole);

            var customerActionIsAllowed = sut.IsAllowed(CustomerAction.ChangeEmail);
            Assert.That(customerActionIsAllowed, Is.False);
        }
    }
}