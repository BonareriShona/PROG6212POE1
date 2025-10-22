using Microsoft.AspNetCore.Mvc;
using CMCSWeb.Data;
using CMCSWeb.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CMCSWeb.Controllers
{
    public class ManagerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Display all verified claims for approval/rejection
        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var verifiedClaims = await _context.Claims
                .Where(c => c.Status == ClaimStatus.Verified)
                .OrderByDescending(c => c.SubmittedAt)
                .ToListAsync();

            return View(verifiedClaims);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim != null && claim.Status == ClaimStatus.Verified)
            {
                claim.Status = ClaimStatus.Approved;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Manage));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim != null && claim.Status == ClaimStatus.Verified)
            {
                claim.Status = ClaimStatus.Rejected;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Manage));
        }
    }
}
