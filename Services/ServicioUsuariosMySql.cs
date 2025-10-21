using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySqlConnector;
using ProdLogApp.Models;

namespace ProdLogApp.Servicios
{
    public sealed class ServicioUsuariosMySql : ServicioBase, IServicioUsuarios
    {
        public ServicioUsuariosMySql(IProveedorConexion proveedor) : base(proveedor) { }

        public async Task<IReadOnlyList<Usuario>> ListarAsync()
        {
            var lista = new List<Usuario>();
            const string sql = @"SELECT UsId, UsNombre, UsDNI, UsGerente, UsFechaIngreso, UsActivo FROM Usuario ORDER BY UsNombre;";
            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                lista.Add(new Usuario
                {
                    Id = rd.GetInt32("UsId"),
                    Nombre = rd.GetString("UsNombre"),
                    Dni = rd.GetString("UsDNI"),
                    EsGerente = rd.GetBoolean("UsGerente"),
                    FechaIngreso = rd.IsDBNull(rd.GetOrdinal("UsFechaIngreso")) ? (DateTime?)null : rd.GetDateTime("UsFechaIngreso"),
                    Activo = rd.GetBoolean("UsActivo")
                });
            }
            return lista;
        }
        public async Task<Usuario> LoginAsync(string dni, string passwordPlano)
        {
            var u = await ObtenerPorDniAsync(dni);
            if (u == null || !u.Activo) return null;

            if (string.IsNullOrWhiteSpace(passwordPlano))
                return u; // operario sin password

            return (u.PasswordHash == passwordPlano) ? u : null;
        }

       


        public async Task<Usuario> ObtenerPorDniAsync(string dni)
        {
            if (dni == null) return null;
            dni = dni.Trim();

            // Normalizamos DNI a 8 caracteres con ceros a la izquierda,
            // para evitar mismatches por “1234567” vs “01234567”.
            if (dni.Length < 8) dni = dni.PadLeft(8, '0');

            const string sql = @"
SELECT UsId, UsNombre, UsDNI, UsGerente, UsFechaIngreso, UsActivo, UsPass
FROM Usuario
WHERE UsDNI = @dni;";

            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@dni", dni);

            using var rd = await cmd.ExecuteReaderAsync();
            if (await rd.ReadAsync())
            {
                return new Usuario
                {
                    Id = rd.GetInt32("UsId"),
                    Nombre = rd.GetString("UsNombre"),
                    Dni = rd.GetString("UsDNI"),
                    EsGerente = rd.GetBoolean("UsGerente"),
                    FechaIngreso = rd.IsDBNull(rd.GetOrdinal("UsFechaIngreso")) ? (DateTime?)null : rd.GetDateTime("UsFechaIngreso"),
                    Activo = rd.GetBoolean("UsActivo"),
                    PasswordHash = rd.IsDBNull(rd.GetOrdinal("UsPass")) ? "" : (rd.GetString("UsPass") ?? "")
                };
            }
            return null;
        }

       



       

        public async Task<int> CrearAsync(Usuario u, string passwordPlano)
        {
            const string sql = @"INSERT INTO Usuario (UsNombre, UsDNI, UsGerente, UsFechaIngreso, UsActivo, UsPass)
                                 VALUES (@n, @dni, @g, @fi, @a, @p);
                                 SELECT LAST_INSERT_ID();";
            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@n", u.Nombre);
            cmd.Parameters.AddWithValue("@dni", u.Dni);
            cmd.Parameters.AddWithValue("@g", u.EsGerente);
            cmd.Parameters.AddWithValue("@fi", (object?)u.FechaIngreso ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@a", u.Activo);
            cmd.Parameters.AddWithValue("@p", passwordPlano);
            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        public async Task ActualizarAsync(Usuario u)
        {
            const string sql = @"UPDATE Usuario
                                 SET UsNombre=@n, UsDNI=@dni, UsGerente=@g, UsFechaIngreso=@fi, UsActivo=@a
                                 WHERE UsId=@id;";
            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@n", u.Nombre);
            cmd.Parameters.AddWithValue("@dni", u.Dni);
            cmd.Parameters.AddWithValue("@g", u.EsGerente);
            cmd.Parameters.AddWithValue("@fi", (object?)u.FechaIngreso ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@a", u.Activo);
            cmd.Parameters.AddWithValue("@id", u.Id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task AlternarEstadoAsync(int id, bool activo)
        {
            const string sql = @"UPDATE Usuario SET UsActivo=@a WHERE UsId=@id;";
            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@a", activo);
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task CambiarPasswordAsync(int id, string passwordPlano)
        {
            const string sql = @"UPDATE Usuario SET UsPass=@p WHERE UsId=@id;";
            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@p", passwordPlano);
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
