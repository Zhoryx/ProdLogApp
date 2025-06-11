using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Services;
using ProdLogApp.Views.Interfaces;
using System;

namespace ProdLogApp.Presenters
{
    public class AddCategoryPresenter
    {
        private readonly IAddCategoryView _view;
        private readonly IDatabaseService _databaseService;
        private Categoria _categoriaActual;

        public AddCategoryPresenter(IAddCategoryView view, IDatabaseService databaseService, Categoria categoria = null)
        {
            _view = view;
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _categoriaActual = categoria;

            if (_categoriaActual != null)
            {
                _view.CargarDatosCategoria(_categoriaActual); // ✅ Carga datos si es edición
            }
        }

        // ✅ Método para agregar o modificar una categoría
        public void GuardarCategoria(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                _view.MostrarMensaje("El nombre de la categoría no puede estar vacío.");
                return;
            }

            if (_categoriaActual == null)
            {
                // ✅ Nueva categoría
                _databaseService.AgregarCategoria(new Categoria { Nombre = nombre });
                _view.MostrarMensaje("Categoría agregada correctamente.");
            }
            else
            {
                // ✅ Modificación de categoría existente
                _categoriaActual.Nombre = nombre;
                _databaseService.ActualizarCategoria(_categoriaActual);
                _view.MostrarMensaje("Categoría modificada correctamente.");
            }

            _view.CerrarVentana(); // ✅ Cierra la ventana después de guardar
        }
    }
}
