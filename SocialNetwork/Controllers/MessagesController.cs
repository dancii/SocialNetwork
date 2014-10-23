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

            //Get all messages, groupby user. Get all Deleted, read and total messages
            var currentUser = db.Users.Find(User.Identity.GetUserId());

            var AllMessages = db.Messages.Where(u => u.receiver.Id == currentUser.Id).GroupBy(group => group.sender).Select(m => new MainMessageViewModel
            {
                Username = m.Key.UserName,
                noOfMessages = m.Where(k => k.MessageStatus == false).Count(),
                noOfReadMessages=m.Where(k=> k.MessageStatus == true).Count(),
                noOfDeletedMessages=db.LoginInfos.Where(lu => lu.LoginUser.Id == currentUser.Id).Select(dm => dm.DeletedMessages).FirstOrDefault(),
                noOfTotalMessages = m.Where(k => k.receiver.Id == currentUser.Id).Count()
            });

            
            // If there is not any messages to send then send only the total deleted count
            if (AllMessages.Any())
            {

            }
            else
            {
                AllMessages = db.LoginInfos.Where(u => u.LoginUser.Id == currentUser.Id).Select(m => new MainMessageViewModel { 
                    noOfDeletedMessages=m.DeletedMessages,
                    noOfMessages=0,
                    noOfReadMessages=0,
                    noOfTotalMessages=0
                });

            }
            return View(AllMessages);
        }

        // GET: Messages/Details/5
        public ActionResult Details(string senderUsername)
        {

            //Get details about all messages from a user
            var CurrentUser = db.Users.Find(User.Identity.GetUserId());

            var SenderUser = db.Users.Where(u=> u.UserName.Equals(senderUsername)).FirstOrDefault();

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


            //read a message from a user
            Message message = db.Messages.Find(id);

            UserDetailMessageViewModel UserDetailMessageModel = new UserDetailMessageViewModel();
            UserDetailMessageModel.MessageId = message.MessageID;
            UserDetailMessageModel.SenderUsername = message.sender.UserName;
            UserDetailMessageModel.MessageText = message.MessageText;

            message.MessageStatus = true;
            db.SaveChanges();

            return View(UserDetailMessageModel);
        }


        // Delete a message 
        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.Messages.Find(id);

            var CurrentUser = db.Users.Find(User.Identity.GetUserId());

            var currentUserInfo = db.LoginInfos.Where(i => i.LoginUser.Id == CurrentUser.Id);

            UserInfo loginInfo = null;

            //Adds to deleted messages

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
