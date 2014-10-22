using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace SocialNetwork.ViewModels
{
    public class UserDetailMessageViewModel
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
        [Display(Name = "MessageText")]
        public string MessageText { get; set; }

    }
}