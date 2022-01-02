using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ViewModels
{
   public class BoxViewModel
    {
        public int Id { get; set; }
        public string TargetEmail { get; set; }
        public string SenderEmail { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string FileUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

    }
}
