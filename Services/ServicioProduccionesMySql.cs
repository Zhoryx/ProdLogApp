using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySqlConnector;
using ProdLogApp.Models;

namespace ProdLogApp.Servicios
{
    public sealed class ServicioProduccionesMySql : ServicioBase, IServicioProducciones
    {
        public ServicioProduccionesMySql(IProveedorConexion proveedorConexion) : base(proveedorConexion) { }

        // =====================================================
        // ASEGURAR PARTE (crear si no existe)
        // =====================================================
        public async Task<int> AsegurarParteAsync(int usuarioId, DateTime fecha)
        {
            const string sel = @"SELECT Parte_Id FROM Parte WHERE Usuario_Id=@uid AND Parte_Fecha=@fec;";
            const string ins = @"INSERT INTO Parte (Usuario_Id, Parte_Fecha) VALUES (@uid, @fec);
                                 SELECT LAST_INSERT_ID();";

            using var cn = await ObtenerConexionAsync();
            using var cmdSel = new MySqlCommand(sel, cn);
            cmdSel.Parameters.AddWithValue("@uid", usuarioId);
            cmdSel.Parameters.AddWithValue("@fec", fecha.Date);

            var existente = await cmdSel.ExecuteScalarAsync();
            if (existente != null && existente != DBNull.Value)
                return Convert.ToInt32(existente);

            using var cmdIns = new MySqlCommand(ins, cn);
            cmdIns.Parameters.AddWithValue("@uid", usuarioId);
            cmdIns.Parameters.AddWithValue("@fec", fecha.Date);
            var nuevoId = Convert.ToInt32(await cmdIns.ExecuteScalarAsync());
            return nuevoId;
        }

        // =====================================================
        // INSERTAR PRODUCCIÓN
        // =====================================================
        public async Task<int> InsertarProduccionAsync(Produccion produccion, int parteId)
        {
            if (produccion == null) throw new ArgumentNullException(nameof(produccion));

            const string sql = @"
            INSERT INTO Producciones
            (Produccion_HoraInicio, Produccion_HoraFin, Produccion_Cantidad, Producto_Id, Puesto_Id, Parte_Id)
            VALUES (@hIni, @hFin, @cant, @prodId, @puesId, @parteId);
            SELECT LAST_INSERT_ID();";

            using var cn = await ObtenerConexionAsync();
            using var tx = await cn.BeginTransactionAsync();

            try
            {
                using var cmd = new MySqlCommand(sql, cn, tx);
                cmd.Parameters.AddWithValue("@hIni", produccion.HoraInicio);
                cmd.Parameters.AddWithValue("@hFin", produccion.HoraFin);
                cmd.Parameters.AddWithValue("@cant", produccion.Cantidad);
                cmd.Parameters.AddWithValue("@prodId", produccion.ProductoId);
                cmd.Parameters.AddWithValue("@puesId", produccion.PuestoId);
                cmd.Parameters.AddWithValue("@parteId", parteId);

                var id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                await tx.CommitAsync();
                return id;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // =====================================================
        // LISTAR POR FECHA (para un usuario)
        // =====================================================
        public async Task<IList<Produccion>> ListarPorFechaAsync(int usuarioId, DateTime fecha)
        {
            var lista = new List<Produccion>();
            const string sql = @"
SELECT p.Produccion_Id,
       p.Produccion_HoraInicio,
       p.Produccion_HoraFin,
       p.Produccion_Cantidad,
       p.Producto_Id AS ProductoId,
       prod.ProductoNombre AS ProductoNombre,
       p.Puesto_Id   AS PuestoId,
       pu.Nombre     AS PuestoNombre,
       p.Parte_Id
FROM Producciones p
JOIN Parte pa     ON pa.Parte_Id   = p.Parte_Id
JOIN Producto prod ON prod.ProductoId = p.Producto_Id
JOIN Puesto   pu   ON pu.PuestoId   = p.Puesto_Id
WHERE pa.Usuario_Id = @uid
  AND pa.Parte_Fecha = @fec
ORDER BY p.Produccion_HoraInicio ASC;";

            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@uid", usuarioId);
            cmd.Parameters.AddWithValue("@fec", fecha.Date);

            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                lista.Add(new Produccion
                {
                    ProduccionId = rd.GetInt32("Produccion_Id"),
                    HoraInicio = rd.GetTimeSpan("Produccion_HoraInicio"),
                    HoraFin = rd.GetTimeSpan("Produccion_HoraFin"),
                    Cantidad = rd.GetInt32("Produccion_Cantidad"),
                    ProductoId = rd.GetInt32("ProductoId"),
                    ProductoNombre = rd.GetString("ProductoNombre"),
                    PuestoId = rd.GetInt32("PuestoId"),
                    PuestoNombre = rd.GetString("PuestoNombre"),
                    ParteId = rd.GetInt32("Parte_Id")
                });
            }
            return lista;
        }


        // =====================================================
        // LISTAR POR RANGO
        // =====================================================
        public async Task<IReadOnlyList<Produccion>> ListarPorRangoAsync(DateTime desde, DateTime hasta)
        {
            var lista = new List<Produccion>();
            const string sql = @"
SELECT p.Produccion_Id,
       p.Produccion_HoraInicio,
       p.Produccion_HoraFin,
       p.Produccion_Cantidad,
       p.Producto_Id AS ProductoId,
       p.Puesto_Id   AS PuestoId,
       p.Parte_Id,
       pa.Parte_Fecha
FROM Producciones p
JOIN Parte pa ON pa.Parte_Id = p.Parte_Id
WHERE pa.Parte_Fecha BETWEEN @d AND @h
ORDER BY pa.Parte_Fecha, p.Produccion_HoraInicio;";

            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@d", desde.Date);
            cmd.Parameters.AddWithValue("@h", hasta.Date);

            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                lista.Add(new Produccion
                {
                    ProduccionId = rd.GetInt32("Produccion_Id"),
                    HoraInicio = rd.GetTimeSpan("Produccion_HoraInicio"),
                    HoraFin = rd.GetTimeSpan("Produccion_HoraFin"),
                    Cantidad = rd.GetInt32("Produccion_Cantidad"),
                    ProductoId = rd.GetInt32("ProductoId"),
                    PuestoId = rd.GetInt32("PuestoId"),
                    ParteId = rd.GetInt32("Parte_Id")
                });
            }
            return lista;
        }

        // =====================================================
        // OBTENER POR ID
        // =====================================================
        public async Task<Produccion> ObtenerPorIdAsync(int produccionId)
        {
            const string sql = @"
SELECT Produccion_Id, Produccion_HoraInicio, Produccion_HoraFin, Produccion_Cantidad,
       Producto_Id AS ProductoId, Puesto_Id AS PuestoId, Parte_Id
FROM Producciones
WHERE Produccion_Id = @id;";

            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@id", produccionId);

            using var rd = await cmd.ExecuteReaderAsync();
            if (!await rd.ReadAsync()) return null;

            return new Produccion
            {
                ProduccionId = rd.GetInt32("Produccion_Id"),
                HoraInicio = rd.GetTimeSpan("Produccion_HoraInicio"),
                HoraFin = rd.GetTimeSpan("Produccion_HoraFin"),
                Cantidad = rd.GetInt32("Produccion_Cantidad"),
                ProductoId = rd.GetInt32("ProductoId"),
                PuestoId = rd.GetInt32("PuestoId"),
                ParteId = rd.GetInt32("Parte_Id")
            };
        }

        // =====================================================
        // ACTUALIZAR
        // =====================================================
        public async Task ActualizarAsync(Produccion produccion)
        {
            if (produccion == null) throw new ArgumentNullException(nameof(produccion));

            const string sql = @"
UPDATE Producciones
   SET Produccion_HoraInicio = @hIni,
       Produccion_HoraFin    = @hFin,
       Produccion_Cantidad   = @cant,
       Producto_Id           = @prodId,
       Puesto_Id             = @puesId
 WHERE Produccion_Id        = @id;";

            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@hIni", produccion.HoraInicio);
            cmd.Parameters.AddWithValue("@hFin", produccion.HoraFin);
            cmd.Parameters.AddWithValue("@cant", produccion.Cantidad);
            cmd.Parameters.AddWithValue("@prodId", produccion.ProductoId);
            cmd.Parameters.AddWithValue("@puesId", produccion.PuestoId);
            cmd.Parameters.AddWithValue("@id", produccion.ProduccionId);

            await cmd.ExecuteNonQueryAsync();
        }

        // =====================================================
        // ELIMINAR
        // =====================================================
        public async Task EliminarAsync(int produccionId)
        {
            const string sql = @"DELETE FROM Producciones WHERE Produccion_Id=@id;";
            using var cn = await ObtenerConexionAsync();
            using var cmd = new MySqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@id", produccionId);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Page<ParteHeaderItem>> ObtenerCabecerasPaginadasAsync(
        DateTime desde,
        DateTime hasta,
        int page,
        int pageSize,
        string operarioLike = null)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 50;

            int offset = (page - 1) * pageSize;

            // Ajustado a tu esquema: tabla Usuario con columnas UsId y UsNombre
            const string baseWhere = @"
            pa.Parte_Fecha BETWEEN @d AND @h
            /**OPLIKE**/
        ";

            string sqlCount = $@"
            SELECT COUNT(*) 
            FROM (
                SELECT pa.Parte_Fecha, u.UsId
                FROM Parte pa
                JOIN Producciones p ON p.Parte_Id = pa.Parte_Id
                JOIN Usuario u      ON u.UsId      = pa.Usuario_Id
                WHERE {baseWhere}
                GROUP BY pa.Parte_Fecha, u.UsId
            ) X;
        ";

            string sqlPage = $@"
            SELECT 
                pa.Parte_Fecha                                  AS FechaParte,
                u.UsId                                          AS UserId,
                u.UsNombre                                      AS Operario,
                COUNT(p.Produccion_Id)                           AS CantProducciones,
                COALESCE(SUM(p.Produccion_Cantidad), 0)          AS TotalCantidad
            FROM Parte pa
            JOIN Producciones p ON p.Parte_Id = pa.Parte_Id
            JOIN Usuario u      ON u.UsId      = pa.Usuario_Id
            WHERE {baseWhere}
            GROUP BY pa.Parte_Fecha, u.UsId, u.UsNombre
            ORDER BY pa.Parte_Fecha DESC, u.UsNombre ASC
            LIMIT @limit OFFSET @offset;
        ";

            string filtroLike = string.IsNullOrWhiteSpace(operarioLike) ? "" : "AND u.UsNombre LIKE @like";
            sqlCount = sqlCount.Replace("/**OPLIKE**/", filtroLike);
            sqlPage = sqlPage.Replace("/**OPLIKE**/", filtroLike);

            var items = new List<ParteHeaderItem>();
            int total = 0;

            using var cn = await ObtenerConexionAsync();

            using (var cmdCount = new MySqlCommand(sqlCount, cn))
            {
                cmdCount.Parameters.AddWithValue("@d", desde.Date);
                cmdCount.Parameters.AddWithValue("@h", hasta.Date);
                if (!string.IsNullOrWhiteSpace(operarioLike))
                    cmdCount.Parameters.AddWithValue("@like", $"%{operarioLike.Trim()}%");

                total = Convert.ToInt32(await cmdCount.ExecuteScalarAsync());
            }

            using (var cmdPage = new MySqlCommand(sqlPage, cn))
            {
                cmdPage.Parameters.AddWithValue("@d", desde.Date);
                cmdPage.Parameters.AddWithValue("@h", hasta.Date);
                if (!string.IsNullOrWhiteSpace(operarioLike))
                    cmdPage.Parameters.AddWithValue("@like", $"%{operarioLike.Trim()}%");
                cmdPage.Parameters.AddWithValue("@limit", pageSize);
                cmdPage.Parameters.AddWithValue("@offset", offset);

                using var rd = await cmdPage.ExecuteReaderAsync();
                while (await rd.ReadAsync())
                {
                    items.Add(new ParteHeaderItem
                    {
                        FechaParte = rd.GetDateTime("FechaParte"),
                        UserId = rd.GetInt32("UserId"),
                        Operario = rd.GetString("Operario"),
                        CantProducciones = rd.GetInt32("CantProducciones"),
                        TotalCantidad = rd.GetInt32("TotalCantidad")
                    });
                }
            }

            return new Page<ParteHeaderItem>
            {
                PageNumber = page,
                PageSize = pageSize,
                Total = total,
                Items = items
            };
        }
    }
}
