using System;
using System.Windows;
using System.Windows.Controls;
using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Services;
using ProdLogApp.Views.Designs.Prompts;
using ProdLogApp.Presenters;

namespace ProdLogApp.Views
{
    public partial class ProductionForm : Window, IAddProductionUserView
    {
        private readonly AddProductionUserPresenter _presenter;
        private readonly User _activeUser;
        private readonly IDatabaseService _databaseService;
        public Production ProduccionCreada { get; private set; }

        // Estado interno de selección (evita búsquedas por texto)
        private int? _productoIdSeleccionado;
        private int? _puestoIdSeleccionado;

        public ProductionForm(User activeuser, IDatabaseService databaseService)
        {
            InitializeComponent();

            _activeUser = activeuser ?? throw new ArgumentNullException(nameof(activeuser));
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _presenter = new AddProductionUserPresenter(this, _databaseService, _activeUser.Id);

            // Evitar desincronización entre texto e ID
            if (ProductoTextBox != null) ProductoTextBox.IsReadOnly = true;
            if (PuestoTextBox != null) PuestoTextBox.IsReadOnly = true;
        }

        // Propiedades requeridas por la interfaz
        public string HoraInicio => HoraInicioTextBox.Text;
        public string HoraFin => HoraFinTextBox.Text;
        public string Cantidad => CantidadTextBox.Text;

        // Feedback visual de validaciones
        public void MostrarError(TextBox campo, string mensaje)
        {
            campo.Tag = "Error"; // activa el trigger visual en XAML
            campo.ToolTip = mensaje;
        }

        public void LimpiarError(TextBox campo)
        {
            campo.ClearValue(Control.TagProperty); // desactiva el trigger
            campo.ToolTip = null;
        }

        public void ActualizarHora(TextBox campo, string horaNormalizada)
        {
            campo.Text = horaNormalizada;
        }

        // Eventos de validación de campos
        private void HoraInicioTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
                _presenter.ValidarHora(tb, tb.Text);
        }

        private void HoraFinTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
                _presenter.ValidarHora(tb, tb.Text);
        }

        private void CantidadTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
                _presenter.ValidarCantidad(tb, tb.Text);
        }

        // Helper genérico para prompts
        private bool TryPrompt<T>(Func<bool?> showDialog, Func<T> getSelection, Action<T> applySelection)
        {
            bool? result = showDialog();
            if (result == true)
            {
                var selection = getSelection();
                applySelection(selection);
                return true;
            }
            return false;
        }

        // Selección de Producto
        private void SeleccionarProducto_Click(object sender, RoutedEventArgs e)
        {
            var prompt = new PromptProduct(_activeUser, _databaseService);

            TryPrompt(
                showDialog: () => prompt.ShowDialog(),
                getSelection: () => prompt.ObtenerProductoSeleccionado(),
                applySelection: producto =>
                {
                    _productoIdSeleccionado = producto.Id;
                    ProductoTextBox.Text = producto.Nombre;
                }
            );
        }

        // Selección de Puesto
        private void SeleccionarPuesto_Click(object sender, RoutedEventArgs e)
        {
            var prompt = new PromptPosition(_activeUser, _databaseService);

            TryPrompt(
                showDialog: () => prompt.ShowDialog(),
                getSelection: () =>
                {
                    prompt.ObtenerPuestoSeleccionado(out int puestoId, out string descripcion);
                    return (puestoId, descripcion);
                },
                applySelection: sel =>
                {
                    _puestoIdSeleccionado = sel.puestoId;
                    PuestoTextBox.Text = sel.descripcion;
                }
            );
        }

        // Construcción segura del modelo con IDs seleccionados
        public Production ObtenerDatosProduccion()
        {
            try
            {
                if (!_productoIdSeleccionado.HasValue)
                    throw new InvalidOperationException("Debe seleccionar un producto.");
                if (!_puestoIdSeleccionado.HasValue)
                    throw new InvalidOperationException("Debe seleccionar un puesto.");

                if (!TimeSpan.TryParse(HoraInicioTextBox.Text, out var hInicio))
                    throw new FormatException("Hora de inicio inválida.");
                if (!TimeSpan.TryParse(HoraFinTextBox.Text, out var hFin))
                    throw new FormatException("Hora de fin inválida.");
                if (!int.TryParse(CantidadTextBox.Text, out var cantidad))
                    throw new FormatException("Cantidad inválida.");

                return new Production
                {
                    ProductoId = _productoIdSeleccionado.Value,
                    PuestoId = _puestoIdSeleccionado.Value,
                    HInicio = hInicio,
                    HFin = hFin,
                    Cantidad = cantidad
                };
            }
            catch (Exception ex)
            {
                MostrarError($"Error al obtener datos del formulario: {ex.Message}");
                return null;
            }
        }

        public void MostrarMensaje(string mensaje)
        {
            MessageBox.Show(mensaje, "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void MostrarError(string mensaje)
        {
            MessageBox.Show(mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // Nuevo: cierre con resultado (lo invoca el Presenter)
        public void CerrarDialogo(bool result, Production produccion = null)
        {
            ProduccionCreada = produccion;
            DialogResult = result;
            Close();
        }

        // Confirmar y cancelar
        private async void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            if (!_productoIdSeleccionado.HasValue)
            {
                MostrarError("Seleccione un producto antes de confirmar.");
                return;
            }
            if (!_puestoIdSeleccionado.HasValue)
            {
                MostrarError("Seleccione un puesto antes de confirmar.");
                return;
            }

            btnConfirmar.IsEnabled = false;
            try
            {
                await _presenter.CargarProduccionAsync();
            }
            finally
            {
                btnConfirmar.IsEnabled = true;
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PuestoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        public void LimpiarFormulario()
        {
            HoraInicioTextBox.Text = "";
            HoraFinTextBox.Text = "";
            CantidadTextBox.Text = "";
            ProductoTextBox.Text = "";
            PuestoTextBox.Text = "";

            _productoIdSeleccionado = null;
            _puestoIdSeleccionado = null;

            LimpiarError(HoraInicioTextBox);
            LimpiarError(HoraFinTextBox);
            LimpiarError(CantidadTextBox);
        }
    }
}
