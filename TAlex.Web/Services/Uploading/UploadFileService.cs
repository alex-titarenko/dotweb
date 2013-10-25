using System;
using System.Diagnostics;
using System.IO;
using System.Web;


namespace TAlex.Web.Services.Uploading
{
    public class UploadFileService : IUploadFileService
    {
        #region IUploadFileService Members

        public bool TryUploadFile(HttpPostedFileBase file, string uploadDir, string filename)
        {
            try
            {
                string dir = HttpContext.Current.Server.MapPath(uploadDir);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                file.SaveAs(Path.Combine(dir, filename));
            }
            catch (Exception exc)
            {
                Trace.TraceError("An exception occurred while uploading file: {0}", exc);
                return false;
            }

            return true;
        }

        public string GetFileNameWithTimestamp(string fileName)
        {
            return String.Format("{0}_{1}{2}",
                    Path.GetFileNameWithoutExtension(fileName),
                    DateTime.Now.ToString("yyyyMMdd_hhmmss"),
                    Path.GetExtension(fileName));
        }

        public void DeleteFile(string path, string uploadDir)
        {
            if (path != null)
            {
                try
                {
                    File.Delete(Path.Combine(HttpContext.Current.Server.MapPath(uploadDir), path));
                }
                catch (DirectoryNotFoundException)
                {
                }
            }
        }

        public void DeleteDirectory(string path)
        {
            string fullPath = HttpContext.Current.Server.MapPath(path);

            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true);
            }
        }

        #endregion
    }
}
