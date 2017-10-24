using Model;
using Service.Interface;
using Swashbuckle.Swagger.Annotations;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using WebAPI.Filters;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/coupon")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CouponController : ApiController
    {
        private ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("couponlistbyuser")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(string), description: "Return CouponList by User")]
        public IHttpActionResult GetCouponListByUser(int userId)
        {
            var ret = _couponService.GetList(userId);

            if (ret == null)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            return Json(ret);
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("couponlist")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(string), description: "Return CouponList")]
        public IHttpActionResult GetCouponList()
        {
            var ret = _couponService.GetList();

            if (ret == null)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            return Json(ret);
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("awardedcouponlist")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(string), description: "Return Awarded CouponList")]
        public IHttpActionResult GetAwardedCouponList()
        {
            var ret = _couponService.GetAwardedCouponList();

            if (ret == null)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            return Json(ret);
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("awardedcouponlistreport")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(string), description: "Return Awarded CouponList Report")]
        public IHttpActionResult GetAwardedCouponListReport()
        {
            var ret = _couponService.GetAwardedCouponListReport();

            if (ret == null)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            return Json(ret);
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("couponlistreport")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(string), description: "Return CouponList Report")]
        public IHttpActionResult GetCouponListReport()
        {
            var ret = _couponService.GetCouponListReport();

            if (ret == null)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            return Json(ret);
        }

        [JwtAuthentication]
        [HttpPost]
        [Route("insertcoupon")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(string), description: "Insert Coupon")]
        public IHttpActionResult InsertCoupon(string code, int userId)
        {
            var ret = _couponService.InsertCoupon(code, userId);

            if (ret)
            {
                return Json(new Coupon { Code = code });
            }
            else
            {
                return Ok();
            }
        }

        [JwtAuthentication]
        [HttpPost]
        [Route("insertawardedcoupon")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(string), description: "Insert Aswarded Coupon")]
        public IHttpActionResult InsertAwardedCoupon(string code)
        {
            _couponService.InsertAwardedCoupon(code);

            return Ok();
        }
    }
}