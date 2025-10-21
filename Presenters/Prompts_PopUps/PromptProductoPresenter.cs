using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProdLogApp.Interfaces;   // IPromptProductoVista
using ProdLogApp.Models;       // Producto
using ProdLogApp.Servicios;    // IServicioProductos

namespace ProdLogApp.Presenters.Prompts_PopUps
{
    public sealed class PromptProductPresenter
    {
        private readonly IPromptProductoVista _view;
        private readonly IServicioProductos _svc;

        private List<Producto> _todos = new();

        public PromptProductPresenter(IPromptProductoVista view, IServicioProductos servicioProductos)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _svc = servicioProductos ?? throw new ArgumentNullException(nameof(servicioProductos));
        }

        public async Task CargarProductosAsync()
        {
            try
            {
                var items = await _svc.ListarAsync();

               
                _todos = (items ?? Array.Empty<Producto>())
                            .Where(p => p.Activo)
                            .ToList();

                _view.CargarProductos(_todos);
            }
            catch (Exception ex)
            {
                _view.MostrarMensaje($"Error al cargar productos: {ex.Message}");
            }
        }


        // Filtrado en memoria: nombre, código (Id) y categoría (por Id o por nombre)
        public void FiltrarProductos(string nombre, string codigoTexto, string categoriaTexto)
        {
            IEnumerable<Producto> q = _todos;

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var n = nombre.Trim().ToLowerInvariant();
                q = q.Where(p => (p.Nombre ?? "").ToLowerInvariant().Contains(n));
            }

            if (!string.IsNullOrWhiteSpace(codigoTexto))
            {
                var frag = codigoTexto.Trim();
                if (int.TryParse(frag, out int id))
                    q = q.Where(p => p.Id == id);
                else
                    q = q.Where(p => p.Id.ToString().Contains(frag));
            }

            if (!string.IsNullOrWhiteSpace(categoriaTexto))
            {
                var cat = categoriaTexto.Trim();

                // Si es número, filtramos por CategoriaId
                if (int.TryParse(cat, out int catId))
                {
                    q = q.Where(p => p.CategoriaId == catId);
                }
                else
                {
                    // Si es texto, filtramos por nombre de categoría
                    var c = cat.ToLowerInvariant();
                    q = q.Where(p => (p.CategoriaNombre ?? "").ToLowerInvariant().Contains(c));
                }
            }

            _view.CargarProductos(q.ToList());
        }
    }
}
