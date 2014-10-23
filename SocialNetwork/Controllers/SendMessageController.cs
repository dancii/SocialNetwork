using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SocialNetwork.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SocialNetwork.ViewModels;

namespace SocialNetwork.Controllers
{
    [Authorize]
    public class SendMessageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SendMessage
        public ActionResult Index()
        {

            var UserNames = db.Users.Select(x => new SelectListItem { Text=x.UserName, Value=x.UserName});
            //TempData["MessageSuccess"] = "";   

            SendMessageViewModel model = new SendMessageViewModel();
            /*
            if (isSuccess)
            {
                ViewBag.Message = "Message " + TotalMessages + " was sent to " + Receiver.UserName + " at " + DateTime.Now + " succesfully!";
            }
            else
            {
                ViewBag.Message = "";
            }
            */
            //System.Diagnostics.Debug.WriteLine("Its on: " + msg);
            
            model.Users = new SelectList(UserNames,"Value","Text");

            var parameter = Request.QueryString["Message"];

            

            if (parameter != null)
            {
                model.SuccessMessage = "FHRITP";//msg;
                ViewBag.Message = parameter; // "fhritp";
                ViewBag.ReturnUrl = Url.Action("Index");
            }
            else
            {
                model.SuccessMessage = null;
                //ViewBag.Message = messageSuccess;
                //ViewBag.ReturnUrl = Url.Action("Index");
                //return View();
            }

            return View(model);
        }

        // POST: SendMessage/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "MessageSubject,MessageText, Receiver")] SendMessageViewModel message)
        {
            var Sender = db.Users.Find(User.Identity.GetUserId());

            var Receiver = db.Users.SingleOrDefault(recv => recv.UserName.Equals(message.Receiver));
            //var TotalMessages =    //.Where(u => u.Id == Sender.Id).Select(m => m);
            var TotalMessages = db.LoginInfos.Where(u => u.LoginUser.Id == Sender.Id).Select(m => m.TotalMessages);

            if (ModelState.IsValid)
            {
                Message MessageDB = new Message();

                MessageDB.MessageSubject = message.MessageSubject;
                MessageDB.MessageText = message.MessageText;
                MessageDB.MessageStatus = false;
                MessageDB.MessageTime=DateTime.Now;
                MessageDB.sender = Sender;
                MessageDB.receiver = Receiver;


                db.Messages.Add(MessageDB);
                db.SaveChanges();

                TempData["MessageSuccess"] = "FHRITP";

                var successMessage = "Message " + " was sent to " + Receiver.UserName + " at " + DateTime.Now + " succesfully!";
                return RedirectToAction("Index", new { Message = successMessage });
                //return RedirectToAction("Index");
            }

            return View(message);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
