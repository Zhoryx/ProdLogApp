using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Services;
using System.Windows;

namespace ProdLogApp.Views
{
    public partial class PositionAdd : Window, IPositionAddView
    {
        private readonly AddPositionPresenter _presenter;

        public PositionAdd(IDatabaseService databaseService, Position puesto = null)
        {
            InitializeComponent();
            _presenter = new AddPositionPresenter(this, databaseService, puesto);
        }

        public string NombreIngresado => namepos.Text;

        public void CargarDatos(Position puesto)
        {
            namepos.Text = puesto.Nombre;
        }

        public void MostrarMensaje(string mensaje)
        {
            MessageBox.Show(mensaje, "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void CerrarConExito()
        {
            DialogResult = true;
            Close();
        }

        public void CerrarSinGuardar()
        {
            DialogResult = false;
            Close();
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e) => _presenter.Confirmar();
        private void Cancelar_Click(object sender, RoutedEventArgs e) => _presenter.Cancelar();
    }
}
