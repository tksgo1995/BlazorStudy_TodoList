using TodoList.Shared;

namespace TodoList.Server.Services.CosmosDbService
{
	public interface ICosmosDbService
	{
		Task<List<TodoItem>> GetAllTodo();
		Task AddTodo(TodoItem item);
		Task DeleteTodo(string id);
		Task UpdateTodo(string id, TodoItem todo);
	}
}
