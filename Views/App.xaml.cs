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
            MenuProduccionGerente view = new MenuProduccionGerente();
            view.Show();
            new Application().Run();
        }
    }
}