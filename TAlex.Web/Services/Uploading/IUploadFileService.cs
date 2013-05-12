using System.Web;


namespace TAlex.Web.Services.Uploading
{
    public interface IUploadFileService
    {
        bool TryUploadFile(HttpPostedFileBase file, string uploadDir, out string filename, bool withTimestamp = true);

        void DeleteFile(string path, string uploadDir);

        void DeleteDirectory(string path);
    }
}
