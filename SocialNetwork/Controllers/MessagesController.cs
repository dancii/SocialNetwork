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
                noOfMessages = m.Where(k => k.MessageStatus == false).Count(),
                noOfReadMessages=m.Where(k=> k.MessageStatus == true).Count(),
                noOfDeletedMessages=db.LoginInfos.Where(lu => lu.LoginUser.Id == currentUser.Id).Select(dm => dm.DeletedMessages).FirstOrDefault(),
                noOfTotalMessages = m.Where(k => k.receiver.Id == currentUser.Id).Count()
            });

            

            if (AllMessages.Any())
            {
                System.Diagnostics.Debug.WriteLine("EXIST");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Sup");

                AllMessages = db.LoginInfos.Where(u => u.LoginUser.Id == currentUser.Id).Select(m => new MainMessageViewModel { 
                    noOfDeletedMessages=m.DeletedMessages,
                    noOfMessages=0,
                    noOfReadMessages=0,
                    noOfTotalMessages=0
                });
                //AllMessages.First().noOfDeletedMessages = db.LoginInfos.Where(lu => lu.LoginUser.Id == currentUser.Id).Select(dm => dm.DeletedMessages).FirstOrDefault();
                //AllMessages.First().noOfReadMessages = db.Messages.Where(m => m.receiver.Id == currentUser.Id && m.MessageStatus == true).Count();
                //AllMessages.First().noOfTotalMessages = db.Messages.Where(t => t.receiver.Id == currentUser.Id).Count();

            }
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

            var CurrentUser = db.Users.Find(User.Identity.GetUserId());

            var currentUserInfo = db.LoginInfos.Where(i => i.LoginUser.Id == CurrentUser.Id);

            UserInfo loginInfo = null;

            if (currentUserInfo.Count() == 0)
            {
                loginInfo = new UserInfo();
                loginInfo.DeletedMessages += 1;
                db.LoginInfos.Add(loginInfo);
            }
            else
            {
                loginInfo = db.LoginInfos.Find(currentUserInfo.SingleOrDefault().LoginInfoID);
                db.LoginInfos.Attach(loginInfo);
                loginInfo.DeletedMessages += 1;
            }

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
