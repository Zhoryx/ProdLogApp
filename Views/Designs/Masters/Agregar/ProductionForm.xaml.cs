using System;
using System.Threading.Tasks;
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

        // Parte del día (viene del presenter en ALTA)
        private readonly int? _parteIdForToday;

        public Production ProduccionCreada { get; private set; }

        // Estado interno de selección (evita búsquedas por texto)
        private int? _productoIdSeleccionado;
        private int? _puestoIdSeleccionado;

        // Modo
        private readonly bool _isEdit = false;
        private readonly int? _editId = null;

        // ==========================
        // Constructores
        // ==========================

        // ALTA (original)
        public ProductionForm(User activeuser, IDatabaseService databaseService)
        {
            InitializeComponent();

            _activeUser = activeuser ?? throw new ArgumentNullException(nameof(activeuser));
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _presenter = new AddProductionUserPresenter(this, _databaseService, _activeUser.Id);

            _isEdit = false;
            _editId = null;
            _parteIdForToday = null;

            if (ProductoTextBox != null) ProductoTextBox.IsReadOnly = true;
            if (PuestoTextBox != null) PuestoTextBox.IsReadOnly = true;

            Loaded += async (_, __) => await PrefillForCreateAsync();
        }

        // EDICIÓN (con selector de prefill según isEdit)
        public ProductionForm(User activeuser, IDatabaseService databaseService, bool isEdit, int? editId = null)
        {
            InitializeComponent();

            _activeUser = activeuser ?? throw new ArgumentNullException(nameof(activeuser));
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _presenter = new AddProductionUserPresenter(this, _databaseService, _activeUser.Id);

            if (ProductoTextBox != null) ProductoTextBox.IsReadOnly = true;
            if (PuestoTextBox != null) PuestoTextBox.IsReadOnly = true;

            _isEdit = isEdit;
            _editId = editId;
            _parteIdForToday = null; // no se usa en edición

            // ⚠️ Antes forzaba PrefillForEditAsync siempre; ahora decide según isEdit
            Loaded += async (_, __) =>
            {
                if (_isEdit) await PrefillForEditAsync();
                else await PrefillForCreateAsync();
            };
        }

        // EDICIÓN/ALTA con parteId (para named arg 'parteId' en la llamada del presenter)
        public ProductionForm(User activeuser, IDatabaseService databaseService, bool isEdit, int? editId, int? parteId)
            : this(activeuser, databaseService, isEdit, editId)
        {
            _parteIdForToday = parteId; // Usado en ALTA para ligar al Parte actual
        }

        // ==========================
        // IAddProductionUserView
        // ==========================
        public string HoraInicio => HoraInicioTextBox.Text;
        public string HoraFin => HoraFinTextBox.Text;
        public string Cantidad => CantidadTextBox.Text;

        public void MostrarError(TextBox campo, string mensaje)
        {
            campo.Tag = "Error";
            campo.ToolTip = mensaje;
        }

        public void LimpiarError(TextBox campo)
        {
            campo.ClearValue(Control.TagProperty);
            campo.ToolTip = null;
        }

        public void ActualizarHora(TextBox campo, string horaNormalizada)
        {
            campo.Text = horaNormalizada;
        }

        public void MostrarMensaje(string mensaje)
        {
            MessageBox.Show(mensaje, "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void MostrarError(string mensaje)
        {
            MessageBox.Show(mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void CerrarDialogo(bool result, Production produccion = null)
        {
            ProduccionCreada = produccion;
            DialogResult = result;
            Close();
        }

        // ==========================
        // Prefill (Alta / Edición)
        // ==========================
        private Task PrefillForCreateAsync()
        {
            if (_isEdit) return Task.CompletedTask;

            try
            {
                if (string.IsNullOrWhiteSpace(HoraInicioTextBox.Text))
                    HoraInicioTextBox.Text = DateTime.Now.AddMinutes(-5).ToString("HH:mm");
                if (string.IsNullOrWhiteSpace(HoraFinTextBox.Text))
                    HoraFinTextBox.Text = DateTime.Now.ToString("HH:mm");
                if (string.IsNullOrWhiteSpace(CantidadTextBox.Text))
                    CantidadTextBox.Text = "1";
            }
            catch { /* no crítico */ }

            this.Title = "Nueva producción";
            return Task.CompletedTask;
        }

        private async Task PrefillForEditAsync()
        {
            if (!_isEdit || !_editId.HasValue) return;

            try
            {
                var p = await _databaseService.GetProductionByIdAsync(_editId.Value);
                if (p == null)
                {
                    MostrarError("No se encontró la producción seleccionada.");
                    DialogResult = false;
                    Close();
                    return;
                }

                _productoIdSeleccionado = p.ProductoId;
                _puestoIdSeleccionado = p.PuestoId;

                HoraInicioTextBox.Text = p.HInicio.ToString(@"hh\:mm");
                HoraFinTextBox.Text = p.HFin.ToString(@"hh\:mm");
                CantidadTextBox.Text = p.Cantidad.ToString();

                try
                {
                    var prod = await _databaseService.GetProductoByIdAsync(p.ProductoId);
                    ProductoTextBox.Text = prod?.Nombre ?? p.ProductoId.ToString();
                }
                catch { ProductoTextBox.Text = p.ProductoId.ToString(); }

                try
                {
                    var puesto = await _databaseService.GetPuestoByIdAsync(p.PuestoId);
                    PuestoTextBox.Text = puesto?.Nombre ?? p.PuestoId.ToString();
                }
                catch { PuestoTextBox.Text = p.PuestoId.ToString(); }

                this.Title = "Editar producción";
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar la producción: {ex.Message}");
                DialogResult = false;
                Close();
            }
        }

        // ==========================
        // Prompts / Selecciones
        // ==========================
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

        // ==========================
        // Validaciones en foco
        // ==========================
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

        // ==========================
        // Construcción del modelo
        // ==========================
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

                var model = new Production
                {
                    ProductoId = _productoIdSeleccionado.Value,
                    PuestoId = _puestoIdSeleccionado.Value,
                    HInicio = hInicio,
                    HFin = hFin,
                    Cantidad = cantidad
                };

                // 🔗 Clave: si es ALTA y tenemos parteId, inyectarlo acá
                if (!_isEdit && _parteIdForToday.HasValue)
                    model.ParteId = _parteIdForToday.Value;

                return model;
            }
            catch (Exception ex)
            {
                MostrarError($"Error al obtener datos del formulario: {ex.Message}");
                return null;
            }
        }

        // ==========================
        // Confirmar / Cancelar
        // ==========================
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
                if (_isEdit)
                {
                    var produccion = ObtenerDatosProduccion();
                    if (produccion == null) return;

                    produccion.ProductionId = _editId ?? 0;
                    await _databaseService.UpdateProduccionAsync(produccion);

                    ProduccionCreada = produccion;
                    DialogResult = true;
                    Close();
                }
                else
                {
                    // Mantener tu flujo con el Presenter (CargarProduccionAsync tomará los datos de la View)
                    // Como inyectamos ParteId en ObtenerDatosProduccion(), ya queda ligada al Parte actual
                    await _presenter.CargarProduccionAsync();
                    // el presenter te llamará a CerrarDialogo(true, produccion)
                }
            }
            catch (Exception ex)
            {
                MostrarError($"No se pudo guardar: {ex.Message}");
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
