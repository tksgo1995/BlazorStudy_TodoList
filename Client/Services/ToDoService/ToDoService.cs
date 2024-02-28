using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using TodoList.Shared;

namespace TodoList.Client.Services.ToDoService
{
	public class ToDoService : IToDoService, IDisposable
	{
		private readonly HttpClient _http;

		public event Action OnChanged;

		public List<TodoItem> ToDos { get; set; } = new List<TodoItem>();

        public ToDoService(HttpClient http)
        {
            _http = http;
		}

		public void Dispose()
		{
			_http.Dispose();
		}

		public async Task DeleteToDo(TodoItem todo)
		{
			string imageName = todo.ImageUrl.Substring(todo.ImageUrl.LastIndexOf("/") + 1);
			ToDos = await _http.GetFromJsonAsync<List<TodoItem>>($"api/ToDo/DeleteToDoById/{todo.id}/{imageName}");
			OnChanged.Invoke();
		}

		public async Task LoadAllToDos()
		{
			ToDos = await _http.GetFromJsonAsync<List<TodoItem>>("api/ToDo/GetAllTodos");
		}

		public async Task UpdateToDoTitle(string id, string title)
		{
			ToDos = await _http.GetFromJsonAsync<List<TodoItem>>($"api/ToDo/UpdateToDoTitleById/{id}/{title}");
			OnChanged.Invoke();
		}

		public async Task AddToDo(string title, IBrowserFile image)
		{
			var content = new MultipartFormDataContent();
			var titleContent = new StringContent(title);
			content.Add(titleContent, "title");

			var streamContent = new StreamContent(image.OpenReadStream(maxAllowedSize: 10485760));
			streamContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
			content.Add(streamContent, "image", image.Name);

			var response = await _http.PostAsync("api/ToDo/AddToDo", content);

			if(response.IsSuccessStatusCode)
			{
				var jsonResponse = await response.Content.ReadAsStringAsync();
				ToDos = JsonSerializer.Deserialize<List<TodoItem>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				OnChanged.Invoke();
			}
		}
	}
}