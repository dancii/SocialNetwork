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
    public class SendMessageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SendMessage
        public ActionResult Index()
        {

            var UserNames = db.Users.Select(x => new SelectListItem { Text=x.UserName, Value=x.UserName});

            SendMessageViewModel model = new SendMessageViewModel();

            model.Users = new SelectList(UserNames,"Value","Text");

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


                return RedirectToAction("Index");
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
