using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Protov4.DAO
{
    public class DbConnection
    {
        private readonly IConfiguration _configuration;

        public DbConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected SqlConnection GetSqlConnection()
        {
            string connectionString = _configuration.GetConnectionString("conexion");
            if (!string.IsNullOrEmpty(connectionString))
            {
                return new SqlConnection(connectionString);
            }
            else
            {
                throw new ApplicationException("La cadena de conexión es nula o vacía.");
            }
        }

    }
}
