using System;
using System.Text;
using Devart.Data.MySql;
using ProdLogApp.Models;

namespace ProdLogApp.Services
{
    public class DatabaseService : IDatabaseService
    {
        private const string ConnectionString = "Server=127.0.0.1;Port=3307;Database=ProdLog_BD;User Id=root;Password=Madersa;";

        public MySqlConnection GetConnection()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(ConnectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                throw new Exception($"Database connection failed: {ex.Message}");
            }
        }
                         
        public bool TestConnection()
        {
            using (var connection = GetConnection())
            {
                return connection != null;
            }
        }

        public void SavePartProductions(List<Production> productionList, int userId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();


                string insertPartQuery = "INSERT INTO Parte (Parte_Fecha, Usuario_Id) VALUES (@Fecha, @UsuarioId); SELECT LAST_INSERT_ID();";
                int partId;

                using (var command = new MySqlCommand(insertPartQuery, connection))
                {
                    command.Parameters.AddWithValue("@Fecha", DateTime.Now.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@UsuarioId", userId);
                    partId = Convert.ToInt32(command.ExecuteScalar());
                }


                StringBuilder query = new StringBuilder();
                query.Append("INSERT INTO Producciones (Produccion_HoraInicio, Produccion_HoraFin, Produccion_Cantidad, Producto_Id, Puesto_Id, Parte_Id) VALUES ");

                foreach (var production in productionList)
                {
                    query.AppendFormat("('{0}', '{1}', {2}, {3}, {4}, {5}),",
                        production.HInicio.ToString(@"hh\:mm\:ss"),
                        production.HFin.ToString(@"hh\:mm\:ss"),
                        production.Cantidad,
                        production.ProductoId,
                        production.PuestoId,
                        partId);
                }

                query.Length--;

                using (var command = new MySqlCommand(query.ToString(), connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Production> GetDailyProductions()
        {
            List<Production> productions = new List<Production>();

        //    using (var connection = GetConnection())
        //    {
        //        connection.Open();
        //        string query = "SELECT * FROM Producciones WHERE DATE(Produccion_HoraInicio) = CURDATE();";

        //        using (var command = new MySqlCommand(query, connection))
        //        using (var reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                productions.Add(new Production
        //                {
        //                    ProductionId = reader.GetInt32("Produccion_Id"),
        //                    HInicio = reader.GetTimeSpan("Produccion_HoraInicio"),
        //                    HFin = reader.GetTimeSpan("Produccion_HoraFin"),
        //                    Cantidad = reader.GetInt32("Produccion_Cantidad"),
        //                    ProductoId = reader.GetInt32("Producto_Id"),
        //                    PuestoId = reader.GetInt32("Puesto_Id")
        //                });
        //            }
        //        }
        //    }

           return productions;
        }

        bool IDatabaseService.SavePartProductions(List<Production> productions, int userId)
        {
            throw new NotImplementedException();
        }
    }
}