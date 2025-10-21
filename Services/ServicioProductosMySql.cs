using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySqlConnector;
using ProdLogApp.Models;

namespace ProdLogApp.Servicios
{
    public sealed class ServicioProductosMySql : ServicioBase, IServicioProductos
    {
        public ServicioProductosMySql(IProveedorConexion proveedor) : base(proveedor) { }

        public async Task<IReadOnlyList<Producto>> ListarAsync()
        {
            var lista = new List<Producto>();
            const string sql = @"
SELECT  p.ProductoId,
        p.ProductoNombre,
        p.CategoriaId,
        c.CategoriaNombre,      -- <- nueva columna
        p.Activo
FROM Producto p
LEFT JOIN Categoria c ON c.CategoriaId = p.CategoriaId
ORDER BY p.ProductoNombre;";

            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                lista.Add(new Producto
                {
                    Id = rd.GetInt32("ProductoId"),
                    Nombre = rd.GetString("ProductoNombre"),
                    CategoriaId = rd.GetInt32("CategoriaId"),
                    CategoriaNombre = rd.IsDBNull(rd.GetOrdinal("CategoriaNombre"))
                                        ? null
                                        : rd.GetString("CategoriaNombre"),
                    Activo = rd.GetBoolean("Activo")
                });
            }
            return lista;
        }

        public async Task<IReadOnlyList<Producto>> ListarActivosAsync()
        {
            var lista = new List<Producto>();
            const string sql = @"
SELECT  p.ProductoId,
        p.ProductoNombre,
        p.CategoriaId,
        c.CategoriaNombre,      -- <- nueva columna
        p.Activo
FROM Producto p
LEFT JOIN Categoria c ON c.CategoriaId = p.CategoriaId
WHERE p.Activo = 1
ORDER BY p.ProductoNombre;";

            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                lista.Add(new Producto
                {
                    Id = rd.GetInt32("ProductoId"),
                    Nombre = rd.GetString("ProductoNombre"),
                    CategoriaId = rd.GetInt32("CategoriaId"),
                    CategoriaNombre = rd.IsDBNull(rd.GetOrdinal("CategoriaNombre"))
                                        ? null
                                        : rd.GetString("CategoriaNombre"),
                    Activo = rd.GetBoolean("Activo")
                });
            }
            return lista;
        }

        public async Task<Producto> ObtenerPorIdAsync(int id)
        {
            const string sql = @"
SELECT  p.ProductoId,
        p.ProductoNombre,
        p.CategoriaId,
        c.CategoriaNombre,      -- <- nueva columna
        p.Activo
FROM Producto p
LEFT JOIN Categoria c ON c.CategoriaId = p.CategoriaId
WHERE p.ProductoId=@id;";

            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@id", id);
            using var rd = await cmd.ExecuteReaderAsync();
            if (await rd.ReadAsync())
            {
                return new Producto
                {
                    Id = rd.GetInt32("ProductoId"),
                    Nombre = rd.GetString("ProductoNombre"),
                    CategoriaId = rd.GetInt32("CategoriaId"),
                    CategoriaNombre = rd.IsDBNull(rd.GetOrdinal("CategoriaNombre"))
                                        ? null
                                        : rd.GetString("CategoriaNombre"),
                    Activo = rd.GetBoolean("Activo")
                };
            }
            return null;
        }

        public async Task<int> CrearAsync(Producto p)
        {
            const string sql = @"INSERT INTO Producto (ProductoNombre, CategoriaId, Activo)
                                 VALUES (@n, @cat, @a);
                                 SELECT LAST_INSERT_ID();";
            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@n", p.Nombre);
            cmd.Parameters.AddWithValue("@cat", p.CategoriaId);
            cmd.Parameters.AddWithValue("@a", p.Activo);
            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        public async Task ActualizarAsync(Producto p)
        {
            const string sql = @"UPDATE Producto
                                 SET ProductoNombre=@n, CategoriaId=@cat, Activo=@a
                                 WHERE ProductoId=@id;";
            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@n", p.Nombre);
            cmd.Parameters.AddWithValue("@cat", p.CategoriaId);
            cmd.Parameters.AddWithValue("@a", p.Activo);
            cmd.Parameters.AddWithValue("@id", p.Id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task AlternarEstadoAsync(int id, bool activo)
        {
            const string sql = @"UPDATE Producto SET Activo=@a WHERE ProductoId=@id;";
            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@a", activo);
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarAsync(int id)
        {
            const string sql = @"DELETE FROM Producto WHERE ProductoId=@id;";
            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
