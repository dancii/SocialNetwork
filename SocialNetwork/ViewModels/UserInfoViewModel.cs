using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetwork.ViewModels
{
    public class UserInfoViewModel
    {
        public int LoginCount { get; set; }
        public DateTime LastLogin { get; set; }
        public int MessageUnreadCount { get; set; }

    }
}