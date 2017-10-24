using Dapper;
using Data.Dapper.Interface;
using Model;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Data.Dapper.Class
{
    public class CouponRepository : BaseRepository, ICouponRepository
    {
        public void InsertCoupon(string code, int userId)
        {
            using (var db = new SqlConnection(connstring))
            {
                const string sql = @"insert into UserCoupon (Code, UserId, CreatedOn) values (@Code, @UserId, GETDATE())";

                db.Execute(sql, new { Code = code, UserId = userId }, commandType: CommandType.Text);
            }
        }

        public int GetCouponCount(int userId)
        {
            int ret;
            using (var db = new SqlConnection(connstring))
            {
                const string sql = @"select count(*) from [UserCoupon] where UserId = @UserID";

                ret = db.Query<int>(sql, new { UserId = userId }, commandType: CommandType.Text).FirstOrDefault();
            }

            return ret;
        }

        public List<UserCoupon> GetList(int userId)
        {
            List<UserCoupon> ret;
            using (var db = new SqlConnection(connstring))
            {
                const string sql = @"select
                C.Id, C.Code, C.CreatedOn,
                U.Name, U.Id, U.SurName, U.Email, U.Phone, U.LastLogon, U.CreatedOn, U.ActivationCode, U.CPF, U.Admin
                from UserCoupon C
                inner join [User] U on U.Id = C.UserId
                where U.Id = @UserID";

                ret = db.Query<UserCoupon, User, UserCoupon>(sql, (coupon, user) =>
                {
                    coupon.User = user;
                    return coupon;
                }, new { Userid = userId }, splitOn: "Name",
                 commandType: CommandType.Text).ToList();
            }

            return ret;
        }

        public List<UserCoupon> GetList()
        {
            List<UserCoupon> ret;
            using (var db = new SqlConnection(connstring))
            {
                const string sql = @"select
                C.Id, C.Code, C.CreatedOn,
                U.Name, U.SurName, U.Email, U.Phone, U.LastLogon, U.CreatedOn, U.ActivationCode, U.CPF, U.Admin
                from UserCoupon C
                inner join [User] U on U.Id = C.UserId";

                ret = db.Query<UserCoupon, User, UserCoupon>(sql, (coupon, user) =>
                    {
                        coupon.User = user;
                        return coupon;
                    }, splitOn: "Name",
                    commandType: CommandType.Text).ToList();
            }

            return ret;
        }

        public void InsertAwardedCoupon(string code)
        {
            using (var db = new SqlConnection(connstring))
            {
                const string sql = @"insert into Coupon (Code) values (@Code)";

                db.Execute(sql, new { Code = code }, commandType: CommandType.Text);
            }
        }

        public bool CheckAwardedCoupon(string code)
        {
            bool ret;
            using (var db = new SqlConnection(connstring))
            {
                const string sql = @"select Id, Code from [Coupon] where code = @Code";

                ret = db.Query<bool>(sql, new { Code = code }, commandType: CommandType.Text).Any();
            }

            return ret;
        }

        public List<Coupon> GetAwardedCouponList()
        {
            List<Coupon> ret;
            using (var db = new SqlConnection(connstring))
            {
                const string sql = @"select Id, Code from Coupon order by Code";

                ret = db.Query<Coupon>(sql, commandType: CommandType.Text).ToList();
            }

            return ret;
        }

        public List<UserCoupon> GetAwardedCouponListReport()
        {
            List<UserCoupon> ret;
            using (var db = new SqlConnection(connstring))
            {
                const string sql = @"select UC.Id, UC.Code, UC.CreatedOn,
                case WHEN C.Code is null then 'Não Premiado'
                else 'Premiado' end as [Status],
                U.Admin, U.CPF, U.CreatedOn, U.Email, U.Name, U.Surname, U.LastLogon, U.Phone
                from UserCoupon UC
                inner join [User] U on U.Id = UC.UserId
                left join Coupon C on C.Code = UC.Code
                where UC.Code in (select Code from Coupon)";

                ret = db.Query<UserCoupon, User, UserCoupon>(sql, (coupon, user) =>
                    {
                        coupon.User = user;
                        return coupon;
                    }, splitOn: "Admin",
                    commandType: CommandType.Text).ToList();
            }

            return ret;
        }

        public List<UserCoupon> GetCouponListReport()
        {
            List<UserCoupon> ret;
            using (var db = new SqlConnection(connstring))
            {
                const string sql = @"select UC.Id, UC.Code, UC.CreatedOn,
                case WHEN C.Code is null then 'Não Premiado'
                else 'Premiado' end as [Status],
                U.Admin, U.CPF, U.CreatedOn, U.Email, U.Name, U.Surname, U.LastLogon, U.Phone
                from UserCoupon UC
                inner join [User] U on U.Id = UC.UserId
                left join Coupon C on C.Code = UC.Code";

                ret = db.Query<UserCoupon, User, UserCoupon>(sql, (coupon, user) =>
                    {
                        coupon.User = user;
                        return coupon;
                    }, splitOn: "Admin",
                    commandType: CommandType.Text).ToList();
            }

            return ret;
        }

        public bool CheckExistingCoupon(string code)
        {
            bool ret;
            using (var db = new SqlConnection(connstring))
            {
                const string sql = @"select Id, Code from [UserCoupon] where code = @Code";

                ret = db.Query<bool>(sql, new { Code = code }, commandType: CommandType.Text).Any();
            }

            return ret;
        }

        public bool CheckExistingAwardedCoupon(string code)
        {
            bool ret;
            using (var db = new SqlConnection(connstring))
            {
                const string sql = @"select Id, Code from [Coupon] where code = @Code";

                ret = db.Query<bool>(sql, new { Code = code }, commandType: CommandType.Text).Any();
            }

            return ret;
        }
    }
}