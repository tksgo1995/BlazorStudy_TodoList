
using Azure.Storage.Blobs;

namespace TodoList.Server.Services.BlobService
{
	public class BlobService : IBlobService
	{
		#region Data
		private readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=todolistimage;AccountKey=DdzDEkE+5/Q7tnOLt0/+zBEQ/hzxvunlP68JZbkMBF69oVL1D1S5lU8jGQ1yyCGi8dHEL/upuAe4+AStX95jOw==;EndpointSuffix=core.windows.net";
		private readonly string _containerName = "image-container";
		#endregion

		private BlobServiceClient _blobServiceClient;
		private BlobContainerClient _blobContainerClient;

        public BlobService()
        {
            _blobServiceClient = new BlobServiceClient(_connectionString);
			_blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        }

		public async Task DeleteImage(string blobName)
		{
			BlobClient blobClient = _blobContainerClient.GetBlobClient(blobName);
			await blobClient.DeleteIfExistsAsync();
		}

		public async Task<string> UploadImage(string blobName, Stream stream)
		{
			BlobClient blobClient = _blobContainerClient.GetBlobClient(blobName);

			await blobClient.UploadAsync(stream, true);
			
			return blobClient.Uri.ToString();
		}
	}
}
