// Servicios/IServicioProducciones.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProdLogApp.Models;

namespace ProdLogApp.Servicios
{
    // Servicio para Partes/Producciones: asegura cabeceras y gestiona el detalle.
    // Permite a Operario trabajar por fecha y a Gerencia consultar por rango o páginas.
    public interface IServicioProducciones
    {
        // Parte (cabecera): crea o recupera la Parte del usuario en la fecha indicada.
        // Requiere UNIQUE (Usuario_Id, Parte_Fecha) en la tabla Parte para evitar duplicados.
        Task<int> AsegurarParteAsync(int usuarioId, DateTime fecha);

        // CRUD del detalle de producción
        Task<int> InsertarProduccionAsync(Produccion produccion, int parteId);
        Task ActualizarAsync(Produccion produccion);
        Task EliminarAsync(int produccionId);
        Task<Produccion> ObtenerPorIdAsync(int produccionId);

        // Listados
        Task<IList<Produccion>> ListarPorFechaAsync(int usuarioId, DateTime fecha); // Vista Operario (día)
        Task<IReadOnlyList<Produccion>> ListarPorRangoAsync(DateTime desde, DateTime hasta); // Vista Gerente (opcional)

        // Paginado para Gerente (cabeceras de partes) con filtro por operario (like)
        Task<Page<ParteHeaderItem>> ObtenerCabecerasPaginadasAsync(
            DateTime desde,
            DateTime hasta,
            int page,
            int pageSize,
            string operarioLike = null);
    }
}
