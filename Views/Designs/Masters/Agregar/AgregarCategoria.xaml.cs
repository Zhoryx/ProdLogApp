using System;
using System.Windows;
using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Servicios;   // <- era Services, ahora usamos la capa modularizada

namespace ProdLogApp.Views
{
    public partial class AgregarCategoria : Window, IAgregarCategoriaVista
    {
        // Servicio específico (no IDatabaseService genérico)
        private readonly IServicioCategorias _servicioCategorias;
        private readonly AgregarCategoriaPresenter _presenter;

        // Si venís a editar una categoría existente, pasala en el ctor
        public AgregarCategoria(Categoria categoria = null)
        {
            InitializeComponent();

            // Composition root simple (podés mover esto a App si querés)
            var proveedor = new ProveedorConexionMySql("ProdLogDb");
            _servicioCategorias = new ServicioCategoriasMySql(proveedor);

            _presenter = new AgregarCategoriaPresenter(this, _servicioCategorias, categoria);
        }

        // ==== IAgregarCategoriaVista (eventos) ====
        public event Action OnAceptar;
        public event Action OnCancelar;

        // ==== IAgregarCategoriaVista (lectura/escritura UI) ====
        public string ObtenerNombre() => NombreCategoriaTextBox.Text?.Trim() ?? string.Empty;
        public bool ObtenerActivo() => ActivoCheckBox.IsChecked == true;

        public void CargarDatosIniciales(string nombre, bool activo)
        {
            NombreCategoriaTextBox.Text = nombre ?? string.Empty;
            ActivoCheckBox.IsChecked = activo;
        }

        public void MostrarMensaje(string mensaje) =>
            MessageBox.Show(mensaje, "Información", MessageBoxButton.OK, MessageBoxImage.Information);

        public void Cerrar() => Close();

        // ==== Handlers de botones ====
        private void Confirmar_Click(object sender, RoutedEventArgs e) => OnAceptar?.Invoke();
        private void Cancelar_Click(object sender, RoutedEventArgs e) => OnCancelar?.Invoke();
    }
}
