using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace DoorsOpen.Controllers
{
    public class ImageUploadController : Controller
    {
        private readonly IConfiguration config;

        public ImageUploadController(IConfiguration _config)
        {
            config = _config;
        }

        // GET: ImageUploadController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ImageUploadController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ImageUploadController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ImageUploadController/Create
        [HttpPost]      
        [ValidateAntiForgeryToken]
        public ActionResult Create(string picturetext, IFormFile upload)
        {
            // Azure needs your connection string like a db
            string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = config.GetValue<string>("AzureConnectionString");
            }

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            // Azure needs to know what folder you want to save in
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("dev-images");
            // Then, you can get a blob writer thing from azure


            containerClient.UploadBlob("test.jpg", upload.OpenReadStream());
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public string GetFileName(IFormFile upload)
        {
            int indexExt = 0;
            string ext;
            string imagename = null;
            if (upload != null)
            {
                indexExt = upload.FileName.IndexOf(".");
                ext = upload.FileName.Substring(indexExt);
                imagename = Guid.NewGuid() + ext;
                //uploadtoAzure(imagename, upload);
            }
            return imagename;
        }





        // GET: ImageUploadController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ImageUploadController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ImageUploadController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ImageUploadController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
