
using Application.Interfaces;
using Application.ViewModels;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Services
{
    public class FileTransferService : IFileTransferService
    {
        private IFileTransferRepository transfersRepo;

        public FileTransferService(IFileTransferRepository _transfersRepo)
        {
            transfersRepo = _transfersRepo;
        }

        public void AddBox(AddBoxViewModel model)
        {
            transfersRepo.AddBox(
                new Domain.Models.Box()
                {
                    TargetEmail = model.TargetEmail,
                    SenderEmail = model.SenderEmail,
                    Title = model.Title,
                    Message = model.Message,
                    Password = model.Password,
                    FileUrl = model.FileUrl,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                });
        }

        public void DeleteBox(int id)
        {
            var box = transfersRepo.GetBox(id);
            if (box == null)
                throw new Exception("The Box does not exist");
            else
            {
                transfersRepo.DeleteBox(box);
            }
        }

        public BoxViewModel GetBox(int id)
        {
            var box = transfersRepo.GetBox(id);
            var result = new BoxViewModel()
            {
                Id = box.Id,
                TargetEmail = box.TargetEmail,
                SenderEmail = box.SenderEmail,
                Title = box.Title,
                Message = box.Message,
                Password = box.Password,
                FileUrl = box.FileUrl,
                DateCreated = box.DateCreated,
                DateUpdated = box.DateUpdated
            };
            return result;
        }

        public IQueryable<BoxViewModel> GetBoxes()
        {
            var list = from b in transfersRepo.GetBoxes() //List<Blog>
                       select new BoxViewModel()
                       {
                           Id = b.Id,
                           TargetEmail = b.TargetEmail,
                           SenderEmail = b.SenderEmail,
                           Title = b.Title,
                           Message = b.Message,
                           Password = b.Password,
                           FileUrl = b.FileUrl,
                           DateCreated = b.DateCreated,
                           DateUpdated = b.DateUpdated
                       };
            return list;
        }

        public void UpdateBox(AddBoxViewModel editedDetails, int id)
        {
            var originalBox = transfersRepo.GetBox(id);
            originalBox.TargetEmail = editedDetails.TargetEmail;
            originalBox.SenderEmail = editedDetails.SenderEmail;
            originalBox.Title = editedDetails.Title;
            originalBox.Message = editedDetails.Message;
            originalBox.Password = editedDetails.Password;
            originalBox.FileUrl = editedDetails.FileUrl;


            transfersRepo.UpdateBox(originalBox);
        }
    }
}
