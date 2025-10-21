using System.Collections.Generic;
using System.Threading.Tasks;
using ProdLogApp.Models; // Usa tus modelos existentes (p.ej., Categoria)

namespace ProdLogApp.Servicios
{
    public interface IServicioCategorias
    {
        Task<IReadOnlyList<Categoria>> ListarAsync();
        Task<Categoria> ObtenerPorIdAsync(int id);
        Task<int> CrearAsync(Categoria categoria);
        Task ActualizarAsync(Categoria categoria);
        Task AlternarEstadoAsync(int id, bool activo);
        Task EliminarAsync(int id);
        Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null);
    }
}
