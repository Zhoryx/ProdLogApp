using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProdLogApp.Views
{
    /// <summary>
    /// Lógica de interacción para MenuProducciones.xaml
    /// </summary>
    public partial class GestionCategoria : Window
    {
        private void AgregarCategoria(object sender, RoutedEventArgs e)
        {
            CategoriaAgregar ventanaSecundaria = new CategoriaAgregar();
            ventanaSecundaria.Show();
            this.Close(); // Opcional: cierra la ventana actual
        }
        private void Volver(object sender, RoutedEventArgs e)
        {
            //MenuGerente ventanaSecundaria = new MenuGerente();
            //ventanaSecundaria.Show();
            //this.Close(); // Opcional: cierra la ventana actual
        }
    }
}
