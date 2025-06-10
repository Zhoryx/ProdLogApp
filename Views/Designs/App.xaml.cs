using System;
using System.Windows;
using ProdLogApp.Services;
using ProdLogApp.Views;
using ProdLogApp.Models;
namespace ProdLogApp
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            IDatabaseService databaseService = new DatabaseService();
            App app = new App();
            app.Run(new Login(databaseService));
        }
    }
}


    //public class Program
    //{
    //    [STAThread]
    //    public static void Main()
    //    {
    //        App app = new App();

    //        // Simulación de usuario activo
    //        User activeUser = new User { Dni = "Admin", IsAdmin = true };

    //        // Iniciar la aplicación con el menú del gerente
    //        app.Run(new ManagerMenu(activeUser));
    //    }
    //}
//}
