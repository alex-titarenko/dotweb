using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Web;


namespace TAlex.Web.Services.Storage
{
    public class LocalStorageService : IStorageService
    {
        #region Properties

        public string BasePath { get; set; }

        #endregion

        #region Constructors

        public LocalStorageService()
        {
        }

        public LocalStorageService(string basePath)
            : base()
        {
            BasePath = basePath;
        }

        #endregion

        #region IStorageService Members

        public bool UploadBlob(Stream stream, string relativePath)
        {
            try
            {
                string mapPath = GetAbsolutePath(relativePath);
                string dirName = Path.GetDirectoryName(mapPath);
                if (!Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName);
                }

                using (var fileStream = new FileStream(mapPath, FileMode.Create))
                {
                    stream.CopyTo(fileStream);
                    fileStream.Flush();
                }
            }
            catch (Exception exc)
            {
                Trace.TraceError("An exception occurred while uploading blob: {0}", exc);
                return false;
            }

            return true;
        }

        public void DeleteBlob(string relativePath)
        {
            if (relativePath != null)
            {
                try
                {
                    var mapPath = GetAbsolutePath(relativePath);
                    var dirName = Path.GetDirectoryName(mapPath);
                    File.Delete(mapPath);

                    if (!Directory.GetFiles(dirName, "*.*", SearchOption.AllDirectories).Any())
                    {
                        Directory.Delete(dirName, true);
                    }
                }
                catch (DirectoryNotFoundException)
                {
                }
            }
        }

        public void DownloadToFile(string sourceRelativePath, string destinationPath)
        {
            var mapPath = GetAbsolutePath(sourceRelativePath);
            File.Copy(mapPath, destinationPath, true);
        }

        #endregion

        #region Helpers

        private string GetAbsolutePath(string relativePath)
        {
            return HttpContext.Current.Server.MapPath(Path.Combine(BasePath + String.Empty, relativePath));
        }

        #endregion
    }
}
