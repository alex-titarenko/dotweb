using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;


namespace TAlex.Web.Services.Storage
{
    public interface IStorageService
    {
        Task<bool> UploadBlobAsync(Stream stream, string relativePath, IDictionary<string, string> blobMetadata = null);

        Task DeleteBlobAsync(string relativePath);

        Task DownloadToFileAsync(string sourcePath, string destinationPath);

        Task<IEnumerable<string>> ListBlobsAsync(string relativePath);
    }
}
