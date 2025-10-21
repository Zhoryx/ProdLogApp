using System.Collections.Generic;
using System.Threading.Tasks;
using ProdLogApp.Models;

namespace ProdLogApp.Servicios
{
    // Contrato de servicio para Puestos (ABM + listados).
    // Útil para prompts y pantallas de administración.
    public interface IServicioPuestos
    {
        Task<IReadOnlyList<Puesto>> ListarAsync();           // Todos
        Task<IReadOnlyList<Puesto>> ListarActivosAsync();    // Solo activos (para selección)
        Task<int> CrearAsync(Puesto puesto);                 // Alta
        Task ActualizarAsync(Puesto puesto);                 // Edición
        Task AlternarEstadoAsync(int id, bool activo);       // Activar/Desactivar
        Task EliminarAsync(int id);                          // Eliminación (si corresponde política)

        Task<Puesto> ObtenerPorIdAsync(int puestoId);        // Detalle por Id
    }
}
