using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FileSharingApplication.Controllers
{
    public class FileTransferController : Controller
    {
        private IFileTransferService transferService;
        private IWebHostEnvironment hostEnv;
        private ILogger<FileTransferController> _logger;

        public FileTransferController(IFileTransferService transferService, IWebHostEnvironment hostEnv, ILogger<FileTransferController> logger)
        {
            this.transferService = transferService;
            this.hostEnv = hostEnv;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var list = transferService.GetBoxes();
            return View();
        }

        [HttpPost]
        public IActionResult Create(AddBoxViewModel model, IFormFile logoFile)
        {
            _logger.Log(LogLevel.Information, $"{User.Identity.Name} is uploading a file called {logoFile.FileName}");

            try
            {
                if (ModelState.IsValid)
                {
                    if (logoFile != null)
                    {
                       
                        string newFilename = Guid.NewGuid() + Path.GetExtension(logoFile.FileName);
                        _logger.Log(LogLevel.Information, $"New filename {newFilename} was generated for the file being uploaded by user {User.Identity.Name}");

                        string absolutePath = hostEnv.WebRootPath + "\\Files";
                        _logger.Log(LogLevel.Information, $"{User.Identity.Name} is about to start saving file at {absolutePath}");

                        string absolutePathWithFilename = absolutePath + "\\" + newFilename;
                        model.FileUrl = "\\Files\\" + newFilename;
                       

                        using (FileStream fs = new FileStream(absolutePathWithFilename, FileMode.CreateNew, FileAccess.Write))
                        {
                            logoFile.CopyTo(fs);
                            fs.Close();
                        }
                        _logger.Log(LogLevel.Information, $"{newFilename} has been saved successfully at {absolutePath}");

                    }


                    transferService.AddBox(model);
                    ViewBag.Message = "Transfer Box added successfully";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Transfer Box wasn't added successfully";
            }

            return View();
        }

        public IActionResult Details(int id)
        {
            var box = transferService.GetBox(id);
            return View(box);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                transferService.DeleteBox(id);
                TempData["Message"] = "Transfer Box was deleted successfully";

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Transfer Box wasnt deleted. Error: " + ex.Message;

            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var blogToBeEdited = transferService.GetBox(id);
            return View(blogToBeEdited);
        }

        public IActionResult Update(BoxViewModel model, IFormFile logoFile)
        {
            try
            {
                var originalBox = transferService.GetBox(model.Id);

                if (ModelState.IsValid)
                {
                    if (logoFile != null)
                    {
                        //deleting only if there is a new image uploading
                        System.IO.File.Delete(hostEnv.WebRootPath + originalBox.FileUrl);

                        //1. to generate a new unique filename
                        //5389205C-813B-4AFA-A453-B912C30BF933.jpg
                        string newFilename = Guid.NewGuid() + Path.GetExtension(logoFile.FileName);

                        //2. find what the absolute path to the folder Files is
                        //C:\Users\attar\Source\Repos\SWD62BEP2021v2\Presentation\Files\5389205C-813B-4AFA-A453-B912C30BF933.jpg

                        //hostEnv.ContentRootPath : C:\Users\attar\Source\Repos\SWD62BEP2021v2\Presentation
                        //hostEnv.WebRootPath:  C:\Users\attar\Source\Repos\SWD62BEP2021v2\Presentation\wwwroot

                        string absolutePath = hostEnv.WebRootPath + "\\Files";
                        string absolutePathWithFilename = absolutePath + "\\" + newFilename;
                        model.FileUrl = "\\Files\\" + newFilename;
                        //3. do the transfer/saving of the actual physical file

                        using (FileStream fs = new FileStream(absolutePathWithFilename, FileMode.CreateNew, FileAccess.Write))
                        {
                            logoFile.CopyTo(fs);
                            fs.Close();
                        }
                    }

                    AddBoxViewModel myModel = new AddBoxViewModel();
                    myModel.TargetEmail = model.TargetEmail;
                    myModel.SenderEmail = model.SenderEmail;
                    myModel.Title = model.Title;
                    myModel.Message = model.Message;
                    myModel.FileUrl = model.FileUrl;

                   

                    //category is not being updated since i did not include the select in page

                    transferService.UpdateBox(myModel, model.Id);
                    ViewBag.Message = "Transfer Box updated successfully";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Transfer Box wasn't updated successfully";
            }

            return View();
        }
    }
}
