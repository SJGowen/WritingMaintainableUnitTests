using System.Collections.Generic;
using System.Linq;

namespace WritingMaintainableUnitTests.Module6UnitTestPractices.Solar
{
    public class SolarPanelInstallation
    {
        public IEnumerable<SolarPanel> SolarPanels { get; }

        public SolarPanelInstallation(IEnumerable<SolarPanel> solarPanels)
        {
            SolarPanels = solarPanels;
        }

        public Watts CalculateTheoreticalCapacity()
        {
            return SolarPanels.Aggregate(Watts.Of(0), (accumulator, solarPanel)
                => accumulator + solarPanel.Capacity);
        }
    }

    public class SolarPanel
    {
        public Watts Capacity { get; }

        public SolarPanel(Watts capacity)
        {
            Capacity = capacity;
        }
    }

    public readonly struct Watts
    {
        public int Value { get; }

        private Watts(int value)
        {
            Value = value;
        }

        public static Watts Of(int value)
        {
            return new Watts(value);
        }

        public static Watts operator +(Watts a, Watts b)
        {
            return Of(a.Value + b.Value);
        }

        public override string ToString()
        {
            return $"{Value} Watts";
        }
    }
}