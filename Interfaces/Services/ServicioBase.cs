using System.Threading.Tasks;
using MySqlConnector;

namespace ProdLogApp.Servicios
{
    public abstract class ServicioBase
    {
        protected readonly IProveedorConexion _conexion;

        protected ServicioBase(IProveedorConexion proveedorConexion)
        {
            _conexion = proveedorConexion ?? throw new System.ArgumentNullException(nameof(proveedorConexion));
        }

        protected Task<MySqlConnection> ObtenerConexionAsync()
        {
            return _conexion.CrearAbiertaAsync();
        }
    }
}
