using WritingMaintainableUnitTests.Module6_UnitTestPractices.Solar;
using WritingMaintainableUnitTests.Tests.Common;

// ReSharper disable InconsistentNaming

namespace WritingMaintainableUnitTests.Tests.Module6_UnitTestPractices._05_DomainKnowledge
{
    [Specification]
    public class When_calculating_the_theoretical_capacity_of_a_solar_panels_installation__improved
    {
        [Establish]
        public void Context()
        {
            var solarPanels = new[]
            {
                new SolarPanel(Watts.Of(368)), 
                new SolarPanel(Watts.Of(368)), 
                new SolarPanel(Watts.Of(278)) 
            };
            
            _sut = new SolarPanelInstallation(solarPanels);
        }
        
        [Because]
        public void Of()
        {
            _theoreticalCapacity = _sut.CalculateTheoreticalCapacity();
        }    
        
        [Observation]
        public void Then_it_should_yield_the_total_capacity_of_all_solar_panels_of_the_installation()
        {
            _theoreticalCapacity.Should_be_equal_to(Watts.Of(1014));
        }

        private SolarPanelInstallation _sut;
        private Watts _theoreticalCapacity;
    }
}