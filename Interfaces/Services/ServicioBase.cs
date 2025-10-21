using System.Threading.Tasks;
using MySqlConnector;

namespace ProdLogApp.Servicios
{
    // Clase base para servicios que acceden a la BD.
    // Centraliza el proveedor de conexión y expone un método protegido para abrir conexiones.
    public abstract class ServicioBase
    {
        protected readonly IProveedorConexion _conexion;

        // Inyectar el proveedor de conexión evita acoplarse a una implementación concreta (mejora testabilidad).
        protected ServicioBase(IProveedorConexion proveedorConexion)
        {
            _conexion = proveedorConexion ?? throw new System.ArgumentNullException(nameof(proveedorConexion));
        }

        // Devuelve una conexión ya abierta. El consumidor debe disponerla (await using).
        // Ej: await using var cn = await ObtenerConexionAsync();
        protected Task<MySqlConnection> ObtenerConexionAsync()
        {
            return _conexion.CrearAbiertaAsync();
        }
    }
}
