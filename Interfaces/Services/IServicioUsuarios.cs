using System.Collections.Generic;
using System.Threading.Tasks;
using ProdLogApp.Models;

namespace ProdLogApp.Servicios
{
    public interface IServicioUsuarios
    {
        Task<IReadOnlyList<Usuario>> ListarAsync();
        Task<Usuario> ObtenerPorDniAsync(string dni);
        Task<Usuario> LoginAsync(string dni, string passwordPlano);
        Task<int> CrearAsync(Usuario usuario, string passwordPlano);
        Task ActualizarAsync(Usuario usuario);
        Task AlternarEstadoAsync(int id, bool activo);
        Task CambiarPasswordAsync(int id, string passwordPlano);
    }
}
