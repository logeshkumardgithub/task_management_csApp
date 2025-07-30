using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Data;
using TaskManagement.API.Models;

namespace TaskManagement.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TasksController(AppDbContext context) => _context = context;

        [HttpGet]
        //public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks() => await _context.Tasks.ToListAsync();
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks([FromQuery] string? title, [FromQuery] string? status)
        {
            var query = _context.Tasks.AsQueryable();
            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(t => t.Title != null && t.Title.ToLower().Contains(title.ToLower()));

            if (status == "Completed")
                query = query.Where(t => t.IsCompleted);
            else if (status == "Pending")
                query = query.Where(t => !t.IsCompleted);

            return await query.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();
            return task;
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> Create(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskItem updatedTask)
        {
            if (id != updatedTask.Id) return BadRequest();
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.DueDate = updatedTask.DueDate;
            task.IsCompleted = updatedTask.IsCompleted;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("completed")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetCompleted() =>
            await _context.Tasks.Where(t => t.IsCompleted).ToListAsync();
    }
}
