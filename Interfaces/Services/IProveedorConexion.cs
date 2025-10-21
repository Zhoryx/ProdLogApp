using System.Threading.Tasks;
using MySqlConnector;

namespace ProdLogApp.Servicios
{
    // Abstracción mínima para obtener conexiones a MySQL.
    // Permite intercambiar origen (config/env/archivo) sin tocar servicios llamantes.
    public interface IProveedorConexion
    {
        // Devuelve la cadena efectiva (evitar loguearla con credenciales).
        string ObtenerConnectionString();

        // Crea y abre una conexión (uso con await using para disponerla).
        Task<MySqlConnection> CrearAbiertaAsync();

        // Prueba que la cadena/host/credenciales sean válidas (diagnóstico simple).
        Task<bool> ProbarConexionAsync();
    }
}
