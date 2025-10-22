using CMCSWeb.Data;
using CMCSWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CMCSWeb.Controllers
{
    public class ClaimController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClaimController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Claim/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Message = null;
            return View();
        }

        // POST: Claim/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Claim claim)
        {
            if (!ModelState.IsValid)
            {
                return View(claim);
            }

            // Handle file upload
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                claim.DocumentPath = "/uploads/" + uniqueFileName;
            }

            // Auto-fill system fields
            claim.Status = ClaimStatus.Pending;
            claim.SubmittedAt = DateTime.Now;

            // Save to DB
            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            // Success message
            TempData["SuccessMessage"] = "Claim submitted successfully! Awaiting coordinator verification.";
            ModelState.Clear();

            return RedirectToAction(nameof(Status));
        }

        // GET: Claim/Status
        [HttpGet]
        public async Task<IActionResult> Status()
        {
            var claims = await _context.Claims
                .OrderByDescending(c => c.SubmittedAt)
                .ToListAsync();

            if (!claims.Any())
            {
                ViewBag.InfoMessage = "You have not submitted any claims yet.";
            }

            return View(claims);
        }
    }
}
