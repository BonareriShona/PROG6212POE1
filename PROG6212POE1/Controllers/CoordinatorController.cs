using Microsoft.AspNetCore.Mvc;
using CMCSWeb.Data;
using CMCSWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CMCSWeb.Controllers
{
    public class CoordinatorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoordinatorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // STEP 1: Display all claims pending verification
        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var pendingClaims = await _context.Claims
                .Where(c => c.Status == ClaimStatus.Pending)
                .OrderByDescending(c => c.SubmittedAt)
                .ToListAsync();

            if (!pendingClaims.Any())
                ViewBag.InfoMessage = "There are no pending claims awaiting verification.";

            return View(pendingClaims);
        }

        // STEP 2: Verify (approve for manager review)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Verify(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null)
            {
                TempData["ErrorMessage"] = "Claim not found.";
                return RedirectToAction(nameof(Manage));
            }

            if (claim.Status == ClaimStatus.Pending)
            {
                claim.Status = ClaimStatus.Verified;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Claim #{claim.Id} verified successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Only pending claims can be verified.";
            }

            return RedirectToAction(nameof(Manage));
        }

        // STEP 3: Reject claim (if incorrect or incomplete)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null)
            {
                TempData["ErrorMessage"] = "Claim not found.";
                return RedirectToAction(nameof(Manage));
            }

            if (claim.Status == ClaimStatus.Pending)
            {
                claim.Status = ClaimStatus.Rejected;
                await _context.SaveChangesAsync();
                TempData["ErrorMessage"] = $"Claim #{claim.Id} has been rejected.";
            }
            else
            {
                TempData["ErrorMessage"] = "Only pending claims can be rejected.";
            }

            return RedirectToAction(nameof(Manage));
        }
    }


}
