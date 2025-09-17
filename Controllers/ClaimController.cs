using CMCSWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace CMCSWeb.Controllers
{
    public class ClaimController : Controller
    {
        // Prototype: static in-memory list of claims
        private static List<Claim> claims = new List<Claim>
        {
            new Claim { ClaimId = 1, LecturerId = 1, HoursWorked = 20, HourlyRate = 400, Status = "Pending" },
            new Claim { ClaimId = 2, LecturerId = 2, HoursWorked = 15, HourlyRate = 450, Status = "Pending" }
        };

        // GET: /Claim/
        public IActionResult Index()
        {
            return View(claims);
        }

        // GET: /Claim/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Claim/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Claim claim, IFormFile SupportingDocument)
        {
            if (ModelState.IsValid)
            {
                claim.ClaimId = claims.Count + 1;

                // Handle supporting document (prototype only)
                if (SupportingDocument != null && SupportingDocument.Length > 0)
                {
                    // For prototype we only save the filename
                    claim.SupportingDocumentPath = SupportingDocument.FileName;
                }

                claim.Status = "Pending";
                claims.Add(claim);

                return RedirectToAction(nameof(Index));
            }

            return View(claim);
        }

        // GET: /Claim/Verify/{id}
        public IActionResult Verify(int id)
        {
            var claim = claims.FirstOrDefault(c => c.ClaimId == id);
            if (claim != null)
            {
                claim.IsVerified = true;
                claim.Status = "Verified";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: /Claim/Approve/{id}
        public IActionResult Approve(int id)
        {
            var claim = claims.FirstOrDefault(c => c.ClaimId == id);
            if (claim != null && claim.IsVerified)
            {
                claim.IsApproved = true;
                claim.Status = "Approved";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
