namespace TodoList.Server.Services.BlobService
{
	public interface IBlobService
	{
		Task<string> UploadImage(string blobName, Stream stream);
		Task DeleteImage(string blobName);
	}
}
