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

        // Cache del parte actual (usuario + fecha)
        private int _currentParteId;
        private DateTime _currentParteDate;

        private List<Production> _productionList = new();

        public OperatorMenuPresenter(IOperatorMenuView view, IDatabaseService databaseService)
        {
            _view = view;
            _databaseService = databaseService;
        }

        // Carga para usuario arbitrario (si lo necesitás)
        public async Task LoadDailyProductionsAsync(int usuarioId, DateTime fecha)
        {
            _currentParteDate = fecha.Date;

            // Si es el usuario activo, cacheamos también el Parte_Id
            if (usuarioId == _activeUser.Id)
                _currentParteId = await _databaseService.EnsureParteAsync(usuarioId, _currentParteDate);

            _productionList = await _databaseService.GetProductionsAsync(usuarioId, _currentParteDate)
                             ?? new List<Production>();

            _view.UpdateProductionList(_productionList);
        }

        // Carga para el usuario activo (principal)
        public async Task LoadDailyProductionsForActiveUserAsync(DateTime day)
        {
            _currentParteDate = day.Date;

            //  Garantiza un único Parte por (usuario, fecha) y lo cachea
            _currentParteId = await _databaseService.EnsureParteAsync(_activeUser.Id, _currentParteDate);

            _productionList = await _databaseService.GetProductionsAsync(_activeUser.Id, _currentParteDate)
                             ?? new List<Production>();

            _view.UpdateProductionList(_productionList);
        }

        // Abrir formulario para crear/editar
        public bool OpenProductionFormForActiveUser(Production? toEdit = null)
        {
            if (toEdit == null)
            {
                // CREATE: pasamos el Parte_Id actual para que el form inserte ligado al mismo Parte
                var form = new ProductionForm(_activeUser, _databaseService, isEdit: false, editId: null, parteId: _currentParteId);
                return form.ShowDialog() == true;
            }
            else
            {
                // EDIT: el form recarga por Id y hace UPDATE
                var form = new ProductionForm(_activeUser, _databaseService, isEdit: true, editId: toEdit.ProductionId);
                return form.ShowDialog() == true;
            }
        }

        // (Alternativa) Alta hecha por el presenter en vez del form (opcional, por si la usás)
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

            // 🔑 Usar el Parte cacheado si corresponde; si no, asegurar uno
            if (usuarioId == _activeUser.Id && fecha.Date == _currentParteDate)
                produccion.ParteId = _currentParteId;
            else
                produccion.ParteId = await _databaseService.EnsureParteAsync(usuarioId, fecha.Date);

            produccion.ProductionId = await _databaseService.InsertProduccionAsync(produccion);

            _productionList.Add(produccion);
            _view.UpdateProductionList(_productionList);
            _view.ShowMessage("Producción registrada correctamente.");
        }

        // Eliminar producción (DB + memoria)
        public async Task DeleteProductionAsync(Production production)
        {
            if (production == null) return;

            await _databaseService.DeleteProduccionAsync(production.ProductionId);

            _productionList.Remove(production);
            _view.UpdateProductionList(_productionList);
        }

        // Guardar "parte + producciones" del usuario activo en la fecha actual (async)
        public async Task SavePartProductionsForActiveUserAsync()
        {
            try
            {
                // Versión async con FECHA para que todas queden bajo el mismo Parte_Id
                await _databaseService.SavePartProductionsAsync(_productionList, _activeUser.Id, _currentParteDate);
                _view.ShowMessage("Parte y producciones guardados correctamente.");
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Error al guardar: {ex.Message}");
            }
        }
    }
}
