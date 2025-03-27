using Microsoft.Data.SqlClient;

namespace DataAcessLayer.Common
{
    public class MSSQLDataAccessLayer : IDataAccessLayer
    {
        private readonly string connectionString;
        public SqlConnection connection;

        public MSSQLDataAccessLayer()
        {
            connectionString = "server=localhost;database=blogManager;trusted_connection=true;uid=wbpoc;pwd=sql@tfs2008;Encrypt=True;TrustServerCertificate=True";
            connection = new SqlConnection(connectionString);
            OpenConnection();
        }

        public async Task OpenConnectionAsync()
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
                await connection.OpenAsync();
            }
            catch (SqlException ex)
            {
                throw;
            }
        }

        public void OpenConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
        }

        public void CloseConnection()
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
                connection.Dispose();
            }
        }
    }
}
