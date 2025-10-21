using System.Collections.Generic;
using System.Threading.Tasks;
using ProdLogApp.Models;

namespace ProdLogApp.Servicios
{
    public interface IServicioPuestos
    {
        Task<IReadOnlyList<Puesto>> ListarAsync();
        Task<IReadOnlyList<Puesto>> ListarActivosAsync();
        Task<int> CrearAsync(Puesto puesto);
        Task ActualizarAsync(Puesto puesto);
        Task AlternarEstadoAsync(int id, bool activo);
        Task EliminarAsync(int id);

        Task<Puesto> ObtenerPorIdAsync(int puestoId);
    }
}
