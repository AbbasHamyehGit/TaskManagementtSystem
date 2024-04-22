using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementAPI.Data;
using Task = TaskManagementAPI.Models.Task;
using Person = TaskManagementAPI.Models.Person;
using Microsoft.AspNetCore.Authorization;


namespace TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
	//[Authorize]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Task>>> GetTasks()
        {
            var tasks = await _context.Tasks.ToListAsync();
            return Ok(tasks);
        }

        // GET: api/Tasks/5
		
        [HttpGet("{id}")]
		
        public async Task<ActionResult<Task>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        // PUT: api/Tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, Task task)
        {
            var existingPerson = await _context.Person.FindAsync(task.PersonId);

            if (existingPerson == null)
            {
                return BadRequest("Invalid personId. No matching person found.");
            }
            if (id != task.Id)
            {
                return BadRequest();
            }

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tasks
        [HttpPost]
        public async Task<ActionResult<Task>> PostTask(Task task)
        {
            var existingPerson = await _context.Person.FindAsync(task.PersonId);

            if (existingPerson == null)
            {
                return BadRequest("Invalid personId. No matching person found.");
            }

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }


    
        // GET: api/Tasks/ForPerson/5
        [HttpGet("ForPerson/{personId}")]
        // [Authorize]
        public async Task<ActionResult<IEnumerable<Task>>> GetTasksForPerson(int personId)
        {
            var tasksForPerson = await _context.Tasks.Where(t => t.PersonId == personId).ToListAsync();

            if (tasksForPerson == null || tasksForPerson.Count == 0)
            {
                return NotFound("No tasks found for the specified person ID.");
            }

            return Ok(tasksForPerson);
        }


        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
