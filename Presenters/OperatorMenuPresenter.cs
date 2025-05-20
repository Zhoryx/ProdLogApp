using System.Collections.Generic;
using ProdLogApp.Models;
using ProdLogApp.Services;
using ProdLogApp.Views;

namespace ProdLogApp.Presenters
{
    public class OperatorMenuPresenter
    {
        private readonly IOperatorMenuView _view;
        private readonly IDatabaseService _databaseService;
        private List<Production> _productionList = new List<Production>();

        public OperatorMenuPresenter(IOperatorMenuView view, IDatabaseService databaseService)
        {
            _view = view;
            _databaseService = databaseService;
        }

        public void LoadDailyProductions()
        {
            _productionList = _databaseService.GetDailyProductions();
            _view.UpdateProductionList(_productionList);
        }

        public List<Production> GetProductionList()
        {
            return _productionList; 
        }

        public void OpenProductionForm(List<Production> productionList, int productionIndex)
        {
            var productionWindow = new ProductionForm(productionList, productionIndex);
            if (productionWindow.ShowDialog() == true)
            {
                _view.UpdateProductionList(_productionList); 
            }
        }

        public void SavePartProductions()
        {
            int userId = UserSession.GetInstance().ActiveUser.Id; 
            _databaseService.SavePartProductions(_productionList, userId);
            _view.ShowMessage("✅ Part and Productions successfully saved.");
        }
    }
}
