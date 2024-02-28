using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoList.Server.Services.ToDoService;
using TodoList.Shared;

namespace TodoList.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ToDoController : ControllerBase
	{
		private readonly IToDoService _toDoService;

        public ToDoController(IToDoService toDoService)
        {
            _toDoService = toDoService;
        }

        [HttpPost("AddToDo")]
		public async Task<ActionResult<List<TodoItem>>> AddToDo([FromForm]string title, [FromForm]IFormFile image)
		{
			using(var stream = image.OpenReadStream())
			{
				await _toDoService.AddToDo(title, stream);
			}
			return Ok(await _toDoService.GetAllTodos());
		}

		[HttpGet("DeleteToDoById/{id}/{imageName}")]
		public async Task<ActionResult<List<TodoItem>>> DeleteToDoById(string id, string imageName)
		{
			return Ok(await _toDoService.DeleteToDoById(id, imageName));
		}

		[HttpGet("UpdateToDoTitleById/{id}/{title}")]
		public async Task<ActionResult<List<TodoItem>>> UpdateToDoTitleById(string id, string title)
		{
			return Ok(await _toDoService.UpdateToDoTitleById(id, title));
		}

		[HttpGet("GetAllTodos")]
		public async Task<ActionResult<List<TodoItem>>> GetAllTodos()
		{
			return Ok(await _toDoService.GetAllTodos());
		}
	}
}
