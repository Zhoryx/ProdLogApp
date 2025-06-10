using Devart.Data.MySql;
using ProdLogApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

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

        public bool SavePartProductions(List<Production> productionList, int userId)
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

                return true;
            }
        }

        public List<Production> GetDailyProductions()
        {
            List<Production> productions = new List<Production>();

            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Producciones WHERE DATE(Produccion_HoraInicio) = CURDATE();";

                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productions.Add(new Production
                        {
                            ProductionId = reader.GetInt32("Produccion_Id"),
                            HInicio = reader.GetTimeSpan("Produccion_HoraInicio"),
                            HFin = reader.GetTimeSpan("Produccion_HoraFin"),
                            Cantidad = reader.GetInt32("Produccion_Cantidad"),
                            ProductoId = reader.GetInt32("Producto_Id"),
                            PuestoId = reader.GetInt32("Puesto_Id")
                        });
                    }
                }
            }

            return productions;
        }

        public void AgregarProductoEnDB(Producto producto)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO Producto (ProductoNombre, CategoriaId) VALUES (@Nombre, @CategoriaId);";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    command.Parameters.AddWithValue("@CategoriaId", producto.CategoriaId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public async Task<List<Categoria>> CategoriesGet(bool soloActivas = false)
        {
            var categorias = new List<Categoria>();

            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                // ✅ Construimos la consulta dinámicamente según el parámetro
                string query = soloActivas
                    ? "SELECT CategoriaId, CategoriaNombre FROM Categoria WHERE Activo = TRUE;"
                    : "SELECT CategoriaId, CategoriaNombre FROM Categoria;";

                using (var command = new MySqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        categorias.Add(new Categoria
                        {
                            Id = reader.GetInt32("CategoriaId"),
                            Nombre = reader.GetString("CategoriaNombre")
                        });
                    }
                }
            }

            return categorias;
        }




        public List<Producto> ObtenerTodosLosProductos()
        {
            var productos = new List<Producto>();

            using (var connection = GetConnection())
            {
                connection.Open();
                string query = @"
            SELECT p.ProductoId, p.ProductoNombre, p.CategoriaId, c.CategoriaNombre, p.Activo 
            FROM Producto p
            INNER JOIN Categoria c ON p.CategoriaId = c.CategoriaId;
        ";

                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productos.Add(new Producto
                        {
                            Id = reader.GetInt32("ProductoId"),
                            Nombre = reader.GetString("ProductoNombre"),
                            CategoriaId = reader.GetInt32("CategoriaId"),
                            CategoriaNombre = reader.GetString("CategoriaNombre"),
                            Activo = reader.GetBoolean("Activo")
                        });
                    }
                }
            }

            return productos;
        }


        public void ModificarProductoEnDB(Producto producto)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "UPDATE Producto SET ProductoNombre = @Nombre, CategoriaId = @CategoriaId WHERE ProductoId = @Id;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    command.Parameters.AddWithValue("@CategoriaId", producto.CategoriaId);
                    command.Parameters.AddWithValue("@Id", producto.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void ToggleProductState(int productoId, bool estado)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "UPDATE Producto SET Activo = @Estado WHERE ProductoId = @ProductoId;";
                Console.WriteLine($"Cambiando estado del producto {productoId} a {estado}");

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Estado", !estado);
                    command.Parameters.AddWithValue("@ProductoId", productoId);
                    command.ExecuteNonQuery();
                }
            }
        }



    }
}

