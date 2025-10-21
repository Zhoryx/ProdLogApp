using System;
using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Servicios;

namespace ProdLogApp.Presenters
{
    // Presenter para alta/edición de Categoría.
    // Enlaza eventos de la vista con operaciones del servicio y controla validaciones mínimas.
    public sealed class AgregarCategoriaPresenter
    {
        private readonly IAgregarCategoriaVista _vista;
        private readonly IServicioCategorias _svc;
        private readonly Categoria? _editar;

        public AgregarCategoriaPresenter(IAgregarCategoriaVista vista, IServicioCategorias servicioCategorias, Categoria? aEditar = null)
        {
            _vista = vista;
            _svc = servicioCategorias ?? throw new ArgumentNullException(nameof(servicioCategorias));
            _editar = aEditar;

            // Suscripción a eventos de la vista
            _vista.OnAceptar += Aceptar;
            _vista.OnCancelar += () => _vista.Cerrar();

            // Precarga de datos en modo edición
            if (_editar != null)
                _vista.CargarDatosIniciales(_editar.Nombre, _editar.Activo);
        }

        // Manejador principal del flujo de guardado (alta o edición)
        private async void Aceptar()
        {
            var nombre = _vista.ObtenerNombre();
            var activo = _vista.ObtenerActivo();

            // Validación simple de requerido
            if (string.IsNullOrWhiteSpace(nombre))
            {
                _vista.MostrarMensaje("Completá el nombre.");
                return;
            }

            try
            {
                if (_editar == null)
                {
                    // Alta: evita duplicados por nombre
                    if (await _svc.ExisteNombreAsync(nombre))
                    {
                        _vista.MostrarMensaje("Ya existe una categoría con ese nombre.");
                        return;
                    }

                    var cat = new Categoria { Nombre = nombre, Activo = activo };
                    await _svc.CrearAsync(cat);
                    _vista.MostrarMensaje("Categoría creada.");
                }
                else
                {
                    // Edición: aplica cambios sobre la instancia existente
                    _editar.Nombre = nombre;
                    _editar.Activo = activo;
                    await _svc.ActualizarAsync(_editar);
                    _vista.MostrarMensaje("Categoría actualizada.");
                }

                // Cierra el diálogo tras completar operación
                _vista.Cerrar();
            }
            catch (Exception ex)
            {
                // Mensaje de error controlado
                _vista.MostrarMensaje($"Error al guardar: {ex.Message}");
            }
        }
    }
}
