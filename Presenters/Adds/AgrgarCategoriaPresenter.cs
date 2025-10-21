using System;
using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Servicios;

namespace ProdLogApp.Presenters
{
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

            _vista.OnAceptar += Aceptar;
            _vista.OnCancelar += () => _vista.Cerrar();

            if (_editar != null)
                _vista.CargarDatosIniciales(_editar.Nombre, _editar.Activo);
        }

        private async void Aceptar()
        {
            var nombre = _vista.ObtenerNombre();
            var activo = _vista.ObtenerActivo();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                _vista.MostrarMensaje("Completá el nombre.");
                return;
            }

            try
            {
                if (_editar == null)
                {
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
                    _editar.Nombre = nombre;
                    _editar.Activo = activo;
                    await _svc.ActualizarAsync(_editar);
                    _vista.MostrarMensaje("Categoría actualizada.");
                }

                _vista.Cerrar();
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al guardar: {ex.Message}");
            }
        }
    }
}
