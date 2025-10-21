using System;
using System.Threading.Tasks;
using ProdLogApp.Interfaces; // IAgregarPuestoVista
using ProdLogApp.Models;     // Puesto
using ProdLogApp.Servicios;  // IServicioPuestos

namespace ProdLogApp.Presenters
{
    // Presenter para alta o edición de Puesto.
    // Conecta la vista con el servicio y aplica validaciones simples antes de guardar.
    public sealed class AgregarPuestoPresenter
    {
        private readonly IAgregarPuestoVista _vista;
        private readonly IServicioPuestos _servicio;
        private readonly Puesto _puestoEditando; // null indica alta

        public AgregarPuestoPresenter(
            IAgregarPuestoVista vista,
            IServicioPuestos servicio,
            Puesto puesto = null)
        {
            _vista = vista ?? throw new ArgumentNullException(nameof(vista));
            _servicio = servicio ?? throw new ArgumentNullException(nameof(servicio));
            _puestoEditando = puesto;

            // Asociación de eventos de la vista con los métodos del presenter
            _vista.OnAceptar += async () => await AceptarAsync();
            _vista.OnCancelar += Cancelar;

            // Precarga de datos en modo edición
            if (_puestoEditando != null)
                _vista.CargarDatosIniciales(_puestoEditando.Nombre ?? string.Empty, _puestoEditando.Activo);
        }

        // Maneja la confirmación de guardado, creando o actualizando según corresponda
        private async Task AceptarAsync()
        {
            var nombre = (_vista.ObtenerNombre() ?? string.Empty).Trim();
            var activo = _vista.ObtenerActivo();

            // Validación básica de campo requerido
            if (string.IsNullOrWhiteSpace(nombre))
            {
                _vista.MostrarMensaje("El nombre del puesto no puede estar vacío.");
                return;
            }

            try
            {
                if (_puestoEditando != null)
                {
                    // Edición existente
                    _puestoEditando.Nombre = nombre;
                    _puestoEditando.Activo = activo;
                    await _servicio.ActualizarAsync(_puestoEditando);
                    _vista.MostrarMensaje("Puesto actualizado correctamente.");
                }
                else
                {
                    // Alta de nuevo puesto
                    var nuevo = new Puesto { Nombre = nombre, Activo = activo };
                    var nuevoId = await _servicio.CrearAsync(nuevo);
                    nuevo.PuestoId = nuevoId;
                    _vista.MostrarMensaje("Puesto agregado correctamente.");
                }

                // Cierre de la vista tras la operación
                _vista.Cerrar();
            }
            catch (Exception ex)
            {
                // Captura de errores y mensaje al usuario
                _vista.MostrarMensaje($"Error al guardar el puesto: {ex.Message}");
            }
        }

        // Cierra la ventana sin realizar acciones
        private void Cancelar() => _vista.Cerrar();
    }
}
