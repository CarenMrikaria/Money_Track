using Microsoft.AspNetCore.Mvc;
using MoneyTrack.Models;
using MoneyTrack.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MoneyTrackProject = MoneyTrack.Models.Project;
using Microsoft.AspNetCore.Identity;
using MoneyTrack.Areas.Identity.Data;

namespace MoneyTrack.Controllers
{
    [Authorize(Roles = "Admin, Investor")]
    public class ProjectController : Controller
    {
        private readonly MoneyTrackContext _context;
        private readonly UserManager<MoneyTrackUser> _userManager;

        public ProjectController(MoneyTrackContext context, UserManager<MoneyTrackUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // Admin and Investor can access the project list
        public async Task<IActionResult> Index()
        {
            var projects = await _context.ProjectTable.ToListAsync();

            // Ensure that the list is not null before passing to the view
            if (projects == null)
            {
                projects = new List<Project>(); // Return an empty list if no projects are found
            }

            return View(projects);
        }

        // Only Admin can add a project
        [Authorize(Roles = "Admin")]
        public IActionResult AddData()
        {
            return View();
        }

        // Admin and Investor can view a project for investment
        [Authorize(Roles = "Investor, Admin")]
        public IActionResult Invest(int id)
        {
            var project = _context.ProjectTable.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }


        // Only Admin can add a project (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddData(MoneyTrackProject project)
        {
            if (ModelState.IsValid)
            {
                _context.ProjectTable.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // Only Admin can edit project data
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditData(int? ProjectId)
        {
            if (ProjectId == null)
            {
                return NotFound();
            }
            var project = await _context.ProjectTable.FindAsync(ProjectId);
            if (project == null)
            {
                return NotFound(ProjectId + " is not found in the table!");
            }
            return View(project);
        }

        // Only Admin can update project data (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateData(MoneyTrackProject project)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.ProjectTable.Update(project);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Project");
                }
                return View("EditData", project);
            }
            catch (Exception ex)
            {
                return NotFound("Error: " + ex.Message);
            }
        }

        // Only Admin can delete a project
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteData(int? ProjectId)
        {
            if (ProjectId == null)
            {
                return NotFound();
            }
            var project = await _context.ProjectTable.FindAsync(ProjectId);
            if (project == null)
            {
                return BadRequest(ProjectId + " is not found in the list!");
            }
            _context.ProjectTable.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Project");
        }
        [HttpPost]
        [Authorize(Roles = "Investor")]
        public async Task<IActionResult> ProcessInvestment(int projectId, decimal investmentAmount, string cardNumber, string expiryDate, string cvv)
        {
            // Find the project in the database
            var project = await _context.ProjectTable.FindAsync(projectId);

            if (project == null)
            {
                return NotFound();
            }

            // Get the user ID of the currently logged-in investor
            var userId = _userManager.GetUserId(User);

            // Simulate credit card processing logic (optional)
            // In a real-world application, you would integrate with a payment gateway.

            // Update the project's current amount and progress
            project.currentAmount += investmentAmount;

            // Create a new Investment entry
            var investment = new Investment
            {
                UserId = userId,
                ProjectId = projectId,
                Amount = investmentAmount,
                InvestmentDate = DateTime.Now
            };

            // Add the investment to the Investments table
            _context.Investments.Add(investment);

            // Save the updated project and new investment
            await _context.SaveChangesAsync();  // Save the updated project data and investment

            // Redirect the user to a confirmation page or back to the project list
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Investor")]
        public async Task<IActionResult> MyProjects()
        {
            var userId = _userManager.GetUserId(User);
            var investments = await _context.Investments
                .Where(i => i.UserId == userId)
                .Include(i => i.Project)  // Include project details
                .ToListAsync();

            return View(investments);
        }




    }
}
