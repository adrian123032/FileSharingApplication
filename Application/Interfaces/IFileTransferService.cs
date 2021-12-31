using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Application.ViewModels;

namespace Application.Interfaces
{
    public interface IFileTransferService
    {
        public IQueryable<BoxViewModel> GetBoxes();
        public BoxViewModel GetBox(int id);
        public void AddBox(AddBoxViewModel model);

        public void DeleteBox(int id);

        public void UpdateBox(AddBoxViewModel editedDetails, int id);
    }
}

