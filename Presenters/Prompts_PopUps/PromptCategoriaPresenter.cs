using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Servicios;

namespace ProdLogApp.Presenters
{
    // Presenter del prompt de selección de Categoría.
    // Carga categorías activas y permite filtrar por nombre o Id, además de ordenar.
    public sealed class PromptCategoriasPresenter
    {
        private readonly IPromptCategoriaVista _view;
        private readonly IServicioCategorias _svcCategorias;

        private List<Categoria> _original = new(); // Fuente completa (solo activos)
        private List<Categoria> _filtrada = new(); // Lista filtrada/ordenada

        public PromptCategoriasPresenter(IPromptCategoriaVista view, IServicioCategorias svcCategorias)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _svcCategorias = svcCategorias ?? throw new ArgumentNullException(nameof(svcCategorias));
        }

        // Carga categorías desde el servicio y filtra por Activo.
        public async Task CargarCategoriasAsync()
        {
            var data = await _svcCategorias.ListarAsync();

            _original = (data ?? new List<Categoria>())
                        .Where(c => c.Activo)
                        .ToList();

            _filtrada = new List<Categoria>(_original);
            _view.CargarCategorias(_filtrada);
        }

        // Filtra por nombre (contiene, case-insensitive) y por Id exacto si se provee código numérico.
        public void FiltrarCategorias(string nombre, string codigoTexto)
        {
            IEnumerable<Categoria> q = _original;

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var n = nombre.Trim().ToLowerInvariant();
                q = q.Where(c => (c.Nombre ?? "").ToLowerInvariant().Contains(n));
            }

            if (!string.IsNullOrWhiteSpace(codigoTexto) && int.TryParse(codigoTexto.Trim(), out var id))
            {
                q = q.Where(c => c.CategoriaId == id);
            }

            _filtrada = q.ToList();
            _view.CargarCategorias(_filtrada);
        }

        // Ordena por el campo indicado y dirección asc/desc.
        public void OrdenarPor(string campo, bool desc)
        {
            IOrderedEnumerable<Categoria> q = campo switch
            {
                "CategoriaId" => desc ? _filtrada.OrderByDescending(c => c.CategoriaId)
                                      : _filtrada.OrderBy(c => c.CategoriaId),
                "Nombre" => desc ? _filtrada.OrderByDescending(c => c.Nombre)
                                      : _filtrada.OrderBy(c => c.Nombre),
                _ => _filtrada.OrderBy(c => c.CategoriaId)
            };

            _filtrada = q.ToList();
            _view.CargarCategorias(_filtrada);
        }
    }
}
