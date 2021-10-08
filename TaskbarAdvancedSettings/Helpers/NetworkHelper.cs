using System.Net;
using System.Runtime.InteropServices;

namespace TaskbarAdvancedSettings.Helpers
{
    public static class NetworkHelper
    {
        public const string AdvSettingsGithubLink = "https://github.com/KRtkovo-eu/Advanced-Settings-for-Windows/releases/latest";

        [DllImport("wininet.dll")]
        public extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        public static bool IsConnectedToInternet()
        {
            bool returnValue = false;
            try
            {

                int Desc;
                returnValue = InternetGetConnectedState(out Desc, 0);
            }
            catch
            {
                returnValue = false;
            }
            return returnValue;
        }
        public static string CheckUpdate()
        {
            string result = "latest";

            try
            {
                if (IsConnectedToInternet())
                {
                    WebRequest req = HttpWebRequest.Create(AdvSettingsGithubLink);
                    string resUri;
                    resUri = req.GetResponse().ResponseUri.AbsoluteUri;
                    result = resUri.Substring(resUri.LastIndexOf("/") + 1);
                }
            }
            catch
            {
            }

            return result;
        }
    }
}
