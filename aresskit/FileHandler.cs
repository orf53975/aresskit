using System.Net;

namespace aresskit
{
    class FileHandler
    {
        public static bool downloadFile(string filename, string url)
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

        public static string uploadFile(string filename, string url)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    byte[] responseArray = client.UploadFile(url, filename);
                    return System.Text.Encoding.ASCII.GetString(responseArray);
                }
                catch (WebException)
                {
                    return "Upload failed";
                }
                // File uploaded
            }
        }
    }
}
