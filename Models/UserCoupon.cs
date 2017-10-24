using System;

namespace Model
{
    public class UserCoupon
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime CreatedOn { get; set; }
        public User User { get; set; }
        public string Status { get; set; }
    }
}