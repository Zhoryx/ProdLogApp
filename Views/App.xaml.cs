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
            MenuProducciones view = new MenuProducciones();
            view.Show();
            new Application().Run();
        }
    }
}