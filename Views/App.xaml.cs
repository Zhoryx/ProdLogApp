using ProdLogApp.Views;
using System;
using System.Windows;

namespace ProdLogApp
{
    public class ProdLogApp : Application
    {
        [STAThread]
        public static void Main()
        {
            ProduccionAgregar view = new ProduccionAgregar();
            view.Show();
            new Application().Run();
        }
    }
}