using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Models
{
    public class Box
    {
        [Key]
        public int Id { get; set; }
        public string TargetEmail { get; set; }
        public string SenderEmail { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Password { get; set; }
        public string FileUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }


    }
}
