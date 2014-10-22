using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialNetwork.ViewModels
{
    public class DetailMessageViewModel
    {

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "MessageId")]
        public int MessageId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "SenderUsername")]
        public string SenderUsername { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Subject")]
        public string MessageSubject { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Datetime")]
        public DateTime MessageTimestamp { get; set; }
    }
}