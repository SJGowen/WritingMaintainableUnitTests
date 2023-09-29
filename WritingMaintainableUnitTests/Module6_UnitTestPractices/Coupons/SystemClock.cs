using System;

namespace WritingMaintainableUnitTests.Module6_UnitTestPractices.Coupons
{
    public class SystemClock : IClock
    {
        public DateTime GetCurrentDate()
        {
            return DateTime.Today;
        }

        public DateTime GetCurrentDateTime()
        {
            return DateTime.UtcNow;
        }
    }
}