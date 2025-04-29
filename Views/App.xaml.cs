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
            ProduccionAgregarGerente view = new ProduccionAgregarGerente();
            view.Show();
            new Application().Run();
        }
    }
}