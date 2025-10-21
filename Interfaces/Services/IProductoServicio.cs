using System.Collections.Generic;
using System.Threading.Tasks;
using ProdLogApp.Models;

namespace ProdLogApp.Servicios
{
    public interface IServicioProductos
    {
        Task<IReadOnlyList<Producto>> ListarAsync();
        Task<IReadOnlyList<Producto>> ListarActivosAsync();
        Task<Producto> ObtenerPorIdAsync(int id);
        Task<int> CrearAsync(Producto producto);
        Task ActualizarAsync(Producto producto);
        Task AlternarEstadoAsync(int id, bool activo);
        Task EliminarAsync(int id);
    }
}
