using Microsoft.AspNetCore.Mvc;

namespace TaskManagerApp.Controllers
{
    public class TaskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            // Sample Task object for demonstration
            var task = new { Id = id, Title = "Sample Task", Description = "This is a task description" };
            return View(task);
        }

    }
}


