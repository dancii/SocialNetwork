using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialNetwork.Models
{
    public class Message
    {
        [Key]
        public int MessageID { get; set; }
        [Required]
        public string MessageSubject { get; set; }
        [Required]
        public string MessageText { get; set; }
        [Required]
        public DateTime MessageTime { get; set; }
        [Required]
        public Boolean MessageStatus { get; set; }

        public virtual ApplicationUser sender { get; set; }
        public virtual ApplicationUser receiver { get; set; }

    }

    public class SendMessageViewModel {

        [Required]
        [DataType(DataType.Text)]
        [Display(Name="Subject")]
        public string MessageSubject { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "MessageText")]
        public string MessageText { get; set; }

        [Required]
        [Display(Name = "Receiver")]
        public string Receiver { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }

    }
}