using System;

namespace Utils.Exceptions
{
    public class CouponExceededException : CustomExceptions
    {
        private const string DefaultCode = "user_coupon_exceeded";
        public const string DefaultMessage = "Coupons Exceeded for this user";

        public CouponExceededException() : base(DefaultCode, DefaultMessage)
        {
        }

        public CouponExceededException(Exception innerException) : base(DefaultCode, DefaultMessage)
        {
        }
    }
}