using Microsoft.Data.SqlClient;

namespace WebApplication1.Models
{
    public class Connection
    {
        public static SqlConnection ServerConnect
        {
            //TrustServerCertificate=True;
            get
            {
                SqlConnection sqlcon = new SqlConnection("Server=94.73.170.25;Database=u2014938_dbF2E;TrustServerCertificate=True;User ID=u2014938_userF2E;Password=woipIvMes01981!");

                return sqlcon;
            }
        }
    }
}
