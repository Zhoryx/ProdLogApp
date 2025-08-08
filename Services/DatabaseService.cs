using MySqlConnector;
using ProdLogApp.Models;
using ProdLogApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace ProdLogApp.Services
{
    public class DatabaseService : IDatabaseService
    {
        //conexion
        private const string ConnectionString = "Server=127.0.0.1;Port=3307;Database=ProdLog_BD;User Id=root;Password=Madersa;";

        public MySqlConnection GetConnection()
        {
            try
            {
                return new MySqlConnection(ConnectionString);
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
                connection.Open();
                return connection.State == ConnectionState.Open;
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

                query.Length--; // quitar última coma

                using (var command = new MySqlCommand(query.ToString(), connection))
                {
                    command.ExecuteNonQuery();
                }

                return true;
            }
        }
        //productos
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

        public List<Producto> ObtenerTodosLosProductos()
        {
            var productos = new List<Producto>();

            using (var connection = GetConnection())
            {
                connection.Open();
                string query = @"
                    SELECT p.ProductoId, p.ProductoNombre, p.CategoriaId, c.CategoriaNombre, p.Activo 
                    FROM Producto p
                    INNER JOIN Categoria c ON p.CategoriaId = c.CategoriaId;";

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

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Estado", !estado);
                    command.Parameters.AddWithValue("@ProductoId", productoId);
                    command.ExecuteNonQuery();
                }
            }
        }



        //Categorias
        public async Task<List<Categoria>> CategoriesGet(bool soloActivas = false)
        {
            var categorias = new List<Categoria>();

            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                string query = soloActivas
                    ? "SELECT CategoriaId, CategoriaNombre, Activo FROM Categoria WHERE Activo = TRUE;"
                    : "SELECT CategoriaId, CategoriaNombre, Activo FROM Categoria;";

                using (var command = new MySqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        categorias.Add(new Categoria
                        {
                            CategoryId = reader.GetInt32("CategoriaId"),
                            Nombre = reader.GetString("CategoriaNombre"),
                            Activo = reader.GetBoolean("Activo")

                        });
                    }
                }
            }

            return categorias;
        }


        public void ToggleCategoryStatus(int CategoryId, bool estado)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "UPDATE Categoria SET Activo = @Estado WHERE CategoriaId = @CategoryId;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Estado", estado);
                    command.Parameters.AddWithValue("@CategoryId", CategoryId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public async Task AgregarCategoria(Categoria categoria)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                string query = "INSERT INTO Categoria (CategoriaNombre) VALUES (@Nombre);";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task ActualizarCategoria(Categoria categoria)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                string query = "UPDATE Categoria SET CategoriaNombre = @Nombre WHERE CategoriaId = @Id;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                    command.Parameters.AddWithValue("@Id", categoria.CategoryId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        // Puestos
        public void AgregarPuesto(Position puesto)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO Puesto (Nombre) VALUES (@Nombre);";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", puesto.Nombre);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarPuesto(Position puesto)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "UPDATE Puesto SET Nombre = @Nombre WHERE PuestoId = @Id;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", puesto.Nombre);
                    command.Parameters.AddWithValue("@Id", puesto.PuestoId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Position> ObtenerTodosLosPuestos()
        {
            var puestos = new List<Position>();

            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT PuestoId, Nombre, Activo FROM Puesto;";

                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        puestos.Add(new Position
                        {
                            PuestoId = reader.GetInt32("PuestoId"),
                            Nombre = reader.GetString("Nombre"),
                            Activo = reader.GetBoolean("Activo")
                        });
                    }
                }
            }

            return puestos;
        }

        public List<Position> ObtenerPuestosActivos()
        {
            var puestos = new List<Position>();

            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT PuestoId, Nombre, Activo FROM Puesto WHERE Activo = TRUE;";

                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        puestos.Add(new Position
                        {
                            PuestoId = reader.GetInt32("PuestoId"),
                            Nombre = reader.GetString("Nombre"),
                            Activo = reader.GetBoolean("Activo")
                        });
                    }
                }
            }

            return puestos;
        }

        public List<Position> ObtenerPuestos()
        {
            // Delegamos en ObtenerPuestosActivos y ordenamos por nombre
            return ObtenerPuestosActivos()
                .OrderBy(p => p.Nombre)
                .ToList();
        }

        public void TogglePositionState(int puestoId, bool estado)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "UPDATE Puesto SET Activo = @NuevoEstado WHERE PuestoId = @Id;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NuevoEstado", !estado);
                    command.Parameters.AddWithValue("@Id", puestoId);
                    command.ExecuteNonQuery();
                }
            }
        }

        //Usuarios

        public async Task<List<User>> UsersGet(bool soloActivas = false)
        {
            var users = new List<User>();

            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                string query = "SELECT UsId, UsNombre, UsDNI, UsGerente, UsFechaIngreso, UsActivo FROM Usuario";
                if (soloActivas)
                    query += " WHERE UsActivo = TRUE;";



                using (var command = new MySqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        users.Add(new User
                        {
                            Id = reader.GetInt32("UsId"),
                            Name = reader.GetString("UsNombre"),   
                            Dni = reader.GetString("UsDNI"),      
                            IsAdmin = reader.GetBoolean("UsGerente"),
                            Fingreso = reader.GetDateOnly("UsFechaIngreso"),
                            Active = reader.GetBoolean("UsActivo")
                        });

                    }
                }
            }

            return users;
        }
        public async Task AddUser(User user) { }

        public async Task UpdateUser(User user) { }
        
    }
}
