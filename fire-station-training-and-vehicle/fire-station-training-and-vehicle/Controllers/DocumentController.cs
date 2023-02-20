using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using fire_station_training_and_vehicle.Models;
using Microsoft.AspNetCore.Identity;
using static System.Collections.Specialized.BitVector32;

namespace fire_station_training_and_vehicle.Controllers
{
    public class DocumentController : Controller
    {
        private readonly FireFighterContext _context;
        private readonly UserManager<User> _userManager;

        public DocumentController(FireFighterContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Document
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var documents = await _context.Documents.Include(d => d.User).Include(x => x.RequestType).Where(x => x.UserId == userId).Where(x=>x.Status==true).ToListAsync();
            return View(documents);
        }
        public async Task<IActionResult> Download(string fileName)
        {
            if (fileName == null)
                return Content("filename is not availble");

            var path = Path.Combine(Directory.GetCurrentDirectory(), "upload", fileName);
           // var path = Path.Combine(Directory.GetCurrentDirectory(), "upload", filePath);

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

        public async Task<IActionResult> Request()
        {
            var newRequests = await _context.Documents.Where(x => x.Status == null).Include(x => x.User).Include(x => x.RequestType).ToListAsync();
            return View(newRequests);
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
        {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
        {".png", "image/png"},
        {".jpg", "image/jpeg"},
        {".jpeg", "image/jpeg"},
        {".gif", "image/gif"},
        {".csv", "text/csv"}
         };
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload([FromForm] int requestId, IFormFile[] files)
        {
            foreach (var file in files)
            {
                var request = _context.Documents.FirstOrDefault(x => x.Id == requestId);
                DateTime date = new DateTime();
                // Get the file name from the browser
                string extension = Path.GetExtension(file.FileName);
                string result = file.FileName.Substring(0, file.FileName.Length - extension.Length);

                var fileName = result + DateTime.Now.ToString("yyyyMMddHHmmss")+extension;

                // Get file path to be uploaded
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "upload", fileName);

                if (request != null)
                {
                    request.Name = fileName;
                    request.Path = filePath;
                    request.Status = true;
                    using (var localFile = System.IO.File.OpenWrite(filePath))
                    using (var uploadedFile = file.OpenReadStream())
                    {
                        uploadedFile.CopyTo(localFile);
                    }
                    _context.Update(request);

                    await _context.SaveChangesAsync();
                }

            }
            TempData["Success"] = "Document uploaded successfully!!";

            return RedirectToAction("Request");
        }
        // GET: Document/Create
        public IActionResult Create()
        {
            ViewData["TypeId"] = new SelectList(_context.RequestTypes, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Document/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Description,RequestTypeId,RequestType,Status,Name,Path")] Document document)
        {

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                document.UserId = userId;
                document.Status = null;
                document.Name = null;
                document.Path = null;
                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeId"] = new SelectList(_context.RequestTypes, "Id", "Name", document.RequestTypeId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", document.UserId);
            return View(document);
        }

        // GET: Document/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Documents == null)
            {
                return NotFound();
            }

            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            ViewData["TypeId"] = new SelectList(_context.RequestTypes, "Id", "Id", document.RequestTypeId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", document.UserId);
            return View(document);
        }

        // POST: Document/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Description,TypeId,Status,Name,Path")] Document document)
        {
            if (id != document.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.Id))
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
            ViewData["TypeId"] = new SelectList(_context.RequestTypes, "Id", "Id", document.RequestTypeId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", document.UserId);
            return View(document);
        }

        // GET: Document/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Documents == null)
        //    {
        //        return NotFound();
        //    }

        //   // var document = await _context.Documents
        //        //.Include(d => d.Type)
        //        //.Include(d => d.User)
        //        //.FirstOrDefaultAsync(m => m.Id == id);
        //    if (document == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(document);
        //}

        // POST: Document/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Documents == null)
            {
                return Problem("Entity set 'FireFighterContext.Documents'  is null.");
            }
            var document = await _context.Documents.FindAsync(id);
            if (document != null)
            {
                _context.Documents.Remove(document);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.Id == id);
        }
    }
}
