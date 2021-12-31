using System;
using System.Collections.Generic;
using System.Text;
using Data.Context;
using Domain.Interfaces;
using Domain.Models;
using System.Linq;

namespace Data.Repositories
{
    public class FileTransferRepository : IFileTransferRepository
    {
        private FileTransferContext context;
        public FileTransferRepository(FileTransferContext _context)
        {
            context = _context;
        }

        public void AddBox(Box b)
        {
            context.Boxes.Add(b);
            context.SaveChanges();
        }

        public void DeleteBox(Box b)
        {
            context.Boxes.Remove(b);
            context.SaveChanges();
        }

        public Box GetBox(int id)
        {
            return context.Boxes.SingleOrDefault(b => b.Id == id);
        }

        public IQueryable<Box> GetBoxes()
        {
            return context.Boxes;
        }

        public void UpdateBox(Box b)
        {
            var originalBox = GetBox(b.Id);
            originalBox.TargetEmail = b.TargetEmail;
            originalBox.SenderEmail = b.SenderEmail;
            originalBox.Title = b.Title;
            originalBox.Message = b.Message;
            originalBox.Password = b.Password;
            originalBox.DateUpdated = DateTime.Now; 
            context.SaveChanges();
        }
    }
}
