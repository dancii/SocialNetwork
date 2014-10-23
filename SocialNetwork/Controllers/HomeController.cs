using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialNetwork.Models;
using SocialNetwork.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SocialNetwork.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
         
        public ActionResult Index()
        {
            //Get all user info from table UserInfo, return a viewmodel
            var currentUser = db.Users.Find(User.Identity.GetUserId());

            var currentUserInfo = db.LoginInfos.Where(i=> i.LoginUser.Id==currentUser.Id);
            UserInfoViewModel UserInfoModel = new UserInfoViewModel();

            System.Diagnostics.Debug.WriteLine(currentUserInfo.Count());

            if (currentUserInfo.Count() == 0){
                UserInfoModel.LoginCount = 0;
            }else{
                UserInfoModel.LoginCount = currentUserInfo.FirstOrDefault().LoginCount;
                UserInfoModel.LastLogin = currentUserInfo.FirstOrDefault().LastLogin;
            }


            return View(UserInfoModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}