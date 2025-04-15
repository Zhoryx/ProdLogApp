using System.Data.SqlClient;

namespace LoginApp.Model
{
    public class UserModel
    {
        private readonly string connectionString = "Data Source=servidor;Initial Catalog=baseDeDatos;User ID=usuario;Password=contraseña";

        public bool Login(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(1) FROM Usuarios WHERE NombreUsuario = @username AND Contraseña = @password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);

                connection.Open();
                int result = (int)command.ExecuteScalar();
                return result > 0;
            }
        }
    }
}
