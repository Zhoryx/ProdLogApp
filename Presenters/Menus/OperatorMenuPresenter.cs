using System.Collections.Generic;
using ProdLogApp.Models;
using ProdLogApp.Services;
using ProdLogApp.Views;
using ProdLogApp.Interfaces;

namespace ProdLogApp.Presenters
{
    public class OperatorMenuPresenter
    {
        private readonly IOperatorMenuView _view;
        private readonly User _activeuser = UserSession.GetInstance().ActiveUser;
        private readonly IDatabaseService _databaseService;
        private List<Production> _productionList = new List<Production>();

        public OperatorMenuPresenter(IOperatorMenuView view, IDatabaseService databaseService)
        {
            _view = view;
            _databaseService = databaseService;
        }

        // Loads the daily production list from the database and updates the view
        public void LoadDailyProductions()
        {
            _productionList = _databaseService.GetDailyProductions();
            _view.UpdateProductionList(_productionList);
        }

        // Returns the current list of productions
        public List<Production> GetProductionList()
        {
            return _productionList;
        }

        // Opens the production form to add or modify production details
        public void OpenProductionForm(List<Production> productionList, int productionIndex)
        {
            var productionWindow = new ProductionForm(productionList, productionIndex, _activeuser,_databaseService);
            if (productionWindow.ShowDialog() == true)
            {
                _view.UpdateProductionList(_productionList);
            }
        }

        // Saves part production records using the database service and informs the user
        public void SavePartProductions()
        {
            int userId = UserSession.GetInstance().ActiveUser.Id;
            _databaseService.SavePartProductions(_productionList, userId);
            _view.ShowMessage("Part and Productions successfully saved.");
        }
    }
}
