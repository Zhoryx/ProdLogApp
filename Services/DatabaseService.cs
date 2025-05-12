using System;
using Devart.Data.MySql;

namespace ProdLogApp.Services
{
    public class DatabaseService
    {
        private const string ConnectionString = "Server=localhost;Port=3306;Database=users_db;User Id=admin;Password=admin123;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}
