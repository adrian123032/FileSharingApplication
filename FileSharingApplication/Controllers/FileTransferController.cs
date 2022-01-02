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
using System.Text;
using System.Windows;

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
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AddBoxViewModel model, IFormFile File)
        {
            

            try
            {
                _logger.Log(LogLevel.Information, $"{User.Identity.Name} is uploading a file called {File.FileName}");
                if (ModelState.IsValid)
                {
                    if (File != null)
                    {
                       
                        string newFilename = Guid.NewGuid() + Path.GetExtension(File.FileName);
                       _logger.Log(LogLevel.Information, $"New filename {newFilename} was generated for the file being uploaded by user {User.Identity.Name}");

                        string absolutePath = hostEnv.WebRootPath + "\\Files";
                       _logger.Log(LogLevel.Information, $"{User.Identity.Name} is about to start saving file at {absolutePath}");

                        string absolutePathWithFilename = absolutePath + "\\" + newFilename;
                        model.FileUrl = "\\Files\\" + newFilename;
                       

                        using (FileStream fs = new FileStream(absolutePathWithFilename, FileMode.CreateNew, FileAccess.Write))
                        {
                            File.CopyTo(fs);
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
        public async Task<IActionResult> Download(int id)
        {
            var box = transferService.GetBox(id);
            var dpath = hostEnv.WebRootPath + box.FileUrl;
            var memory = new MemoryStream();
            using (var stream = new FileStream(dpath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var ext = Path.GetExtension(dpath).ToLowerInvariant();
            return File(memory, GetMimeTypes()[ext], Path.GetFileName(dpath));
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain" },
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms.excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.speadsheet.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
               {".gif", "image/gif"},
               {".csv", "text/csv"}

            };
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
            var boxToBeEdited = transferService.GetBox(id);
            return View(boxToBeEdited);
        }

        public IActionResult Update(BoxViewModel model, IFormFile File)
        {
            try
            {
                var originalBox = transferService.GetBox(model.Id);

                if (ModelState.IsValid)
                {
                    if (File != null)
                    {
                        System.IO.File.Delete(hostEnv.WebRootPath + originalBox.FileUrl);


                        string newFilename = Guid.NewGuid() + Path.GetExtension(File.FileName);


                        string absolutePath = hostEnv.WebRootPath + "\\Files";
                        string absolutePathWithFilename = absolutePath + "\\" + newFilename;
                        model.FileUrl = "\\Files\\" + newFilename;
                        //3. do the transfer/saving of the actual physical file

                        using (FileStream fs = new FileStream(absolutePathWithFilename, FileMode.CreateNew, FileAccess.Write))
                        {
                            File.CopyTo(fs);
                            fs.Close();
                        }
                    }

                    AddBoxViewModel myModel = new AddBoxViewModel();
                    myModel.TargetEmail = model.TargetEmail;
                    myModel.SenderEmail = model.SenderEmail;
                    myModel.Title = model.Title;
                    myModel.Message = model.Message;
                    myModel.Password = model.Password;
                    myModel.FileUrl = model.FileUrl;



                    transferService.UpdateBox(myModel, model.Id);
                    ViewBag.Message = "Transfer Box updated successfully";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Transfer Box wasn't updated successfully";
            }

            return RedirectToAction("Index");
        }
    }
}
