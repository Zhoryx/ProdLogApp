using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Services;
using ProdLogApp.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProdLogApp.Presenters
{
    public class OperatorMenuPresenter
    {
        private readonly IOperatorMenuView _view;
        private readonly IDatabaseService _databaseService;
        private readonly User _activeUser = UserSession.GetInstance().ActiveUser;

        private int _currentParteId;
        private DateTime _currentParteDate;
        private int _currentUsuarioId;

        private List<Production> _productionList = new();

        public OperatorMenuPresenter(IOperatorMenuView view, IDatabaseService databaseService)
        {
            _view = view;
            _databaseService = databaseService;

            // Doble click en una producción => abrir en modo Editar
            _view.OnProductionDoubleClick += OnProductionDoubleClick;
        }

        // --- Cargas de contexto ---

        // Modo gerente / auditoría (usuario arbitrario)
        public async Task LoadDailyProductionsAsync(int usuarioId, DateTime fecha)
        {
            _currentUsuarioId = usuarioId;
            _currentParteDate = fecha.Date;

            _currentParteId = await _databaseService.EnsureParteAsync(usuarioId, _currentParteDate);

            _productionList = await _databaseService.GetProductionsAsync(usuarioId, _currentParteDate)
                             ?? new List<Production>();

            _view.UpdateProductionList(_productionList);
        }

        // Modo operario (usuario activo)
        public async Task LoadDailyProductionsForActiveUserAsync(DateTime day)
        {
            _currentUsuarioId = _activeUser.Id;
            _currentParteDate = day.Date;

            _currentParteId = await _databaseService.EnsureParteAsync(_activeUser.Id, _currentParteDate);

            _productionList = await _databaseService.GetProductionsAsync(_activeUser.Id, _currentParteDate)
                             ?? new List<Production>();

            _view.UpdateProductionList(_productionList);
        }

        // --- Alta/Edición ---

        // Abre el formulario usando el Parte del contexto actual
        public bool OpenProductionFormForActiveUser(Production? toEdit = null)
        {
            if (toEdit == null)
            {
                // CREATE: se vincula al Parte del contexto actual (_currentParteId)
                var form = new ProductionForm(
                    _activeUser,           // identidad visual; la vinculación real es por ParteId
                    _databaseService,
                    isEdit: false,
                    editId: null,
                    parteId: _currentParteId
                );
                return form.ShowDialog() == true;
            }
            else
            {
                // EDIT: el form recarga por Id y hace UPDATE
                var form = new ProductionForm(_activeUser, _databaseService, isEdit: true, editId: toEdit.ProductionId);
                return form.ShowDialog() == true;
            }
        }

        // Doble click en la lista => editar y refrescar
        private async void OnProductionDoubleClick(Production prod)
        {
            if (prod == null) return;

            bool edited = OpenProductionFormForActiveUser(prod);
            if (edited)
                await RefreshCurrentListAsync();
        }

        // Refresca la grilla según el contexto cacheado (usuario/fecha)
        private async Task RefreshCurrentListAsync()
        {
            _productionList = await _databaseService
                .GetProductionsAsync(_currentUsuarioId, _currentParteDate)
                ?? new List<Production>();

            _view.UpdateProductionList(_productionList);
        }

        // --- Guardados ---

        // Guarda el "parte + producciones" del CONTEXTO ACTUAL (usuario/fecha cacheados)
        public async Task SavePartProductionsForCurrentContextAsync()
        {
            try
            {
                await _databaseService.SavePartProductionsAsync(_productionList, _currentUsuarioId, _currentParteDate);
                _view.ShowMessage("Parte y producciones guardados correctamente.");
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Error al guardar: {ex.Message}");
            }
        }

        // Conveniencia: guarda una producción usando el CONTEXTO ACTUAL
        public Task SaveProductionAsync()
            => SaveProductionAsync(_currentUsuarioId, _currentParteDate);

        // Guarda una producción para un usuario/fecha específicos (si difiere del contexto, asegura el Parte)
        public async Task SaveProductionAsync(int usuarioId, DateTime fecha)
        {
            var produccion = _view.GetProductionInput();
            if (produccion == null) return;

            if (produccion.HInicio >= produccion.HFin)
            {
                _view.ShowMessage("La hora de inicio debe ser menor que la hora de fin.");
                return;
            }
            if (produccion.Cantidad <= 0)
            {
                _view.ShowMessage("La cantidad debe ser mayor que cero.");
                return;
            }

            // Usar el Parte cacheado si coincide el contexto; si no, asegurar uno
            if (usuarioId == _currentUsuarioId && fecha.Date == _currentParteDate)
                produccion.ParteId = _currentParteId;
            else
                produccion.ParteId = await _databaseService.EnsureParteAsync(usuarioId, fecha.Date);

            produccion.ProductionId = await _databaseService.InsertProduccionAsync(produccion);

            _productionList.Add(produccion);
            _view.UpdateProductionList(_productionList);
            _view.ShowMessage("Producción registrada correctamente.");
        }

        // --- Eliminación ---

        // Eliminar producción (DB + memoria)
        public async Task DeleteProductionAsync(Production production)
        {
            if (production == null) return;

            await _databaseService.DeleteProduccionAsync(production.ProductionId);

            _productionList.Remove(production);
            _view.UpdateProductionList(_productionList);
        }
    }
}
