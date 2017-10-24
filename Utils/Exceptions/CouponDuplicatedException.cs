using System;

namespace Utils.Exceptions
{
    public class CouponDuplicatedException : CustomExceptions
    {
        private const string DefaultCode = "coupon_duplicated";
        public const string DefaultMessage = "This coupon already in the database";

        public CouponDuplicatedException() : base(DefaultCode, DefaultMessage)
        {
        }

        public CouponDuplicatedException(Exception innerException) : base(DefaultCode, DefaultMessage)
        {
        }
    }
}