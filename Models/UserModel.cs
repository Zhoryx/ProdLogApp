using System.Windows.Controls;
using Devart.Data.MySql;
using ProdLogApp.Services;

namespace ProdLogApp.Models
{
    public class User
    {
        public required string Dni { get; set; }
        private string Password { get; set; }
        public bool IsAdmin { get; set; }

        public static User GetByDni(string dni)
        {
            User activeUser = null;

            using (var connection = DatabaseService.GetConnection()) 
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Dni, IsAdmin, Password FROM Users WHERE Dni = @dni";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.Add(new MySqlParameter("@dni", dni));

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                activeUser = new User
                                {
                                    Dni = reader["Dni"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    IsAdmin = Convert.ToBoolean(reader["IsAdmin"])
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Buscando el Usuario: {ex.Message}");
                }
            }

            return activeUser;
        }

        public static bool PasswordValidate(User activeUser, string PasswordGet) {
            if (activeUser.Password == PasswordGet) return true; else return false;
        }
    }
}
