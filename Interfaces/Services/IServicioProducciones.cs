// Servicios/IServicioProducciones.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProdLogApp.Models;

namespace ProdLogApp.Servicios
{
    public interface IServicioProducciones
    {
        // Parte (cabecera)
        Task<int> AsegurarParteAsync(int usuarioId, DateTime fecha);

        // CRUD Producciones
        Task<int> InsertarProduccionAsync(Produccion produccion, int parteId);
        Task ActualizarAsync(Produccion produccion);
        Task EliminarAsync(int produccionId);
        Task<Produccion> ObtenerPorIdAsync(int produccionId);

        // Listados
        Task<IList<Produccion>> ListarPorFechaAsync(int usuarioId, DateTime fecha);   // para Operario
        Task<IReadOnlyList<Produccion>> ListarPorRangoAsync(DateTime desde, DateTime hasta); // opcional

        // Paginado para Gerente (lista de cabeceras)
        Task<Page<ParteHeaderItem>> ObtenerCabecerasPaginadasAsync(
            DateTime desde,
            DateTime hasta,
            int page,
            int pageSize,
            string operarioLike = null);
    }
}
