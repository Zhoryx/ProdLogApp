using System;
using System.Windows;
using System.Windows.Controls;
using ProdLogApp.Views;

namespace ProdLogApp.Views
{
    public partial class Login : Window
    {
        private readonly LoginPresenter _presenter;

        public string Dni => DniTextBox.Text;

        public Login()
        {
            InitializeComponent();
            _presenter = new LoginPresenter(this);
        }

        public void MostrarMensaje(string mensaje)
        {
            MessageBox.Show(mensaje);
        }

        public void Cerrar()
        {
            this.Close();
        }

        private void IngresarButton_Click(object sender, RoutedEventArgs e)
        {
            _presenter.ValidarIngreso();
        }
    }
}