using Microsoft.AspNetCore.Components.Forms;
using TodoList.Shared;

namespace TodoList.Client.Services.ToDoService
{
	public interface IToDoService
	{
		event Action OnChanged;
		List<TodoItem> ToDos { get; set; }
		Task LoadAllToDos();
		Task AddToDo(string title, IBrowserFile image);
		Task DeleteToDo(TodoItem todo);
		Task UpdateToDoTitle(string id, string title);
	}
}
