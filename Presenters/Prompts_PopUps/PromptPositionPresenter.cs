using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProdLogApp.Presenters
{
    public class PromptPositionPresenter
    {
        private readonly IPromptPositionView _view;
        private readonly IDatabaseService _databaseService;

        private List<Position> _positionsOriginal = new();
        private List<Position> _positionsFiltered = new();

        public PromptPositionPresenter(IPromptPositionView view, IDatabaseService databaseService)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));

            CargarPuestos(); // ✅ Carga automática al construir
        }

        public void CargarPuestos()
        {
            try
            {
                var puestos = _databaseService.ObtenerTodosLosPuestos(); // ✅ Garantiza datos incluso si no hay activos

                _positionsOriginal = puestos ?? new List<Position>();
                _positionsFiltered = new List<Position>(_positionsOriginal);

                _view.MostrarPuestos(_positionsFiltered);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al cargar puestos: {ex.Message}");
            }
        }

        public void FiltrarPorNombre(string texto)
        {
            _positionsFiltered = string.IsNullOrWhiteSpace(texto)
                ? new List<Position>(_positionsOriginal)
                : _positionsOriginal
                    .Where(p => p.Nombre != null && p.Nombre.ToLower().Contains(texto.Trim().ToLower()))
                    .ToList();

            _view.MostrarPuestos(_positionsFiltered);
        }

        public void FiltrarPorCodigo(string texto)
        {
            _positionsFiltered = string.IsNullOrWhiteSpace(texto)
                ? new List<Position>(_positionsOriginal)
                : int.TryParse(texto.Trim(), out int codigo)
                    ? _positionsOriginal.Where(p => p.PuestoId == codigo).ToList()
                    : new List<Position>();

            _view.MostrarPuestos(_positionsFiltered);
        }

        public void OrdenarPor(string campo)
        {
            _positionsFiltered = campo switch
            {
                "PuestoId" => _positionsFiltered.OrderBy(p => p.PuestoId).ToList(),
                "Nombre" => _positionsFiltered.OrderBy(p => p.Nombre).ToList(),
                _ => _positionsFiltered
            };

            _view.MostrarPuestos(_positionsFiltered);
        }
    }
}
