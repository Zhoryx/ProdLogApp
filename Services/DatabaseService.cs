using System;
using System.Windows; // Para mostrar MessageBox en aplicaciones WPF
using Devart.Data.MySql;

namespace ProdLogApp.Services
{
    public class DatabaseService
    {
        private const string ConnectionString = "Server=127.0.0.1;Port=3307;Database=ProdLog_BD;User Id=root;Password=Madersa;";

        public static MySqlConnection GetConnection()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(ConnectionString);
                connection.Open(); // Intenta abrir la conexión
                return connection; // Devuelve la conexión si fue exitosa
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con la base de datos: {ex.Message}", "Error de Conexión", MessageBoxButton.OK, MessageBoxImage.Error);
                return null; // Retorna `null` si la conexión falla
            }
        }
    }
}
