using TodoList.Server.Services.BlobService;
using TodoList.Server.Services.CosmosDbService;
using TodoList.Shared;

namespace TodoList.Server.Services.ToDoService
{
	public class ToDoService : IToDoService
	{
		private ICosmosDbService _cosmosDbService;
		private IBlobService _blobService;

		public List<TodoItem> ToDos { get; set; }

        public ToDoService(ICosmosDbService cosmosDbService, IBlobService blobService)
		{
			_cosmosDbService = cosmosDbService;
			_blobService = blobService;
			ToDos = GetToDosByDbContext();
        }

		public async Task AddToDo(string title, Stream stream)
		{
			string url = await _blobService.UploadImage(title + ".jpg", stream);
			TodoItem todo = new TodoItem { id = (int.Parse(GetMaxId()) + 1).ToString(), IsDone = false, Title = title, ImageUrl = url };
			await _cosmosDbService.AddTodo(todo);
			ToDos.Add(todo);
		}

		public async Task<List<TodoItem>> DeleteToDoById(string id, string imageName)
		{
			TodoItem todo = ToDos.FirstOrDefault(x => x.id == id);
			if (todo != null)
			{
				await _blobService.DeleteImage(imageName);
				await _cosmosDbService.DeleteTodo(id);
				ToDos.Remove(todo);
			}

			return ToDos;
		}

		public async Task<List<TodoItem>> UpdateToDoTitleById(string id, string title)
		{
			TodoItem todo = ToDos.FirstOrDefault(x => x.id == id);
			if (todo != null)
			{
				todo.Title = title;
				await _cosmosDbService.UpdateTodo(id, todo);
			}

			return ToDos;
		}

		public async Task<List<TodoItem>> GetAllTodos()
		{
			return ToDos.ToList();
		}

		#region private Methods
		private List<TodoItem> GetToDosByDbContext()
		{
			return _cosmosDbService.GetAllTodo().Result;
		}

		private string GetMaxId()
		{
			return ToDos.Max(x => int.Parse(x.id)).ToString();
		}
		#endregion
	}
}
