using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IFileTransferRepository
    {
        public IQueryable<Box> GetBoxes();
        public Box GetBox(int id);
        public void AddBox(Box b);

        public void DeleteBox(Box b);

        public void UpdateBox(Box b);


    }
}