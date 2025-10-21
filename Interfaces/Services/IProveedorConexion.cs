using System.Threading.Tasks;
using MySqlConnector;

namespace ProdLogApp.Servicios
{
   
    public interface IProveedorConexion
    {
        
        string ObtenerConnectionString();

        
        Task<MySqlConnection> CrearAbiertaAsync();

       
        Task<bool> ProbarConexionAsync();
    }
}
