using System;
using System.Net;

namespace aresskit
{
    class FileHandler
    {
        public bool downloadFile(string filename, string url)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.DownloadFile(url, filename);
                }
                catch (WebException)
                {
                    return false;
                }
                // File downloaded
                return true;
            }
        }

        public bool uploadFile(string filename, string url)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.UploadFile(url, filename);
                }
                catch (WebException)
                {
                    return false;
                }
                // File uploaded
                return true;
            }
        }
    }
}
