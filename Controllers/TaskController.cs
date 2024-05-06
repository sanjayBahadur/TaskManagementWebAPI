using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private TaskContext _context;
        public TaskController(TaskContext context)
        {
            _context = context;
        }

        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetTasks()
        {
            try
            {
                var tasks = await _context.Tasks.ToListAsync();
                if (tasks.Count == 0)
                {
                    return NotFound(); 
                }

                return tasks;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


        [HttpDelete("DeleteAll")]
        public async Task<ActionResult> DeleteAllTasks()
        {
            try
            {
                _context.Tasks.RemoveRange(_context.Tasks);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting tasks: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Task>> GetTaskById(int id)
        {
            try
            {
                // Retrieve the task with the specified ID from the database
                var task = await _context.Tasks.FindAsync(id);

                // Check if the task was found
                if (task == null)
                {
                    // If task not found, return a 404 Not Found response
                    return NotFound();
                }

                // Return a 200 OK response with the task data
                return Ok(task);
            }
            catch (Exception ex)
            {
                // Handle any unexpected server error and return a 500 Internal Server Error response
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while fetching task details: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Models.Task>> AddTask(Models.Task task)
        {
            try
            {
                // Add the new task to the database
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                // Return a 201 Created response with the newly created task
                return CreatedAtAction(nameof(GetTaskById), new { id = task.ID }, task);
            }
            catch (Exception ex)
            {
                // Handle any unexpected server error and return a 500 Internal Server Error response
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding the task: {ex.Message}");
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<Models.Task>> DeleteTask(int id)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task == null)
                {
                    return NotFound();
                }

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();

                return task;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting the task: {ex.Message}");
            }
        }

        [HttpGet("priority")]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetPriorityTasks()
        {
            try
            {
                // Fetch tasks with high priority from the database
                var priorityTasks = await _context.Tasks.Where(t => t.HighPriority).ToListAsync();

                // Check if any high priority tasks were found
                if (priorityTasks.Count == 0)
                {
                    return NotFound(); // Return 404 if no high priority tasks are found
                }

                return Ok(priorityTasks); // Return high priority tasks
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while fetching high priority tasks: {ex.Message}");
            }
        }

    }
}
