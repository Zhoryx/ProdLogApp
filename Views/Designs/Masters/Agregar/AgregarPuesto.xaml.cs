using System;
using System.Windows;
using ProdLogApp.Interfaces;     // IAgregarPuestoVista
using ProdLogApp.Models;         // Puesto
using ProdLogApp.Presenters;     // AgregarPuestoPresenter
using ProdLogApp.Servicios;      // IServicioPuestos

namespace ProdLogApp.Views
{
    public partial class AgregarPuesto : Window, IAgregarPuestoVista
    {
        private readonly AgregarPuestoPresenter _presenter;

        // Eventos que el presenter escucha (la vista solo los dispara)
        public event Action OnAceptar;
        public event Action OnCancelar;

        // ⚠️ Nombres/firmas actualizados: servicio y modelo nuevos
        public AgregarPuesto(IServicioPuestos servicioPuestos, Puesto puesto = null)
        {
            InitializeComponent();
            if (servicioPuestos == null) throw new ArgumentNullException(nameof(servicioPuestos));

            _presenter = new AgregarPuestoPresenter(this, servicioPuestos, puesto);
        }

        // ===== IAgregarPuestoVista (sin tocar el diseño) =====
        public string ObtenerNombre() => namepos.Text?.Trim() ?? string.Empty;

        // Tu XAML no tiene “activo”, devolvemos true por defecto (sin agregar controles)
        public bool ObtenerActivo() => true;

        // Precarga solo el nombre (ignoramos “activo” para no modificar UI)
        public void CargarDatosIniciales(string nombre, bool activo)
        {
            namepos.Text = nombre ?? string.Empty;
        }

        public void MostrarMensaje(string mensaje)
        {
            MessageBox.Show(mensaje, "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void Cerrar() => Close();

        // ===== Botones: solo disparan eventos, el presenter ya está suscripto =====
        private void Confirmar_Click(object sender, RoutedEventArgs e) => OnAceptar?.Invoke();
        private void Cancelar_Click(object sender, RoutedEventArgs e) => OnCancelar?.Invoke();
    }
}
