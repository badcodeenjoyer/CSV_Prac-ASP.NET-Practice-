
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CSVApp.Data;
using CSVApp.Models;
using System.Data;
using CSVApp.Interfaces;

namespace CSVApp.Controllers
{
    public class CSVModelsController : Controller
    {
        private readonly CSVAppContext _context;
        private readonly IRead _readService;

        public CSVModelsController(CSVAppContext context , IRead readService)
        {
            _context = context;
            _readService=readService;
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
                    
                    var models = _readService.Read_CSV(fileName);
                    
                    _context.CSVModel.AddRange(models);
                    _context.SaveChanges();

                    return CSVView(models);      
        }
        
    }
    
}
