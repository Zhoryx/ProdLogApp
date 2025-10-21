using System;
using System.Windows;
using ProdLogApp.Servicios;

using ProdLogApp.Views;

namespace ProdLogApp
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            var app = new App();

            // Inyecci�n simple para Login (ajusta seg�n tu Login)
            var proveedor = new ProveedorConexionMySql("ProdLogDb");
            var svcUsuarios = new ServicioUsuariosMySql(proveedor);

            var login = new Login(); // o el ctor que tenga tu Login
            app.Run(login);
        }
    }
}
