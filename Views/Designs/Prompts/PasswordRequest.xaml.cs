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
    /// Lógica de interacción para PasswordRequest.xaml
    /// </summary>
    public partial class PasswordRequest : Window
    {
        public PasswordRequest()
        {
            InitializeComponent();
        }


        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Text == "Escriba Constraseña Aqui...")
            {
                txtPassword.Text = "";
                txtPassword.Foreground = Brushes.Black;
            }
        }

        private void txtDNI_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Text = "Escriba Constraseña Aqui...";
                txtPassword.Foreground = Brushes.Gray;
            }
        }

        private void txtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void Confirmar(object sender, RoutedEventArgs e)
        {
            // Your code here
        }

        private void Cancelar(object sender, RoutedEventArgs e) { }
    }
}
