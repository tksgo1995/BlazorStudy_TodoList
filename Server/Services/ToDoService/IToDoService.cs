using TodoList.Shared;

namespace TodoList.Server.Services.ToDoService
{
	public interface IToDoService
	{
		Task AddToDo(string title, Stream stream);
		Task<List<TodoItem>> DeleteToDoById(string id, string imageName);
		Task<List<TodoItem>> UpdateToDoTitleById(string id, string title);
		Task<List<TodoItem>> GetAllTodos();
	}
}
