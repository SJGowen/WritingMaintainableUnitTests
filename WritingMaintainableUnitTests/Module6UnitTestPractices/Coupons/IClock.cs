using System;

namespace WritingMaintainableUnitTests.Module6UnitTestPractices.Coupons;

public interface IClock
{
    DateTime GetCurrentDate();
    DateTime GetCurrentDateTime();
}