using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySqlConnector;
using ProdLogApp.Models;

namespace ProdLogApp.Servicios
{
    public sealed class ServicioCategoriasMySql : ServicioBase, IServicioCategorias
    {
        public ServicioCategoriasMySql(IProveedorConexion proveedorConexion) : base(proveedorConexion) { }

        public async Task<IReadOnlyList<Categoria>> ListarAsync()
        {
            var lista = new List<Categoria>();
            const string sql = @"SELECT CategoriaId, CategoriaNombre, Activo
                                 FROM Categoria
                                 ORDER BY CategoriaNombre ASC;";
            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                lista.Add(new Categoria
                {
                    CategoriaId = rd.GetInt32("CategoriaId"),
                    Nombre = rd.GetString("CategoriaNombre"),
                    Activo = rd.GetBoolean("Activo")
                });
            }
            return lista;
        }

        public async Task<Categoria> ObtenerPorIdAsync(int id)
        {
            const string sql = @"SELECT CategoriaId, CategoriaNombre, Activo
                                 FROM Categoria WHERE CategoriaId=@id;";
            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@id", id);
            using var rd = await cmd.ExecuteReaderAsync();
            if (await rd.ReadAsync())
            {
                return new Categoria
                {
                    CategoriaId = rd.GetInt32("CategoriaId"),
                    Nombre = rd.GetString("CategoriaNombre"),
                    Activo = rd.GetBoolean("Activo")
                };
            }
            return null;
        }

        public async Task<int> CrearAsync(Categoria categoria)
        {
            if (categoria == null) throw new ArgumentNullException(nameof(categoria));
            const string sql = @"INSERT INTO Categoria (CategoriaNombre, Activo)
                                 VALUES (@nombre, @activo);
                                 SELECT LAST_INSERT_ID();";
            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@nombre", categoria.Nombre);
            cmd.Parameters.AddWithValue("@activo", categoria.Activo);
            var id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return id;
        }

        public async Task ActualizarAsync(Categoria categoria)
        {
            if (categoria == null) throw new ArgumentNullException(nameof(categoria));
            const string sql = @"UPDATE Categoria
                                 SET CategoriaNombre=@nombre, Activo=@activo
                                 WHERE CategoriaId=@id;";
            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@nombre", categoria.Nombre);
            cmd.Parameters.AddWithValue("@activo", categoria.Activo);
            cmd.Parameters.AddWithValue("@id", categoria.CategoriaId);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task AlternarEstadoAsync(int id, bool activo)
        {
            const string sql = @"UPDATE Categoria SET Activo=@activo WHERE CategoriaId=@id;";
            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@activo", activo);
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarAsync(int id)
        {
            const string sql = @"DELETE FROM Categoria WHERE CategoriaId=@id;";
            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null)
        {
            const string sql = @"SELECT COUNT(1) FROM Categoria
                                 WHERE CategoriaNombre=@nombre
                                 AND (@excluirId IS NULL OR CategoriaId<>@excluirId);";
            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@excluirId", (object?)excluirId ?? DBNull.Value);
            var cnt = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return cnt > 0;
        }
    }
}
