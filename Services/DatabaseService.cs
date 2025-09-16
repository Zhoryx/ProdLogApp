using MySqlConnector;
using ProdLogApp.Models;
using ProdLogApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;              // NECESARIO por .OrderBy
using System.Text;
using System.Threading;        // NECESARIO por CancellationToken
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

        // =========================
        // Productos
        // =========================
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

        // =========================
        // Categorías
        // =========================
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

        // =========================
        // Puestos
        // =========================
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

        // =========================
        // Usuarios
        // =========================
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

        // =========================
        // Parte & Producciones
        // =========================
        public async Task<Page<ParteHeaderItem>> GetParteHeadersPageAsync(
    DateTime from, DateTime to, int page, int pageSize, string operarioLike = null)
        {
            var result = new Page<ParteHeaderItem> { PageNumber = page, PageSize = pageSize };

            using var conn = GetConnection();
            await conn.OpenAsync();

            const string countSql = @"
            SELECT COUNT(DISTINCT pa.Parte_Id)
            FROM Parte pa
            JOIN Usuario u ON u.UsId = pa.Usuario_Id
            WHERE pa.Parte_Fecha BETWEEN @from AND @to
              AND (@operarioLike IS NULL OR u.UsNombre LIKE CONCAT('%', @operarioLike, '%'));";

            using (var countCmd = new MySqlCommand(countSql, conn))
            {
                countCmd.Parameters.AddWithValue("@from", from);
                countCmd.Parameters.AddWithValue("@to", to);
                countCmd.Parameters.AddWithValue("@operarioLike", (object?)operarioLike ?? DBNull.Value);
                result.Total = Convert.ToInt32(await countCmd.ExecuteScalarAsync());
            }

            var offset = (page - 1) * pageSize;

            const string dataSql = @"
                    SELECT
                pa.Parte_Id AS ParteId,
                pa.Parte_Fecha AS ParteFecha,
                pa.Usuario_Id AS UserId,
                u.UsNombre AS Operario,
                COUNT(pr.Produccion_Id) AS CantProducciones,
                COALESCE(SUM(pr.Produccion_Cantidad), 0) AS TotalCantidad
            FROM Parte pa
            INNER JOIN Usuario u ON u.UsId = pa.Usuario_Id
            LEFT JOIN Producciones pr ON pr.Parte_Id = pa.Parte_Id
            WHERE pa.Parte_Fecha BETWEEN @from AND @to
              AND (@operarioLike IS NULL OR u.UsNombre LIKE CONCAT('%', @operarioLike, '%'))
            GROUP BY pa.Parte_Id, pa.Parte_Fecha, pa.Usuario_Id, u.UsNombre
            ORDER BY pa.Parte_Fecha DESC, pa.Parte_Id DESC
            LIMIT @pageSize OFFSET @offset;";

            using (var cmd = new MySqlCommand(dataSql, conn))
            {
                cmd.Parameters.AddWithValue("@from", from);
                cmd.Parameters.AddWithValue("@to", to);
                cmd.Parameters.AddWithValue("@operarioLike", (object?)operarioLike ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@offset", offset);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    result.Items.Add(new ParteHeaderItem
                    {
                        ParteId = reader.GetInt32("ParteId"),
                        FechaParte = reader.GetDateTime("ParteFecha"),
                        UserId = reader.GetInt32("UserId"),
                        Operario = reader["Operario"]?.ToString() ?? "",
                        CantProducciones = reader.GetInt32("CantProducciones"),
                        TotalCantidad = reader.GetDecimal("TotalCantidad")
                    });
                }
            }

            return result;
        }



        public async Task<int> EnsureParteAsync(int usuarioId, DateTime fecha)
        {
            const string sql = @"
            INSERT INTO Parte (Usuario_Id, Parte_Fecha)
            VALUES (@UsuarioId, @Fecha)
            ON DUPLICATE KEY UPDATE Parte_Id = LAST_INSERT_ID(Parte_Id);
            SELECT LAST_INSERT_ID();";

            await using var conn = GetConnection();
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.Add("@UsuarioId", MySqlDbType.Int32).Value = usuarioId;
            cmd.Parameters.Add("@Fecha", MySqlDbType.Date).Value = fecha.Date;

            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        public async Task<int> InsertProduccionAsync(Production prod)
        {
            // Doble red: si no te pasaron ParteId, asegurá uno (ideal es pasarlo desde el presenter)
            if (prod.ParteId <= 0)
                prod.ParteId = await EnsureParteAsync(UserSession.GetInstance().ActiveUser.Id, DateTime.Today);

            const string sql = @"
            INSERT INTO Producciones
            (Produccion_HoraInicio, Produccion_HoraFin, Produccion_Cantidad,
             Producto_Id, Puesto_Id, Parte_Id)
            VALUES (@HInicio, @HFin, @Cantidad, @ProductoId, @PuestoId, @ParteId);
            SELECT LAST_INSERT_ID();";

            await using var conn = GetConnection();
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.Add("@HInicio", MySqlDbType.Time).Value = prod.HInicio;
            cmd.Parameters.Add("@HFin", MySqlDbType.Time).Value = prod.HFin;
            cmd.Parameters.Add("@Cantidad", MySqlDbType.Int32).Value = prod.Cantidad;
            cmd.Parameters.Add("@ProductoId", MySqlDbType.Int32).Value = prod.ProductoId;
            cmd.Parameters.Add("@PuestoId", MySqlDbType.Int32).Value = prod.PuestoId;
            cmd.Parameters.Add("@ParteId", MySqlDbType.Int32).Value = prod.ParteId;

            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        public async Task<List<Production>> GetProductionsAsync(int usuarioId, DateTime fecha)
        {
            var list = new List<Production>();

            const string sql = @"
                SELECT p.Produccion_Id,
                       p.Produccion_HoraInicio,
                       p.Produccion_HoraFin,
                       p.Produccion_Cantidad,
                       p.Producto_Id,
                       pr.ProductoNombre      AS ProductoNombre,
                       p.Puesto_Id,
                       pu.Nombre              AS PuestoDescripcion,
                       p.Parte_Id
                  FROM Producciones p
                INNER JOIN Parte pa   ON p.Parte_Id   = pa.Parte_Id
                INNER JOIN Producto pr ON p.Producto_Id = pr.ProductoId
                INNER JOIN Puesto   pu ON p.Puesto_Id   = pu.PuestoId
                 WHERE pa.Usuario_Id = @UsuarioId
                   AND pa.Parte_Fecha = @Fecha
              ORDER BY p.Produccion_HoraInicio;";

            await using var conn = GetConnection();
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.Add("@UsuarioId", MySqlDbType.Int32).Value = usuarioId;
            cmd.Parameters.Add("@Fecha", MySqlDbType.Date).Value = fecha.Date;

            await using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                list.Add(new Production
                {
                    ProductionId = rd.GetInt32("Produccion_Id"),
                    HInicio = rd.GetTimeSpan("Produccion_HoraInicio"),
                    HFin = rd.GetTimeSpan("Produccion_HoraFin"),
                    Cantidad = rd.GetInt32("Produccion_Cantidad"),
                    ProductoId = rd.GetInt32("Producto_Id"),
                    PuestoId = rd.GetInt32("Puesto_Id"),
                    ParteId = rd.GetInt32("Parte_Id"),
                    ProductoNombre = rd.GetString("ProductoNombre"),
                    PuestoDescripcion = rd.GetString("PuestoDescripcion")
                });
            }

            return list;
        }

        // Async recomendado con FECHA: liga todas las filas al mismo Parte_Id
        public async Task<bool> SavePartProductionsAsync(List<Production> productions, int userId, DateTime fecha, CancellationToken ct = default)
        {
            if (productions is null || productions.Count == 0) return true;

            int parteId = await EnsureParteAsync(userId, fecha.Date);

            await using var conn = GetConnection();
            await conn.OpenAsync(ct);
            await using var tx = await conn.BeginTransactionAsync(ct);

            const string sql = @"
                INSERT INTO Producciones
                    (Produccion_HoraInicio, Produccion_HoraFin, Produccion_Cantidad,
                     Producto_Id, Puesto_Id, Parte_Id)
                VALUES
                    (@HInicio, @HFin, @Cantidad, @ProductoId, @PuestoId, @ParteId);";

            try
            {
                await using var cmd = new MySqlCommand(sql, conn, (MySqlTransaction)tx);
                cmd.Parameters.Add("@HInicio", MySqlDbType.Time);
                cmd.Parameters.Add("@HFin", MySqlDbType.Time);
                cmd.Parameters.Add("@Cantidad", MySqlDbType.Int32);
                cmd.Parameters.Add("@ProductoId", MySqlDbType.Int32);
                cmd.Parameters.Add("@PuestoId", MySqlDbType.Int32);
                cmd.Parameters.Add("@ParteId", MySqlDbType.Int32);

                foreach (var p in productions)
                {
                    cmd.Parameters["@HInicio"].Value = p.HInicio;
                    cmd.Parameters["@HFin"].Value = p.HFin;
                    cmd.Parameters["@Cantidad"].Value = p.Cantidad;
                    cmd.Parameters["@ProductoId"].Value = p.ProductoId;
                    cmd.Parameters["@PuestoId"].Value = p.PuestoId;
                    cmd.Parameters["@ParteId"].Value = parteId;

                    await cmd.ExecuteNonQueryAsync(ct);
                }

                await tx.CommitAsync(ct);
                return true;
            }
            catch
            {
                await tx.RollbackAsync(CancellationToken.None);
                throw;
            }
        }

        // Overload async sin fecha (compatibilidad, usa Today)
        public async Task<bool> SavePartProductionsAsync(List<Production> productions, int userId, CancellationToken ct = default)
        {
            return await SavePartProductionsAsync(productions, userId, DateTime.Today, ct);
        }

        // Wrapper SINCRÓNICO requerido por la interfaz (y usado por tu presenter actual)
        public bool SavePartProductions(List<Production> productions, int userId)
        {
            return SavePartProductionsAsync(productions, userId, DateTime.Today).GetAwaiter().GetResult();
        }

        // Lecturas varias
        public async Task<List<Production>> GetDailyProductionsAsync(CancellationToken ct = default)
        {
            var list = new List<Production>();

            await using var conn = GetConnection();
            await conn.OpenAsync(ct);

            const string sql = @"
                SELECT ProductoId, PuestoId, HInicio, HFin, Cantidad
                FROM Producciones
                WHERE DATE(FechaCarga) = CURDATE();";

            await using var cmd = new MySqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync(ct);

            while (await reader.ReadAsync(ct))
            {
                list.Add(new Production
                {
                    ProductoId = reader.GetInt32("ProductoId"),
                    PuestoId = reader.GetInt32("PuestoId"),
                    HInicio = reader.GetTimeSpan("HInicio"),
                    HFin = reader.GetTimeSpan("HFin"),
                    Cantidad = reader.GetInt32("Cantidad")
                });
            }

            return list;
        }

        public List<Production> GetDailyProductions()
        {
            return GetDailyProductionsAsync().GetAwaiter().GetResult();
        }

        public async Task<int> ConfirmarProduccionAsync(Production production, int userId, CancellationToken ct = default)
        {
            if (production == null) throw new ArgumentNullException(nameof(production));
            await using var conn = GetConnection();
            await conn.OpenAsync(ct);

            const string sql = @"
            INSERT INTO Producciones
            (Producto_Id, Puesto_Id, Produccion_HoraInicio, Produccion_HoraFin, Produccion_Cantidad)
            VALUES
            (@ProductoId, @PuestoId, @HInicio, @HFin, @Cantidad);";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ProductoId", production.ProductoId);
            cmd.Parameters.AddWithValue("@PuestoId", production.PuestoId);
            cmd.Parameters.AddWithValue("@HInicio", production.HInicio);
            cmd.Parameters.AddWithValue("@HFin", production.HFin);
            cmd.Parameters.AddWithValue("@Cantidad", production.Cantidad);

            await cmd.ExecuteNonQueryAsync(ct);
            return (int)cmd.LastInsertedId;
        }

        public async Task<Production?> GetProductionByIdAsync(int productionId)
        {
            const string sql = @"
        SELECT p.Produccion_Id,
               p.Produccion_HoraInicio,
               p.Produccion_HoraFin,
               p.Produccion_Cantidad,
               p.Producto_Id,
               pr.ProductoNombre      AS ProductoNombre,
               p.Puesto_Id,
               pu.Nombre              AS PuestoDescripcion,
               p.Parte_Id
          FROM Producciones p
          LEFT JOIN Producto pr ON p.Producto_Id = pr.ProductoId
          LEFT JOIN Puesto   pu ON p.Puesto_Id   = pu.PuestoId
         WHERE p.Produccion_Id = @Id;";

            await using var conn = GetConnection();
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = productionId;

            await using var rd = await cmd.ExecuteReaderAsync();
            if (await rd.ReadAsync())
            {
                return new Production
                {
                    ProductionId = rd.GetInt32("Produccion_Id"),
                    HInicio = rd.GetTimeSpan("Produccion_HoraInicio"),
                    HFin = rd.GetTimeSpan("Produccion_HoraFin"),
                    Cantidad = rd.GetInt32("Produccion_Cantidad"),
                    ProductoId = rd.GetInt32("Producto_Id"),
                    PuestoId = rd.GetInt32("Puesto_Id"),
                    ParteId = rd.GetInt32("Parte_Id"),
                    ProductoNombre = rd["ProductoNombre"] is DBNull ? null : rd.GetString("ProductoNombre"),
                    PuestoDescripcion = rd["PuestoDescripcion"] is DBNull ? null : rd.GetString("PuestoDescripcion"),
                };
            }

            return null;
        }

        public async Task UpdateProduccionAsync(Production prod)
        {
            const string sql = @"
        UPDATE Producciones
           SET Produccion_HoraInicio = @HInicio,
               Produccion_HoraFin    = @HFin,
               Produccion_Cantidad   = @Cantidad,
               Producto_Id           = @ProductoId,
               Puesto_Id             = @PuestoId
         WHERE Produccion_Id         = @Id;";

            await using var conn = GetConnection();
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.Add("@HInicio", MySqlDbType.Time).Value = prod.HInicio;
            cmd.Parameters.Add("@HFin", MySqlDbType.Time).Value = prod.HFin;
            cmd.Parameters.Add("@Cantidad", MySqlDbType.Int32).Value = prod.Cantidad;
            cmd.Parameters.Add("@ProductoId", MySqlDbType.Int32).Value = prod.ProductoId;
            cmd.Parameters.Add("@PuestoId", MySqlDbType.Int32).Value = prod.PuestoId;
            cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = prod.ProductionId;

            var rows = await cmd.ExecuteNonQueryAsync();
            if (rows == 0)
                throw new InvalidOperationException("No se encontró la producción a actualizar.");
        }

        public async Task<Producto?> GetProductoByIdAsync(int productoId)
        {
            const string sql = @"
        SELECT ProductoId, ProductoNombre, CategoriaId, Activo
          FROM Producto
         WHERE ProductoId = @Id;";

            await using var conn = GetConnection();
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = productoId;

            await using var rd = await cmd.ExecuteReaderAsync();
            if (await rd.ReadAsync())
            {
                return new Producto
                {
                    Id = rd.GetInt32("ProductoId"),
                    Nombre = rd.GetString("ProductoNombre"),
                    CategoriaId = rd.GetInt32("CategoriaId"),
                    Activo = rd.GetBoolean("Activo")
                };
            }
            return null;
        }

        public async Task<Position?> GetPuestoByIdAsync(int puestoId)
        {
            const string sql = @"
        SELECT PuestoId, Nombre, Activo
          FROM Puesto
         WHERE PuestoId = @Id;";

            await using var conn = GetConnection();
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = puestoId;

            await using var rd = await cmd.ExecuteReaderAsync();
            if (await rd.ReadAsync())
            {
                return new Position
                {
                    PuestoId = rd.GetInt32("PuestoId"),
                    Nombre = rd.GetString("Nombre"),
                    Activo = rd.GetBoolean("Activo")
                };
            }
            return null;
        }

        public async Task DeleteProduccionAsync(int produccionId)
        {
            const string sql = @"DELETE FROM Producciones WHERE Produccion_Id = @Id;";
            await using var conn = GetConnection();
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = produccionId;

            var rows = await cmd.ExecuteNonQueryAsync();
            if (rows == 0)
                throw new InvalidOperationException("No se encontró la producción a eliminar.");
        }
    }
}
