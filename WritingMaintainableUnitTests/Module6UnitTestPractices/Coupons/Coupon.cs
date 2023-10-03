using System;

namespace WritingMaintainableUnitTests.Module6UnitTestPractices.Coupons
{
    public class Coupon
    {
        private readonly string _code;
        private readonly DateTime _creationDate;

        public Coupon(string code, DateTime creationDate)
        {
            _code = code;
            _creationDate = creationDate;
        }

        #region Original version

        public bool IsApplicableOn(DateTime date)
        {
            var oneMonthAfterCreationDate = _creationDate.AddMonths(1);
            return date <= oneMonthAfterCreationDate;
        }

        #endregion

        #region Improved version

        public bool IsApplicableOnV2(DateTime date)
        {
            var oneMonthAfterCreationDate = AddFullMonths(_creationDate, 1);
            return date <= oneMonthAfterCreationDate;
        }

        private static DateTime AddFullMonths(DateTime date, int months)
        {
            if (date.Day != DateTime.DaysInMonth(date.Year, date.Month))
                return date.AddMonths(months);

            return date.AddDays(1).AddMonths(months).AddDays(-1);
        }

        #endregion
    }
}