namespace WebAdvert.Web.Services.Abstract
{
    public interface IFileUploader
    {
        Task<bool> FileUploadAsync(string fileName, Stream stream);

    }
}
