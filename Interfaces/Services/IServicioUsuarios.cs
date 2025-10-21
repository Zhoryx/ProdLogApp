using System.Collections.Generic;
using System.Threading.Tasks;
using ProdLogApp.Models;

namespace ProdLogApp.Servicios
{
    // Contrato de servicio para operaciones sobre Usuarios.
    // Separa lectura/escritura y encapsula hashing/validaciones en un único lugar.
    public interface IServicioUsuarios
    {
        Task<IReadOnlyList<Usuario>> ListarAsync();                 // Listado general
        Task<Usuario> ObtenerPorDniAsync(string dni);               // Búsqueda por DNI
        Task<Usuario> LoginAsync(string dni, string passwordPlano); // Autenticación (hash + compare)
        Task<int> CrearAsync(Usuario usuario, string passwordPlano);// Alta (genera hash)
        Task ActualizarAsync(Usuario usuario);                      // Edición de datos
        Task AlternarEstadoAsync(int id, bool activo);              // Activar/Desactivar
        Task CambiarPasswordAsync(int id, string passwordPlano);    // Cambio de password (re-hash)
    }
}
