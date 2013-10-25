using System.Web;


namespace TAlex.Web.Services.Uploading
{
    public interface IUploadFileService
    {
        bool TryUploadFile(HttpPostedFileBase file, string uploadDir, string filename);

        string GetFileNameWithTimestamp(string fileName);

        void DeleteFile(string path, string uploadDir);

        void DeleteDirectory(string path);
    }
}
