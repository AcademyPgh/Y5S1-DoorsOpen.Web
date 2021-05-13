using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoorsOpen.Data;
using DoorsOpen.Models;
using Microsoft.AspNetCore.Http;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace DoorsOpen.Controllers
{
    public class BuildingModelsController : Controller
    {
        private readonly SiteDbContext _context;
        private readonly IConfiguration _config;

        public BuildingModelsController(SiteDbContext context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }

        // GET: BuildingModels
        public async Task<IActionResult> Index()
        {
            
            return View(await _context.Buildings.ToListAsync());
        }

        // GET: BuildingModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buildingModel = await _context.Buildings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (buildingModel == null)
            {
                return NotFound();
            }

            return View(buildingModel);
        }

        // GET: BuildingModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BuildingModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Building,Address1,Address2,City,State,Zip,WheelchairAccessible,RestroomsAvailable,WheelchairAccessibleRestroom,PhotographyAllowed,StartTime,EndTime,Capacity,HistoricalOverview,VisitorExperience,Image")] BuildingModel buildingModel, IFormFile upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null)
                {
                    string imageName = GetFileName(upload);
                    buildingModel.Image = imageName;
                    UploadToAzure(imageName, upload);
                }
                
                _context.Add(buildingModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(buildingModel);
        }

        // GET: BuildingModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buildingModel = await _context.Buildings.FindAsync(id);
            if (buildingModel == null)
            {
                return NotFound();
            }
            return View(buildingModel);
        }

        // POST: BuildingModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Building,Address1,Address2,City,State,Zip,WheelchairAccessible,RestroomsAvailable,WheelchairAccessibleRestroom,PhotographyAllowed,StartTime,EndTime,Capacity,HistoricalOverview,VisitorExperience,Image")] BuildingModel buildingModel, IFormFile upload)
        {
            if (id != buildingModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var buildingToEdit = await _context.Buildings.Where(b => b.Id == buildingModel.Id).FirstOrDefaultAsync();

                var deleteImage = false;
                if (buildingToEdit.Image != buildingModel.Image || upload != null)
                {
                    if (!string.IsNullOrEmpty(buildingModel.Image))
                    {
                        deleteImage = true;
                    }
                }

                if (deleteImage)
                {
                    if (!string.IsNullOrEmpty(buildingModel.Image))
                    {
                        DeleteFromAzure(buildingModel.Image);
                        buildingModel.Image = null;
                    }
                }

                if (upload != null)
                {
                    string imageName = GetFileName(upload);
                    buildingModel.Image = imageName;
                    UploadToAzure(imageName, upload);
                    Console.WriteLine(buildingModel.Image);
                }

                buildingToEdit.Building = buildingModel.Building;
                buildingToEdit.Address1 = buildingModel.Address1;
                buildingToEdit.Address2 = buildingModel.Address2;
                buildingToEdit.City = buildingModel.City;
                buildingToEdit.State = buildingModel.State;
                buildingToEdit.Zip = buildingModel.Zip;
                buildingToEdit.WheelchairAccessible = buildingModel.WheelchairAccessible;
                buildingToEdit.RestroomsAvailable = buildingModel.RestroomsAvailable;
                buildingToEdit.WheelchairAccessibleRestroom = buildingModel.WheelchairAccessibleRestroom;
                buildingToEdit.PhotographyAllowed = buildingModel.PhotographyAllowed;
                buildingToEdit.StartTime = buildingModel.StartTime;
                buildingToEdit.EndTime = buildingModel.EndTime;
                buildingToEdit.Capacity = buildingModel.Capacity;
                buildingToEdit.HistoricalOverview = buildingModel.HistoricalOverview;
                buildingToEdit.VisitorExperience = buildingModel.VisitorExperience;
                buildingToEdit.Image = buildingModel.Image;

                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(buildingModel);
        }

        // GET: BuildingModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buildingModel = await _context.Buildings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (buildingModel == null)
            {
                return NotFound();
            }

            return View(buildingModel);
        }

        // POST: BuildingModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var buildingModel = await _context.Buildings.FindAsync(id);
            _context.Buildings.Remove(buildingModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuildingModelExists(int id)
        {
            return _context.Buildings.Any(e => e.Id == id);
        }

        public string GetFileName(IFormFile upload)
        {
            int indexExt = 0;
            string ext;
            string imageName = null;
            if (upload != null)
            {
                indexExt = upload.FileName.IndexOf(".");
                ext = upload.FileName.Substring(indexExt);
                imageName = Guid.NewGuid() + ext;
            }
            return imageName;
        }

        public void UploadToAzure(string imageName, IFormFile upload)
        {
            // Azure needs your connection string like a db
            string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = _config.GetValue<string>("AzureConnectionString");
            }
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            // Azure needs to know what folder you want to save in
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("dev-images");
            // Then, you can get a blob writer thing from azure
            containerClient.UploadBlob(imageName, upload.OpenReadStream());
        }

        public void DeleteFromAzure(string imageName)
        {
            // need connection to Azure just like a local db
            string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = _config.GetValue<string>("AzureConnectionString");
            }
            // connect to Azure
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            // go to a specific folder
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("dev-images");
            // find a specific file
            BlobClient blob = containerClient.GetBlobClient($"{imageName}");        
            // delete if exists
            if (blob.Exists())
            {
                blob.Delete();
            }
        }
    }
}
