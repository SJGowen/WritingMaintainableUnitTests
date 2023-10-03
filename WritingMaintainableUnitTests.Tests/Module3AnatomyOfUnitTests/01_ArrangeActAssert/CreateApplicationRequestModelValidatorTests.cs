using NUnit.Framework;
using WritingMaintainableUnitTests.Module3AnatomyOfUnitTests.ArrangeActAssert;

namespace WritingMaintainableUnitTests.Tests.Module3AnatomyOfUnitTests._01_ArrangeActAssert;

public class CreateApplicationRequestValidatorTests
{        
    [Test]
    public void ValidateModel_WithoutFirstName_IsInvalid()
    {
        var modelWithoutFirstName = new CreateApplicationRequestModel { LastName = "Vedder" };
        var isValid = CreateApplicationRequestModelValidator.IsValid(modelWithoutFirstName);
        Assert.That(isValid, Is.False);
    }
    
    [Test]
    public void ValidateModel_WithoutLastName_IsInvalid()
    {
        var modelWithoutLastName = new CreateApplicationRequestModel { FirstName = "Eddie" };
        var isValid = CreateApplicationRequestModelValidator.IsValid(modelWithoutLastName);
        Assert.That(isValid, Is.False);
    }
    
    [Test]
    public void ValidateModel_WithAllDataProvided_IsValid()
    {
        var validModel = new CreateApplicationRequestModel { FirstName = "Eddie", LastName = "Vedder" };
        var isValid = CreateApplicationRequestModelValidator.IsValid(validModel);
        Assert.That(isValid);
    }
        
    [Test]
    public void ValidateModel_WithAllDataProvided_IsValid_WithStageComments()
    {
        // Arrange
        var validModel = new CreateApplicationRequestModel { FirstName = "Eddie", LastName = "Vedder" };
        
        // Act
        var isValid = CreateApplicationRequestModelValidator.IsValid(validModel);
        
        // Assert
        Assert.That(isValid);
    }
}