using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.ViewModels
{
    public class AddBoxViewModel
    {
        [Required]
        public string TargetEmail { get; set; }
        [Required]
        public string SenderEmail { get; set; }
        [Required]
        public string Title { get; set; }
        public string Message { get; set; }
        public string Password { get; set; }
        [Required]
        public string FileUrl { get; set; }


    }
}