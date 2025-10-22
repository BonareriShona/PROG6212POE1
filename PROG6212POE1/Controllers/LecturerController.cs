using Microsoft.AspNetCore.Mvc;
using CMCSWeb.Data;
using CMCSWeb.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CMCSWeb.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public LecturerController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // STEP 1: Display the claim submission form
        [HttpGet]
        public IActionResult Submit()
        {
            return View();
        }

        // STEP 2: Handle claim submission (with file upload)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(Claim claim, IFormFile? document)
        {
            if (ModelState.IsValid)
            {
                // ✅ Handle file upload (with validation)
                if (document != null && document.Length > 0)
                {
                    var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx", ".jpg", ".jpeg" };
                    var fileExtension = Path.GetExtension(document.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("DocumentPath", "Only .pdf, .docx, .xlsx, .jpg, or .jpeg files are allowed.");
                        return View(claim);
                    }

                    // Limit file size to 5 MB
                    const long maxFileSize = 5 * 1024 * 1024; // 5 MB
                    if (document.Length > maxFileSize)
                    {
                        ModelState.AddModelError("DocumentPath", "File size cannot exceed 5 MB.");
                        return View(claim);
                    }

                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = $"{Path.GetFileNameWithoutExtension(document.FileName)}_{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await document.CopyToAsync(stream);
                    }

                    claim.DocumentPath = uniqueFileName;
                }

                // ✅ Ensure proper claim initialization
                claim.Status = ClaimStatus.Pending;
                claim.SubmittedAt = DateTime.Now;

                _context.Claims.Add(claim);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Your claim has been submitted successfully!";
                return RedirectToAction(nameof(Track));
            }

            // If validation fails, reload the form with errors
            return View(claim);
        }

        // STEP 3: Lecturer can view and track all their claims
        [HttpGet]
        public async Task<IActionResult> Track()
        {
            var claims = await _context.Claims
                .OrderByDescending(c => c.SubmittedAt)
                .ToListAsync();

            return View(claims);
        }
    }
}
