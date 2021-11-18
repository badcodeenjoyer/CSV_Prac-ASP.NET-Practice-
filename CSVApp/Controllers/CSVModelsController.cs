using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CSVApp.Data;
using CSVApp.Models;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using CsvHelper;
using CsvHelper.Configuration;

namespace CSVApp.Controllers
{
    public class CSVModelsController : Controller
    {
        private readonly CSVAppContext _context;

        public CSVModelsController(CSVAppContext context)
        {
            _context = context;
        }


          public  IActionResult CSVView(List<CSVModel>? models =null)
          {
              models = models==null ? new List<CSVModel>():models;
              return View(models);
          }
        public async Task<IActionResult> Index(string sortOrder)
        {
           
            var models = _context.CSVModel;
            switch (sortOrder)
            {
                case "Name":             
                    return View(await models.OrderBy(s => s.Name).ToListAsync());
                case "DateOfBirth":                 
                    return View(await models.OrderBy(s => s.DateOfBirth).ToListAsync());
                case "Married":                   
                    return View(await models.OrderBy(s => s.Married).ToListAsync());
                case "Phone":                 
                    return View(await models.OrderBy(s => s.Phone).ToListAsync());
                case "Salary":                
                    return View(await models.OrderBy(s => s.Salary).ToListAsync()); 
                default:
                    return View(await models.ToListAsync());                
            }
           
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cSVModel = await _context.CSVModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cSVModel == null)
            {
                return NotFound();
            }

            return View(cSVModel);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DateOfBirth,Married,Phone,Salary")] CSVModel cSVModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cSVModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cSVModel);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cSVModel = await _context.CSVModel.FindAsync(id);
            if (cSVModel == null)
            {
                return NotFound();
            }
            return View(cSVModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DateOfBirth,Married,Phone,Salary")] CSVModel cSVModel)
        {
            if (id != cSVModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cSVModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CSVModelExists(cSVModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cSVModel);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cSVModel = await _context.CSVModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cSVModel == null)
            {
                return NotFound();
            }

            return View(cSVModel);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var cSVModel = await _context.CSVModel.FindAsync(id);
            _context.CSVModel.Remove(cSVModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CSVModelExists(int id)
        {
            return _context.CSVModel.Any(e => e.Id == id);
        }
      
        [HttpPost]
        public  IActionResult CSVView(IFormFile file,[FromServices] Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            string fileName = $"{hostingEnvironment.WebRootPath}\\files\\{file.FileName}";
            
                    using (FileStream fileStream = System.IO.File.Create(fileName))
                    {
                        file.CopyTo(fileStream);
                        fileStream.Flush();
                    }
                    
                    var models = Read_CSV(fileName);
                    
                    _context.CSVModel.AddRange(models);
                    _context.SaveChanges();

                    return CSVView(models);      
        }
        private List<CSVModel> Read_CSV(string fileName)
        {
            var path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}"+"\\"+ fileName;
            var models = new List<CSVModel>();
            using (var reader = new StreamReader(fileName)) 
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<FooMap>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {

                    var model = csv.GetRecord<CSVModel>();
                    
                    models.Add(model);
                }
            }
            
            return models;



        }
    }
    public class FooMap : ClassMap<CSVModel>
    {
        public FooMap()
        {

            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.Id).Ignore();
        }
    }
}
