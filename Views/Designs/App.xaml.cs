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
