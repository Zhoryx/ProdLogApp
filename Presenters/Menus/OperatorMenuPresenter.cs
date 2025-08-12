using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Services;
using ProdLogApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProdLogApp.Presenters
{
    public class OperatorMenuPresenter
    {
        private readonly IOperatorMenuView _view;
        private readonly IDatabaseService _databaseService;
        private readonly User _activeUser = UserSession.GetInstance().ActiveUser;
        private int _currentParteId;
  
        private List<Production> _productionList = new();

        public OperatorMenuPresenter(IOperatorMenuView view, IDatabaseService databaseService)
        {
            _view = view;
            _databaseService = databaseService;
        }
        public async Task LoadDailyProductionsAsync(int usuarioId, DateTime fecha)
        {
            // Asegurar parte (aunque sea solo para prevenir inserciones huérfanas luego)
            _currentParteId = await _databaseService.EnsureParteAsync(usuarioId, fecha);

            var producciones = await _databaseService.GetProductionsAsync(usuarioId, fecha);
            _view.UpdateProductionList(producciones);
        }



        public async Task SaveProductionAsync(int usuarioId, DateTime fecha)
        {
            // 1) Obtener datos de la vista
            var produccion = _view.GetProductionInput();

            // 2) Validaciones (horas, cantidad, etc.)
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

            // 3) Asegurar Parte_Id
            int parteId = await _databaseService.EnsureParteAsync(usuarioId, fecha);
            produccion.ParteId = parteId;

            // 4) Guardar producción
            produccion.ProductionId = await _databaseService.InsertProduccionAsync(produccion);

            // 5) Actualizar vista
            _view.AddProductionToList(produccion);
            _view.ShowMessage("Producción registrada correctamente.");
        }




        public async Task LoadDailyProductionsForActiveUserAsync(DateTime day)
        {
            try
            {
                // Firma recomendada en el servicio: filtrar en la consulta
                _productionList = await _databaseService.GetProductionsAsync(_activeUser.Id, day)
                                  ?? new List<Production>();

                // Si tu servicio aún no tiene ese método, como alternativa temporal:
                // var all = await _databaseService.GetProductionsByDateAsync(day) ?? new List<Production>();
                // _productionList = all.Where(p => p.OperarioId == _activeUser.Id).ToList();

                _view.UpdateProductionList(_productionList);
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Error al cargar producciones: {ex.Message}");
            }
        }

        public bool OpenProductionFormForActiveUser(Production? toEdit = null)
        {
            // El ProductionForm debería aceptar user/operario para asegurar ownership
            var form = new ProductionForm(_activeUser, _databaseService);
            bool result = form.ShowDialog() == true;
            return result;
        }

        public void RemoveFromCurrentList(Production production)
        {
            if (production == null) return;
            _productionList.Remove(production);
            _view.UpdateProductionList(_productionList);
        }

        public void SavePartProductionsForActiveUser()
        {
            try
            {
                _databaseService.SavePartProductions(_productionList, _activeUser.Id);
                _view.ShowMessage("Parte y producciones guardados correctamente.");
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Error al guardar: {ex.Message}");
            }
        }
    }
}
