using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProdLogApp.Presenters.Prompts_PopUps
{
    internal class PromptProductPresenter
    {
        private readonly IPromptProductView _view;
        private readonly IDatabaseService _databaseService;
        private List<Producto> _productos;

        public PromptProductPresenter(IPromptProductView view, IDatabaseService databaseService)
        {
            _view = view;
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
        }

        public void CargarProductos()
        {
            _productos = _databaseService.ObtenerTodosLosProductos();
            _view.MostrarProductos(_productos);
        }

        public void FiltrarProductos(string filtroNombre, string filtroCodigo, string filtroCategoria)
        {
            if (_productos == null)
            {
                _view.MostrarProductos(new List<Producto>());
                return;
            }

            var resultado = _productos.Where(p =>
                (string.IsNullOrWhiteSpace(filtroNombre) || p.Nombre.Contains(filtroNombre, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(filtroCodigo) || p.Id.ToString().StartsWith(filtroCodigo)) &&
                (string.IsNullOrWhiteSpace(filtroCategoria) || p.CategoriaNombre.Contains(filtroCategoria, StringComparison.OrdinalIgnoreCase))
            ).ToList();

            _view.MostrarProductos(resultado);
        }
    }
}
