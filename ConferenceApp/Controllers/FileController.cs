using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConferenceApp.Data;
using ConferenceApp.Models;
using ConferenceApp.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using File = ConferenceApp.Models.File;

namespace ConferenceApp.Controllers
{
    public class FileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment hostingEnvironment;

        public FileController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            hostingEnvironment = environment;
        }

        // GET: File
        public async Task<IActionResult> Index()
        {
            return View(await _context.Files.ToListAsync());
        }

        // GET: File/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await _context.Files
                .FirstOrDefaultAsync(m => m.Id == id);
            if (file == null)
            {
                return NotFound();
            }

            return View(file);
        }

        // GET: File/Create
        public async Task<IActionResult> Create(int eventId)
        {
            var view = new FileViewModel();
            view.EventId = eventId;
            return View(view);
        }

        // POST: File/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FileViewModel fileViewModel)
        {
            if (ModelState.IsValid)
            {
                var file = new File();
                if (fileViewModel.MyFile != null)
                {
                    var uniqueFileName = GetUniqueFileName(fileViewModel.MyFile.FileName);
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                    var filePath = Path.Combine(uploads,uniqueFileName);
                    await fileViewModel.MyFile.CopyToAsync(new FileStream(filePath, FileMode.Create)); 

                    //to do : Save uniqueFileName  to your db table   
                    file.UniqueFileName = uniqueFileName;
                    file.Path = filePath;
                    file.Description = fileViewModel.Description;
                    file.EventId = fileViewModel.EventId;
                }
                _context.Add(file);
                await _context.SaveChangesAsync();
                // agregamos la referencia del file al evento al que pertenece
                var @event = await _context.Events.FirstOrDefaultAsync(m => m.Id == file.EventId);
                @event.FileId = file.Id;
                await _context.SaveChangesAsync();
                if (@event is PracticalSession)
                {
                    return RedirectToAction("Details", "PracticalSession", new { id = file.EventId.ToString() });
                }
                else
                {
                    return RedirectToAction("Details", "Talk", new { id = file.EventId.ToString() });
                }
            }
            return View(fileViewModel);
        }
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return  Path.GetFileNameWithoutExtension(fileName)
                    + "_" 
                    + Guid.NewGuid().ToString().Substring(0, 4) 
                    + Path.GetExtension(fileName);
        }
        
        public async Task<IActionResult> Download(int fileId)
        {
            var file = await _context.Files.FirstOrDefaultAsync(m => m.Id == fileId);
            if (file.UniqueFileName == null)  
                return Content("filename not present");  
  
            var path = Path.Combine(hostingEnvironment.WebRootPath, "uploads", file.UniqueFileName);  
  
            var memory = new MemoryStream();  
            using (var stream = new FileStream(path, FileMode.Open))  
            {  
                await stream.CopyToAsync(memory);  
            }  
            memory.Position = 0;  
            return File(memory, GetContentType(path), Path.GetFileName(path));  
        }
        
        private string GetContentType(string path)  
        {  
            var types = GetMimeTypes();  
            var ext = Path.GetExtension(path).ToLowerInvariant();  
            return types[ext];  
        } 
        
        private Dictionary<string, string> GetMimeTypes()  
        {  
            return new Dictionary<string, string>  
            {  
                {".txt", "text/plain"},  
                {".pdf", "application/pdf"},  
                {".doc", "application/vnd.ms-word"},  
                {".docx", "application/vnd.ms-word"},  
                {".xls", "application/vnd.ms-excel"},  
                {".xlsx", "application/vnd.openxmlformats  officedocument.spreadsheetml.sheet"},  
                    {".png", "image/png"},  
                {".jpg", "image/jpeg"},  
                {".jpeg", "image/jpeg"},  
                {".gif", "image/gif"},  
                {".csv", "text/csv"}  
            };  
            } 

        // GET: File/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await _context.Files.FindAsync(id);
            if (file == null)
            {
                return NotFound();
            }
            return View(file);
        }

        // POST: File/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Path,Description,EventId")] File file)
        {
            if (id != file.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(file);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileExists(file.Id))
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
            return View(file);
        }

        // GET: File/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await _context.Files
                .FirstOrDefaultAsync(m => m.Id == id);
            if (file == null)
            {
                return NotFound();
            }

            return View(file);
        }

        // POST: File/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var file = await _context.Files.FindAsync(id);
            _context.Files.Remove(file);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FileExists(int id)
        {
            return _context.Files.Any(e => e.Id == id);
        }
    }
}
