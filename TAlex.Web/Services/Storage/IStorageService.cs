using System.IO;
using System.Web;


namespace TAlex.Web.Services.Storage
{
    public interface IStorageService
    {
        bool UploadBlob(Stream stream, string path);

        void DeleteBlob(string path);
    }
}
