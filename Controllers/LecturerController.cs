using CMCSWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace CMCSWeb.Controllers
{
    public class LecturerController : Controller
    {
       
            // GET: /Lecturer/
            public IActionResult Index()
            {
                // Dummy data for prototype stage
                var lecturers = new List<Lecturer>
            {
                new Lecturer { LecturerId = 1, FullName = "John Doe", Email = "john.doe@example.com" },
                new Lecturer { LecturerId = 2, FullName = "Jane Smith", Email = "jane.smith@example.com" }
            };

                // Passes the list to the View
                return View(lecturers);
            }
        }

    }
