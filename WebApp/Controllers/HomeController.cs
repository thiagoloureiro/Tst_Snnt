using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public const string APIUrl = "http://thiagodev.net/webapi/";

        public ActionResult Index()
        {
            Session["User"] = null;
            return View();
        }

        private bool UserLogin(string username, string password)
        {
            var client = new HttpClient { BaseAddress = new Uri(APIUrl) };

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.GetAsync($"api/user/login?username={username}&password={password}").Result;

            if (response.IsSuccessStatusCode)
            {
                var ret = response.Content.ReadAsStringAsync();
                var userObj = JsonConvert.DeserializeObject<User>(ret.Result);

                Session["User"] = userObj;
                Session["UserName"] = userObj.Name;
                Session["Admin"] = userObj.Admin;
                return true;
            }
            return false;
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (UserLogin(username, password))
            {
                return RefreshGrid();
            }
            else return View("Invalid");
        }

        public ActionResult Register()
        {
            if (Session["User"] is null)
            {
                return View("Index");
            }
            else
            {
                var model = GetUserCoupons();
                return View("Register", model);
            }
        }

        public ActionResult RegisterAwarded()
        {
            if (Session["User"] is null)
            {
                return View("Index");
            }
            else
            {
                var model = GetAwardedCoupons();
                return View("RegisterAwarded", model);
            }
        }

        public ActionResult UserList()
        {
            if (Session["User"] is null)
            {
                return View("Index");
            }
            else
            {
                var model = GetUserList();
                return View("UserList", model);
            }
        }

        public ActionResult CupomListReport()
        {
            if (Session["User"] is null)
            {
                return View("Index");
            }
            else
            {
                var model = GetCupomListReport();
                return View("CupomListReport", model);
            }
        }

        public ActionResult AwardedCupomListReport()
        {
            if (Session["User"] is null)
            {
                return View("Index");
            }
            else
            {
                var model = GetAwardedCupomListReport();
                return View("AwardedCupomListReport", model);
            }
        }

        private ActionResult RefreshGrid()
        {
            var model = GetUserCoupons();

            return RedirectToAction("Register", model);
        }

        private ActionResult RefreshGridAwardedCode()
        {
            var model = GetAwardedCoupons();

            return RedirectToAction("RegisterAwarded", model);
        }

        [HttpPost]
        public ActionResult RegisterCode(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                var ret = RegisterCustomerCode(code);
                return RefreshGrid();
            }
            else
            {
                TempData["notice"] = "Por favor preencha o campo Cupom";
                return RefreshGrid();
            }
        }

        [HttpPost]
        public ActionResult RegisterAwardedCode(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                RegisterAwardedCustomerCode(code);

                return RefreshGridAwardedCode();
            }
            else
            {
                TempData["notice"] = "Por favor preencha o campo Cupom";
                return RefreshGridAwardedCode();
            }
        }

        #region EndPoint Calls

        private bool RegisterAwardedCustomerCode(string code)
        {
            var client = new HttpClient { BaseAddress = new Uri(APIUrl) };

            if (Session["User"] != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());

                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.PostAsync($"api/coupon/insertawardedcoupon?code={code}", null).Result;

                var contents = response.Content.ReadAsStringAsync();

                if (contents.Result.Contains("CouponDuplicatedException"))
                {
                    TempData["notice"] = "Cupom duplicado!";
                    return false;
                }

                if (response.IsSuccessStatusCode)
                {
                    TempData["notice"] = "Cupom premiado cadastrado com sucesso !";
                }
            }
            return false;
        }

        private bool RegisterCustomerCode(string code)
        {
            var client = new HttpClient { BaseAddress = new Uri(APIUrl) };

            if (Session["User"] != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());

                int userId = GetUserId();

                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.PostAsync($"api/coupon/insertcoupon?code={code}&userId={userId}", null).Result;

                var contents = response.Content.ReadAsStringAsync();

                if (contents.Result.Contains("CouponDuplicatedException"))
                {
                    TempData["notice"] = "Cupom duplicado!";
                    return false;
                }

                if (contents.Result.Contains("CouponExceededException"))
                {
                    TempData["notice"] = "É permitido no máximo 05 cupons por CPF, boa sorte !";
                    return false;
                }

                if (response.IsSuccessStatusCode)
                {
                    var ret = response.Content.ReadAsStringAsync();
                    var couponObj = JsonConvert.DeserializeObject<Coupon>(ret.Result);

                    if (couponObj != null)
                        TempData["notice"] = "Parabéns! Seu cupom é premiado !";
                    else
                        TempData["notice"] = "Cupom cadastrado com sucesso, boa sorte !";

                    return true;
                }
            }
            return false;
        }

        private List<UserCoupon> GetUserCoupons()
        {
            var client = new HttpClient { BaseAddress = new Uri(APIUrl) };

            if (Session["User"] != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());

                int userId = GetUserId();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync($"api/coupon/couponlistbyuser?UserId={userId}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var ret = response.Content.ReadAsStringAsync();
                    var list = JsonConvert.DeserializeObject<List<UserCoupon>>(ret.Result);
                    return list;
                }
            }
            return null;
        }

        private List<Coupon> GetAwardedCoupons()
        {
            var client = new HttpClient { BaseAddress = new Uri(APIUrl) };

            if (Session["User"] != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());

                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync($"api/coupon/awardedcouponlist").Result;

                if (response.IsSuccessStatusCode)
                {
                    var ret = response.Content.ReadAsStringAsync();
                    var list = JsonConvert.DeserializeObject<List<Coupon>>(ret.Result);
                    return list;
                }
            }
            return null;
        }

        private List<User> GetUserList()
        {
            var client = new HttpClient { BaseAddress = new Uri(APIUrl) };

            if (Session["User"] != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());

                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync($"api/user/userlist").Result;

                if (response.IsSuccessStatusCode)
                {
                    var ret = response.Content.ReadAsStringAsync();
                    var list = JsonConvert.DeserializeObject<List<User>>(ret.Result);
                    return list;
                }
            }
            return null;
        }

        private List<UserCoupon> GetCupomListReport()
        {
            var client = new HttpClient { BaseAddress = new Uri(APIUrl) };

            if (Session["User"] != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());

                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync("api/coupon/couponlistreport").Result;

                if (response.IsSuccessStatusCode)
                {
                    var ret = response.Content.ReadAsStringAsync();
                    var list = JsonConvert.DeserializeObject<List<UserCoupon>>(ret.Result);
                    return list;
                }
            }
            return null;
        }

        private List<UserCoupon> GetAwardedCupomListReport()
        {
            var client = new HttpClient { BaseAddress = new Uri(APIUrl) };

            if (Session["User"] != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());

                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync("api/coupon/awardedcouponlistreport").Result;

                if (response.IsSuccessStatusCode)
                {
                    var ret = response.Content.ReadAsStringAsync();
                    var list = JsonConvert.DeserializeObject<List<UserCoupon>>(ret.Result);
                    return list;
                }
            }
            return null;
        }

        private int GetUserId()
        {
            int userId = ((User)Session["User"]).Id;
            return userId;
        }

        private string GetToken()
        {
            string token = ((User)Session["User"]).Token;
            return token;
        }

        #endregion EndPoint Calls
    }
}