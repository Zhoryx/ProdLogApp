using MySqlConnector; // ✅ Conector open-source y sin licencia
using ProdLogApp.Services;
using System;
using System.Data;

namespace ProdLogApp.Models
{
    public class User
    {
        public string Dni { get; set; }
        private string Password { get; set; }
        public bool IsAdmin { get; set; }
        public int Id { get; set; }

        public static User GetByDni(string dni)
        {
            User activeUser = null;

            try
            {
                DatabaseService dbService = new DatabaseService();
                using (var connection = dbService.GetConnection())
                {
                    connection.Open(); // ✅ IMPORTANTE: abrir conexión manualmente

                    string query = "SELECT UsDNI, UsGerente, UsPass, UsId FROM Usuario WHERE UsDNI = @dni";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@dni", dni);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                activeUser = new User
                                {
                                    Dni = reader.GetString("UsDNI"),
                                    Password = reader.IsDBNull("UsPass") ? null : reader.GetString("UsPass"),
                                    IsAdmin = reader.GetBoolean("UsGerente"),
                                    Id = reader.GetInt32("UsId")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching user: {ex.Message}");
            }

            return activeUser;
        }

        public static bool PasswordValidate(User activeUser, string PasswordGet)
        {
            return activeUser?.Password == PasswordGet;
        }
    }
}
