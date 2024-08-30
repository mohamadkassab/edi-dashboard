using EDI.Controllers;
using EDI.Repositories;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace EDI.Utilities.ItemDataUtilities
{
    public class ImageCopy
    {
        private static string fdsPath = Constants.FDS_PATH;
        private static string hdmPath = Constants.HDM_PATH;
        private static string mainPath = Constants.MAIN_PATH;
        private static string localPath = Constants.LOCAL_PATH;
        private static string jsPath = Constants.JS_PATH;
        private readonly ErrorController _errorController;


        public ImageCopy(IConfiguration configuration)
        {
            _errorController = new ErrorController(configuration);
        }
        public async Task<string>? CopyImage(SqlConnection connection, string imageNo)
        {
            try
            {
                string imageName = imageNo + ".jpg";
                string localNetwork = Path.Combine(localPath, imageName);
                string fdsNetwork = Path.Combine(fdsPath, imageName);
                string hdmNetwork = Path.Combine(hdmPath, imageName);
                string mainNetwork = Path.Combine(mainPath, imageName);

                if (!File.Exists(localNetwork))
                {
                    if (File.Exists(hdmNetwork))
                    {
                        File.Copy(hdmNetwork, localNetwork);
                        return Path.Combine(jsPath, imageName);
                    }
                    else if (File.Exists(mainNetwork))
                    {
                        File.Copy(mainNetwork, localNetwork);
                        return Path.Combine(jsPath, imageName);
                    }
                    else if (File.Exists(fdsNetwork))
                    {
                        File.Copy(fdsNetwork, localNetwork);
                        return Path.Combine(jsPath, imageName);
                    }

                    return null;
                }
                else
                {
                    return Path.Combine(jsPath, imageName); ;
                }
            }
            catch (Exception ex)
            {
                string controllerName = this.GetType().Name;
                string FuntionName = MethodBase.GetCurrentMethod().Name;
                _errorController.Insert(controllerName, FuntionName, ex.Message, ex.StackTrace!);
                return null;
            }
        }
    }
}
