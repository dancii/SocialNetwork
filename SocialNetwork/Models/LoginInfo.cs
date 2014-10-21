using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialNetwork.Models
{
    public class LoginInfo
    {
        [Key]
        public int LoginInfoID { get; set; }
        public int LoginCount { get; set; }
        public DateTime LastLogin { get; set; }

        public virtual ApplicationUser LoginUser { get; set; }
    }
}