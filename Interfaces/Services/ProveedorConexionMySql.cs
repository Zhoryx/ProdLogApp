using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using MySqlConnector;

namespace ProdLogApp.Servicios
{
   

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
            catch { }

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

            // 4️⃣ connection.txt en AppData
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

            // 5️⃣ fallback (solo desarrollo)
            const string fallbackDev = "Server=127.0.0.1;Port=3307;Database=ProdLog_BD;User Id=root;Password=Madersa;";
            _cs = fallbackDev;
        }

        public string ObtenerConnectionString()
        {
            if (string.IsNullOrWhiteSpace(_cs))
                throw new InvalidOperationException("No se pudo resolver la cadena de conexión.");
            return _cs;
        }

        /// <summary>
        /// Devuelve una conexión MySQL ya abierta lista para usar.
        /// </summary>
        public async Task<MySqlConnection> CrearAbiertaAsync()
        {
            var cn = new MySqlConnection(ObtenerConnectionString());
            await cn.OpenAsync();
            return cn;
        }

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
