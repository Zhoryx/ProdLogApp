using System.Collections.Generic;
using System.Threading.Tasks;
using ProdLogApp.Models; // Usa tus modelos existentes (p.ej., Categoria)

namespace ProdLogApp.Servicios
{
    // Servicio para Categorías: operaciones CRUD y validaciones de unicidad por nombre.
    public interface IServicioCategorias
    {
        Task<IReadOnlyList<Categoria>> ListarAsync();                 // Todos
        Task<Categoria> ObtenerPorIdAsync(int id);                     // Detalle
        Task<int> CrearAsync(Categoria categoria);                     // Alta
        Task ActualizarAsync(Categoria categoria);                     // Edición
        Task AlternarEstadoAsync(int id, bool activo);                 // Activar/Desactivar
        Task EliminarAsync(int id);                                    // Eliminación (si aplica)
        Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null); // Único por nombre (excluyendo el propio en edición)
    }
}
