using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SocialNetwork.Models;
using SocialNetwork.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SocialNetwork.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Messages

        public ActionResult Index()
        {
            var currentUser = db.Users.Find(User.Identity.GetUserId());

            var AllMessages = db.Messages.Where(u => u.receiver.Id == currentUser.Id).GroupBy(group => group.sender).Select(m => new MainMessageViewModel
            {
                Username = m.Key.UserName,
                noOfMessages = m.Where(k => k.MessageStatus == false).Count()
            });

            System.Diagnostics.Debug.WriteLine(AllMessages);

            return View(AllMessages);
        }

        // GET: Messages/Details/5
        public ActionResult Details(string senderUsername)
        {
            var CurrentUser = db.Users.Find(User.Identity.GetUserId());

            var SenderUser = db.Users.Where(u=> u.UserName.Equals(senderUsername)).FirstOrDefault();

            System.Diagnostics.Debug.WriteLine(SenderUser.UserName);

            if (senderUsername == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var allMessagesFromUser = db.Messages.Where(u => u.sender.Id==SenderUser.Id && u.receiver.Id==CurrentUser.Id).Select(m => new DetailMessageViewModel { 
                MessageId=m.MessageID,
                SenderUsername = m.sender.UserName,
                MessageSubject=m.MessageSubject,
                MessageTimestamp=m.MessageTime
            
            });

            System.Diagnostics.Debug.WriteLine(allMessagesFromUser);

            //Message message = db.Messages.Find(id);
            if (allMessagesFromUser == null)
            {
                return HttpNotFound();
            }
            return View(allMessagesFromUser);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        public ActionResult ReadMessage(int? id) {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Message message = db.Messages.Find(id);

            UserDetailMessageViewModel UserDetailMessageModel = new UserDetailMessageViewModel();
            UserDetailMessageModel.MessageId = message.MessageID;
            UserDetailMessageModel.SenderUsername = message.sender.UserName;
            UserDetailMessageModel.MessageText = message.MessageText;

            message.MessageStatus = true;
            db.SaveChanges();

            return View(UserDetailMessageModel);
        }


        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
            db.SaveChanges();
            return RedirectToAction("Index");
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
