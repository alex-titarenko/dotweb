using System.Collections.Generic;
using System.IO;
using System.Web;


namespace TAlex.Web.Services.Storage
{
    public interface IStorageService
    {
        bool UploadBlob(Stream stream, string relativePath);

        void DeleteBlob(string relativePath);

        void DownloadToFile(string sourcePath, string destinationPath);

        IEnumerable<string> ListBlobs(string relativePath);
    }
}
