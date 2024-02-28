using Microsoft.Azure.Cosmos;
using TodoList.Shared;

namespace TodoList.Server.Services.CosmosDbService
{
	public class CosmosDbService : ICosmosDbService, IDisposable
	{
		#region Data
		private readonly string _connectionString = "AccountEndpoint=https://todolist-cosmos.documents.azure.com:443/;AccountKey=QwmpaC5k3rAmzeJ311iQcO4a10vUOlsS4S7L5u2tr4CHWt5mFg43sakjTMxqulPRwF8OQ5n2eOpcACDbreMP2g==;";
		private readonly string _databaseId = "ToDoList";
		private readonly string _containerId = "ToDoListContainer";
		#endregion

		private CosmosClient _client;
		private Container _container;

        public CosmosDbService()
        {
			_client = new CosmosClient(_connectionString);
			_container = _client.GetContainer(_databaseId, _containerId);
		}
		public void Dispose()
		{
			_container = null;
			_client.Dispose();
		}

		public async Task AddTodo(TodoItem todo)
		{
			try
			{
				await _container.CreateItemAsync(todo, new PartitionKey(todo.id));
			}
			catch (Exception ex)
			{

			}
		}

		public async Task DeleteTodo(string id)
		{
			await _container.DeleteItemAsync<TodoItem>(id, new PartitionKey(id));
		}

		public async Task<List<TodoItem>> GetAllTodo()
		{
			var query = "SELECT * FROM c";
			var queryDefinition = new QueryDefinition(query);
			var queryResultSetIterator = _container.GetItemQueryIterator<TodoItem>(queryDefinition);

			var results = new List<TodoItem>();

			while(queryResultSetIterator.HasMoreResults)
			{
				var response = await queryResultSetIterator.ReadNextAsync();
				results.AddRange(response);
			}

			return results;
		}

		public async Task UpdateTodo(string id, TodoItem todo)
		{
			await _container.ReplaceItemAsync(todo, id, new PartitionKey(id));
		}
	}
}
