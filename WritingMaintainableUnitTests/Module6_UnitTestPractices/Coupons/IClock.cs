using System;

namespace WritingMaintainableUnitTests.Module6_UnitTestPractices.Coupons
{
    public interface IClock
    {
        DateTime GetCurrentDate();
        DateTime GetCurrentDateTime();
    }
}