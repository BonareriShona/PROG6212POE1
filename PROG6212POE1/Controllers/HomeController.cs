using Microsoft.AspNetCore.Mvc;

namespace CMCSWeb.Controllers
{
    public class HomeController : Controller
    {
        // Landing page
        [HttpGet]
        public IActionResult Index()
        {
            ViewData["Title"] = "Welcome to CMCS";
            ViewData["SystemName"] = "Contract Monthly Claim System";
            return View();
        }

        // About page
        [HttpGet]
        public IActionResult About()
        {
            ViewData["Title"] = "About CMCS";
            ViewData["Description"] = "The Contract Monthly Claim System (CMCS) streamlines the claim submission and approval process for lecturers, coordinators, and managers. Lecturers can submit claims with supporting documents, coordinators verify them, and managers approve payments.";
            return View();
        }

        // Help or contact page
        [HttpGet]
        public IActionResult Help()
        {
            ViewData["Title"] = "Help & Support";
            ViewData["Message"] = "If you experience any issues, please contact your Programme Coordinator or Academic Manager for assistance.";
            return View();
        }

        // Error page
        [HttpGet]
        public IActionResult Error()
        {
            ViewData["Title"] = "Error";
            return View();
        }

        // Quick navigation to each actor's dashboard
        // This redirects users to the appropriate controller based on their role
        [HttpGet]
        public IActionResult Dashboard(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                TempData["ErrorMessage"] = "Please select a role to proceed.";
                return RedirectToAction(nameof(Index));
            }

            switch (role.ToLower())
            {
                case "lecturer":
                    // Redirect to Lecturer Track page
                    return RedirectToAction("Track", "Lecturer");
                case "coordinator":
                    // Redirect to Coordinator management page
                    return RedirectToAction("Manage", "Coordinator");
                case "manager":
                    // Redirect to Manager management page
                    return RedirectToAction("Manage", "Manager");
                default:
                    TempData["ErrorMessage"] = "Invalid role selected.";
                    return RedirectToAction(nameof(Index));
            }
        }
    }
}
