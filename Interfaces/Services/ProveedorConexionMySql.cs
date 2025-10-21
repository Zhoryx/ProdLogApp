using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using MySqlConnector;

namespace ProdLogApp.Servicios
{
    // Proveedor de conexión para MySQL con múltiples fuentes posibles:
    // 1) App.config (ConnectionStrings)
    // 2) Variable de entorno "PRODLOG_CS"
    // 3) Archivo connection.txt junto al .exe
    // 4) Archivo connection.txt en AppData\Roaming\ProdLogApp
    // 5) Fallback de desarrollo (último recurso, no recomendado para producción)
    public sealed class ProveedorConexionMySql : IProveedorConexion
    {
        private readonly string _cs;

        public ProveedorConexionMySql(string nombreCadena = "ProdLogDb")
        {
            // 1️⃣ App.config / Web.config
            try
            {
                var cs = ConfigurationManager.ConnectionStrings[nombreCadena]?.ConnectionString;
                if (!string.IsNullOrWhiteSpace(cs))
                {
                    _cs = cs;
                    return;
                }
            }
            catch { } // Si ConfigurationManager no está disponible o falla, seguimos con las otras fuentes.

            // 2️⃣ Variable de entorno
            var env = Environment.GetEnvironmentVariable("PRODLOG_CS");
            if (!string.IsNullOrWhiteSpace(env))
            {
                _cs = env;
                return;
            }

            // 3️⃣ connection.txt junto al .exe
            var exeDir = AppDomain.CurrentDomain.BaseDirectory;
            var pathLocal = Path.Combine(exeDir, "connection.txt");
            if (File.Exists(pathLocal))
            {
                var raw = File.ReadAllText(pathLocal).Trim();
                if (!string.IsNullOrWhiteSpace(raw))
                {
                    _cs = raw;
                    return;
                }
            }

            // 4️⃣ connection.txt en AppData\Roaming\ProdLogApp\connection.txt
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var pathRoaming = Path.Combine(appData, "ProdLogApp", "connection.txt");
            if (File.Exists(pathRoaming))
            {
                var raw = File.ReadAllText(pathRoaming).Trim();
                if (!string.IsNullOrWhiteSpace(raw))
                {
                    _cs = raw;
                    return;
                }
            }

            // 5️⃣ Fallback (solo desarrollo): evita bloquear el arranque si nada está configurado.
            // ⚠️ Contiene credenciales en código: no usar en producción.
            const string fallbackDev = "Server=127.0.0.1;Port=3307;Database=ProdLog_BD;User Id=root;Password=Madersa;";
            _cs = fallbackDev;
        }

        // Expone la cadena efectiva (útil para diagnósticos). 
        public string ObtenerConnectionString()
        {
            if (string.IsNullOrWhiteSpace(_cs))
                throw new InvalidOperationException("No se pudo resolver la cadena de conexión.");
            return _cs;
        }

        // Devuelve una conexión MySQL ya abierta lista para usar.
        // Uso recomendado: await using var cn = await CrearAbiertaAsync();
        public async Task<MySqlConnection> CrearAbiertaAsync()
        {
            var cn = new MySqlConnection(ObtenerConnectionString());
            await cn.OpenAsync();
            return cn;
        }

        // Permite verificar rápidamente conectividad/credenciales.
        public async Task<bool> ProbarConexionAsync()
        {
            try
            {
                using var cn = await CrearAbiertaAsync();
                return cn.State == System.Data.ConnectionState.Open;
            }
            catch
            {
                return false;
            }
        }
    }
}
