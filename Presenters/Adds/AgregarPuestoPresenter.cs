using System;
using System.Threading.Tasks;
using ProdLogApp.Interfaces; // IAgregarPuestoVista
using ProdLogApp.Models;     // Puesto
using ProdLogApp.Servicios;  // IServicioPuestos

namespace ProdLogApp.Presenters
{
    public sealed class AgregarPuestoPresenter
    {
        private readonly IAgregarPuestoVista _vista;
        private readonly IServicioPuestos _servicio;
        private readonly Puesto _puestoEditando; // null => alta

        public AgregarPuestoPresenter(
            IAgregarPuestoVista vista,
            IServicioPuestos servicio,
            Puesto puesto = null)
        {
            _vista = vista ?? throw new ArgumentNullException(nameof(vista));
            _servicio = servicio ?? throw new ArgumentNullException(nameof(servicio));
            _puestoEditando = puesto;

            // Enlazo eventos de la vista
            _vista.OnAceptar += async () => await AceptarAsync();
            _vista.OnCancelar += Cancelar;

            // Si edito, precargo los datos
            if (_puestoEditando != null)
                _vista.CargarDatosIniciales(_puestoEditando.Nombre ?? string.Empty, _puestoEditando.Activo);
        }

        private async Task AceptarAsync()
        {
            var nombre = (_vista.ObtenerNombre() ?? string.Empty).Trim();
            var activo = _vista.ObtenerActivo();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                _vista.MostrarMensaje("El nombre del puesto no puede estar vacío.");
                return;
            }

            try
            {
                if (_puestoEditando != null)
                {
                    // Modificación
                    _puestoEditando.Nombre = nombre;
                    _puestoEditando.Activo = activo;
                    await _servicio.ActualizarAsync(_puestoEditando);
                    _vista.MostrarMensaje("Puesto actualizado correctamente.");
                }
                else
                {
                    // Alta
                    var nuevo = new Puesto { Nombre = nombre, Activo = activo };
                    var nuevoId = await _servicio.CrearAsync(nuevo);
                    nuevo.PuestoId = nuevoId;
                    _vista.MostrarMensaje("Puesto agregado correctamente.");
                }

                _vista.Cerrar();
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al guardar el puesto: {ex.Message}");
            }
        }

        private void Cancelar() => _vista.Cerrar();
    }
}
