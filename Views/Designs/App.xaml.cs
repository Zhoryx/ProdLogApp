using System;
using System.Windows;

namespace ProdLogApp
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            App app = new App();
            app.Run(new Views.Login());
        }
    }
}


//sin BD
//using System;
//using System.Windows;
//using ProdLogApp.Models;
//using ProdLogApp.Views;

//namespace ProdLogApp
//{
//    public class Program
//    {
//        [STAThread]
//        public static void Main()
//        {
//            App app = new App();

//            // Simulaci�n de usuario activo
//            User activeUser = new User { Dni = "Admin", IsAdmin = true };

//            // Iniciar la aplicaci�n con el men� del gerente
//            app.Run(new ManagerMenu(activeUser));
//        }
//    }
//}