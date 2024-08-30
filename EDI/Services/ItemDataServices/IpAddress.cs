using EDI.Repositories;
using Microsoft.Data.SqlClient;
using System.Net;

namespace EDI.Utilities.ItemDataUtilities
{
    public class IpAddress
    {
        public  string Get()
        {
            try
            {
                string hostName = Dns.GetHostName();
                string ip = Dns.GetHostByName(hostName).AddressList[0].ToString();
                string ipAddress = hostName + "/" + ip;
                return ipAddress;
            }
            catch (Exception ex)
            {
                return null!;
            }
        }
    }
}
