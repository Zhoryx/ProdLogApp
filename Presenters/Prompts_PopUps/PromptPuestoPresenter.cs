using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProdLogApp.Interfaces; // IPromptPuestoVista
using ProdLogApp.Models;     // Puesto
using ProdLogApp.Servicios;  // IServicioPuestos

namespace ProdLogApp.Presenters.Prompts_PopUps
{
    public class PromptPuestoPresenter
    {
        private readonly IPromptPuestoVista _view;
        private readonly IServicioPuestos _servicio;

        private List<Puesto> _original = new();
        private List<Puesto> _filtrados = new();

        public PromptPuestoPresenter(IPromptPuestoVista view, IServicioPuestos servicio)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _servicio = servicio ?? throw new ArgumentNullException(nameof(servicio));
            // No llamamos CargarPuestosAsync aquí para evitar doble carga.
        }

        public async Task CargarPuestosAsync()
        {
            try
            {
                var puestos = await _servicio.ListarAsync(); // usamos el método general
                _original = (puestos ?? Array.Empty<Puesto>())
                                .Where(p => p.Activo)       // ← filtramos solo activos
                                .ToList();

                _filtrados = new List<Puesto>(_original);
                _view.CargarPuestos(_filtrados);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al cargar puestos: {ex.Message}");
            }
        }


        public void FiltrarPorNombre(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                _filtrados = new List<Puesto>(_original);
            }
            else
            {
                var term = texto.Trim().ToLowerInvariant();
                _filtrados = _original
                    .Where(p => !string.IsNullOrEmpty(p.Nombre) &&
                                p.Nombre.ToLowerInvariant().Contains(term))
                    .ToList();
            }

            _view.CargarPuestos(_filtrados);
        }

        public void FiltrarPorCodigo(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                _filtrados = new List<Puesto>(_original);
            }
            else if (int.TryParse(texto.Trim(), out int codigo))
            {
                _filtrados = _original.Where(p => p.PuestoId == codigo).ToList();
            }
            else
            {
                _filtrados = new List<Puesto>();
            }

            _view.CargarPuestos(_filtrados);
        }

        public void OrdenarPor(string campo)
        {
            _filtrados = campo switch
            {
                "PuestoId" => _filtrados.OrderBy(p => p.PuestoId).ToList(),
                "Nombre" => _filtrados.OrderBy(p => p.Nombre).ToList(),
                _ => _filtrados
            };

            _view.CargarPuestos(_filtrados);
        }
    }
}
