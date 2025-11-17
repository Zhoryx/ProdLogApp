using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Presenters;
using ProdLogApp.Servicios;   // ⬅️ usar servicios modulares
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ProdLogApp.Views.Designs.Prompts; // PromptProducto, PromptPuesto


namespace ProdLogApp.Views
{
    public partial class FormularioProduccion : Window, IFormularioProduccionVista
    {
        private readonly AgregarProduccionPresenter _presenter;
        private readonly Usuario _activeUser;
        private readonly IServicioProducciones _svcProducciones;   
        private readonly int? _parteIdForToday;

        public Produccion ProduccionCreada { get; private set; }

        private int? _productoIdSeleccionado;
        private int? _puestoIdSeleccionado;
        private string _productoNombreSeleccionado;
        private string _puestoNombreSeleccionado;

        private readonly bool _isEdit = false;
        private readonly int? _editId = null;

        
        private System.Threading.Tasks.Task PrefillForCreateAsync()
        {
            if (_isEdit) return System.Threading.Tasks.Task.CompletedTask;

            try
            {
                if (string.IsNullOrWhiteSpace(HoraInicioTextBox.Text))
                    HoraInicioTextBox.Text = DateTime.Now.AddMinutes(-5).ToString("HH:mm");

                if (string.IsNullOrWhiteSpace(HoraFinTextBox.Text))
                    HoraFinTextBox.Text = DateTime.Now.ToString("HH:mm");

                if (string.IsNullOrWhiteSpace(CantidadTextBox.Text))
                    CantidadTextBox.Text = "1";
            }
            catch
            {
                // no crítico
            }

            this.Title = "Nueva producción";
            return System.Threading.Tasks.Task.CompletedTask;
        }

        private async Task PrefillForEditAsync()
        {
            if (!_isEdit || !_editId.HasValue) return;

            try
            {
                var p = await _svcProducciones.ObtenerPorIdAsync(_editId.Value);
                if (p == null)
                {
                    MostrarError("No se encontró la producción seleccionada.");
                    DialogResult = false;
                    Close();
                    return;
                }

                // IDs seleccionados para que Confirmar funcione sin re-elegir
                _productoIdSeleccionado = p.ProductoId;
                _puestoIdSeleccionado = p.PuestoId;

                // Traer nombres para mostrar en los textbox de solo lectura
                _productoNombreSeleccionado = await ObtenerNombreProductoAsync(p.ProductoId);
                _puestoNombreSeleccionado = await ObtenerNombrePuestoAsync(p.PuestoId);

                // Mostrar nombres (NO pisar con IDs)
                if (ProductoTextBox != null) ProductoTextBox.Text = _productoNombreSeleccionado;
                if (PuestoTextBox != null) PuestoTextBox.Text = _puestoNombreSeleccionado;

                // Resto de campos
                if (HoraInicioTextBox != null) HoraInicioTextBox.Text = p.HoraInicio.ToString(@"hh\:mm");
                if (HoraFinTextBox != null) HoraFinTextBox.Text = p.HoraFin.ToString(@"hh\:mm");
                if (CantidadTextBox != null) CantidadTextBox.Text = p.Cantidad.ToString();

                Title = "Editar producción";
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar la producción: {ex.Message}");
                DialogResult = false;
                Close();
            }
        }

        // === ALTA ===
        public FormularioProduccion(Usuario activeuser, IServicioProducciones svcProducciones)
        {
            InitializeComponent();

            _activeUser = activeuser ?? throw new ArgumentNullException(nameof(activeuser));
            _svcProducciones = svcProducciones ?? throw new ArgumentNullException(nameof(svcProducciones));
            _presenter = new AgregarProduccionPresenter(this, _svcProducciones, _activeUser.Id); // ⬅️ tu presenter también debe migrar

            _isEdit = false;
            _editId = null;
            _parteIdForToday = null;

            if (ProductoTextBox != null) ProductoTextBox.IsReadOnly = true;
            if (PuestoTextBox != null) PuestoTextBox.IsReadOnly = true;

            Loaded += async (_, __) => await PrefillForCreateAsync();
        }

        // === EDICIÓN ===
        public FormularioProduccion(Usuario activeuser, IServicioProducciones svcProducciones, bool isEdit, int? editId = null)
        {
            InitializeComponent();

            _activeUser = activeuser ?? throw new ArgumentNullException(nameof(activeuser));
            _svcProducciones = svcProducciones ?? throw new ArgumentNullException(nameof(svcProducciones));
            _presenter = new AgregarProduccionPresenter(this, _svcProducciones, _activeUser.Id);

            if (ProductoTextBox != null) ProductoTextBox.IsReadOnly = true;
            if (PuestoTextBox != null) PuestoTextBox.IsReadOnly = true;

            _isEdit = isEdit;
            _editId = editId;
            _parteIdForToday = null;

            Loaded += async (_, __) =>
            {
                if (_isEdit) await PrefillForEditAsync();
                else await PrefillForCreateAsync();
            };
        }

        // === EDICIÓN/ALTA con parteId ===
        public FormularioProduccion(Usuario activeuser, IServicioProducciones svcProducciones, bool isEdit, int? editId, int? parteId)
            : this(activeuser, svcProducciones, isEdit, editId)
        {
            _parteIdForToday = parteId;
        }



        // ===== Confirmar / Guardar =====
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

            // ⏱ rango horario
            if (!ValidarRangoHoras())
            {
                MostrarMensaje("Corregí el rango horario antes de confirmar.");
                return;
            }

            btnConfirmar.IsEnabled = false;
            try
            {
                var produccion = ObtenerDatosProduccion();
                if (produccion == null)
                {
                    // Ya se mostró el error adentro; no cierres el form
                    return;
                }

                if (_isEdit)
                {
                    produccion.ProduccionId = _editId ?? 0;
                    await _svcProducciones.ActualizarAsync(produccion);
                }

                ProduccionCreada = produccion;
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MostrarError($"No se pudo procesar: {ex.Message}");
            }
            finally
            {
                btnConfirmar.IsEnabled = true;
            }
        }


        private async Task<string> ObtenerNombreProductoAsync(int productoId)
        {
            try
            {
                var prov = new ProveedorConexionMySql("ProdLogDb");
                var svc = new ServicioProductosMySql(prov);
                var p = await svc.ObtenerPorIdAsync(productoId);
                return p?.Nombre ?? productoId.ToString();
            }
            catch
            {
                return productoId.ToString();
            }
        }

        private async Task<string> ObtenerNombrePuestoAsync(int puestoId)
        {
            try
            {
                var prov = new ProveedorConexionMySql("ProdLogDb");
                var svc = new ServicioPuestosMySql(prov);
                var pu = await svc.ObtenerPorIdAsync(puestoId);
                return pu?.Nombre ?? puestoId.ToString();
            }
            catch
            {
                return puestoId.ToString();
            }
        }



        // ======= Implementación IFormularioProduccionVista que faltaba =======

        public string HoraInicio => HoraInicioTextBox.Text;
        public string HoraFin => HoraFinTextBox.Text;
        public string Cantidad => CantidadTextBox.Text;

        public void MostrarError(TextBox campo, string mensaje)
        {
            if (campo == null) return;
            campo.Tag = "Error";
            campo.ToolTip = mensaje;
        }

        public void LimpiarError(TextBox campo)
        {
            if (campo == null) return;
            campo.ClearValue(Control.TagProperty);
            campo.ToolTip = null;
        }

        public void ActualizarHora(TextBox campo, string horaNormalizada)
        {
            if (campo == null) return;
            campo.Text = horaNormalizada;
        }

        public void MostrarMensaje(string mensaje) =>
            MessageBox.Show(mensaje, "Información", MessageBoxButton.OK, MessageBoxImage.Information);

        public void MostrarError(string mensaje) =>
            MessageBox.Show(mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        public void CerrarDialogo(bool result, Produccion produccion = null)
        {
            ProduccionCreada = produccion;
            DialogResult = result;
            Close();
        }

        public Produccion ObtenerDatosProduccion()
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

                // ✅ rango horario se valida con ValidarRangoHoras() antes de llegar acá

                if (!int.TryParse(CantidadTextBox.Text, out var cantidad))
                    throw new FormatException("Cantidad inválida.");

                // ✅ acá le pegamos al caso cantidad negativa / cero
                if (cantidad <= 0)
                    throw new InvalidOperationException("La cantidad debe ser mayor que 0.");

                var model = new Produccion
                {
                    ProductoId = _productoIdSeleccionado.Value,
                    PuestoId = _puestoIdSeleccionado.Value,
                    ProductoNombre = _productoNombreSeleccionado,
                    PuestoNombre = _puestoNombreSeleccionado,
                    HoraInicio = hInicio,
                    HoraFin = hFin,
                    Cantidad = cantidad
                };

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


        private void SeleccionarProducto_Click(object sender, RoutedEventArgs e)
        {
            var prompt = new Views.Designs.Prompts.PromptProducto(_activeUser)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            if (prompt.ShowDialog() == true)
            {
                var prod = prompt.ObtenerProductoSeleccionado();
                if (prod != null)
                {
                    _productoIdSeleccionado = prod.Id;
                    _productoNombreSeleccionado = prod.Nombre;

                    if (ProductoTextBox != null)
                        ProductoTextBox.Text = _productoNombreSeleccionado;
                }
            }
        }

        // Puesto
        private void SeleccionarPuesto_Click(object sender, RoutedEventArgs e)
        {
            var prompt = new Views.Designs.Prompts.PromptPuesto()
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            if (prompt.ShowDialog() == true)
            {
                var puesto = prompt.ObtenerPuestoSeleccionado();
                if (puesto != null)
                {
                    _puestoIdSeleccionado = puesto.PuestoId;
                    _puestoNombreSeleccionado = puesto.Nombre;

                    if (PuestoTextBox != null)
                        PuestoTextBox.Text = _puestoNombreSeleccionado;
                }
            }
        }

        // === Validaciones en foco / cambios de texto ===
        private void HoraInicioTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            _presenter.ValidarHora(HoraInicioTextBox, HoraInicioTextBox.Text);
            ValidarRangoHoras();  
        }

        private void HoraFinTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            _presenter.ValidarHora(HoraFinTextBox, HoraFinTextBox.Text);
            ValidarRangoHoras();   
        }

        private void CantidadTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Debe ser entero > 0
            _presenter.ValidarCantidad(CantidadTextBox, CantidadTextBox.Text);
        }

        private void PuestoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Opcional: limpiar feedback de error visual si lo hubiera
            if (sender is TextBox tb) LimpiarError(tb);
        }

        // === Botón Cancelar ===
        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool ValidarRangoHoras()
        {
            // Limpio errores previos
            LimpiarError(HoraInicioTextBox);
            LimpiarError(HoraFinTextBox);

            // Si alguna hora no se puede parsear, no valido rango acá.
            // El mensaje de formato ya lo da el presenter.
            if (!TimeSpan.TryParse(HoraInicioTextBox.Text, out var hInicio))
                return false;

            if (!TimeSpan.TryParse(HoraFinTextBox.Text, out var hFin))
                return false;

            if (hInicio >= hFin)
            {
                
                MostrarError(HoraInicioTextBox, "La hora de inicio debe ser menor que la hora de fin.");
                return false;
            }

            return true;
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
