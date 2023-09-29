using NUnit.Framework;
using WritingMaintainableUnitTests.Module3_AnatomyOfUnitTests.ArrangeActAssert;

namespace WritingMaintainableUnitTests.Tests.Module3_AnatomyOfUnitTests._01_ArrangeActAssert
{
    [TestFixture]
    public class ApplicationRequestTests
    {
        [Test]
        public void ApplicationRequest_Create_StatusIsPending()
        {
            var sut = new ApplicationRequest("First", "Last");
            Assert.That(sut.Status, Is.EqualTo(ApplicationRequestStatus.Pending));
        }
        
        [Test]
        public void ApplicationRequest_Create_NameIsSpecified()
        {
            var sut = new ApplicationRequest("Nathaniel", "Rateliff");
            Assert.That(sut.Name, Is.EqualTo("Nathaniel Rateliff"));
        }

        [Test]
        public void PendingApplicationRequest_Approve_StatusIsApproved()
        {
            var sut = new ApplicationRequest("Dave", "Grohl");
            sut.Approve();
            Assert.That(sut.Status, Is.EqualTo(ApplicationRequestStatus.Approved));
        }

        [Test]
        public void PendingApplicationRequest_Approve_StatusIsApproved_WithStageComments()
        {
            // Arrange
            var sut = new ApplicationRequest("Dave", "Grohl");
            
            // Act
            sut.Approve();
            
            // Assert
            Assert.That(sut.Status, Is.EqualTo(ApplicationRequestStatus.Approved));
        }
    }
}