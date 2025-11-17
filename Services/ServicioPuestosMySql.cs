using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySqlConnector;
using ProdLogApp.Models;

namespace ProdLogApp.Servicios
{
    public sealed class ServicioPuestosMySql : ServicioBase, IServicioPuestos
    {
        public ServicioPuestosMySql(IProveedorConexion proveedor) : base(proveedor) { }

        //  Lista todos los puestos (activos e inactivos)
        public async Task<IReadOnlyList<Puesto>> ListarAsync()
        {
            var lista = new List<Puesto>();
            const string sql = @"SELECT PuestoId, Nombre, Activo FROM Puesto ORDER BY Nombre;";
            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                lista.Add(new Puesto
                {
                    PuestoId = rd.GetInt32("PuestoId"),
                    Nombre = rd.GetString("Nombre"),
                    Activo = rd.GetBoolean("Activo")
                });
            }
            return lista;
        }

        //  Solo activos
        public async Task<IReadOnlyList<Puesto>> ListarActivosAsync()
        {
            var lista = new List<Puesto>();
            const string sql = @"SELECT PuestoId, Nombre, Activo FROM Puesto WHERE Activo = 1 ORDER BY Nombre;";
            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                lista.Add(new Puesto
                {
                    PuestoId = rd.GetInt32("PuestoId"),
                    Nombre = rd.GetString("Nombre"),
                    Activo = rd.GetBoolean("Activo")
                });
            }
            return lista;
        }

        public async Task<int> CrearAsync(Puesto p)
        {
            const string sql = @"INSERT INTO Puesto (Nombre, Activo)
                                 VALUES (@n, @a);
                                 SELECT LAST_INSERT_ID();";
            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@n", p.Nombre);
            cmd.Parameters.AddWithValue("@a", p.Activo);
            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        public async Task ActualizarAsync(Puesto p)
        {
            const string sql = @"UPDATE Puesto SET Nombre=@n, Activo=@a WHERE PuestoId=@id;";
            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@n", p.Nombre);
            cmd.Parameters.AddWithValue("@a", p.Activo);
            cmd.Parameters.AddWithValue("@id", p.PuestoId);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task AlternarEstadoAsync(int id, bool activo)
        {
            const string sql = @"UPDATE Puesto SET Activo=@a WHERE PuestoId=@id;";
            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@a", activo);
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarAsync(int id)
        {
            const string sql = @"DELETE FROM Puesto WHERE PuestoId=@id;";
            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Puesto> ObtenerPorIdAsync(int puestoId)
        {
            const string sql = @"SELECT PuestoId, Nombre, Activo
                                 FROM Puesto
                                 WHERE PuestoId = @id;";

            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@id", puestoId);

            using var rd = await cmd.ExecuteReaderAsync();
            if (await rd.ReadAsync())
            {
                return new Puesto
                {
                    PuestoId = rd.GetInt32("PuestoId"),
                    Nombre = rd.GetString("Nombre"),
                    Activo = rd.GetBoolean("Activo")
                };
            }
            return null;
        }
    }
}
