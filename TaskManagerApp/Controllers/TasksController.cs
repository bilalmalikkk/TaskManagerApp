using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagerApp.Data;
using TaskManagerApp.Models;

namespace TaskManagerApp.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Tasks
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tasks = await _context.TaskItems
                .Where(t => t.UserId == userId)
                .ToListAsync();
            return View(tasks);
        }

        // GET: /Tasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItem task)
        {
            if (ModelState.IsValid)
            {
                task.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        // GET: /Tasks/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null || task.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return NotFound();

            return View(task);
        }

        // POST: /Tasks/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskItem task)
        {
            if (id != task.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    task.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        // GET: /Tasks/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null || task.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return NotFound();

            return View(task);
        }

        // GET: /Tasks/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null || task.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return NotFound();

            return View(task);
        }

        // POST: /Tasks/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task != null && task.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                _context.TaskItems.Remove(task);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
