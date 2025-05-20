using System.Collections.Generic;
using ProdLogApp.Models;

namespace ProdLogApp.Views
{
    public interface IOperatorMenuView
    {
        void ShowMessage(string message);
        void UpdateProductionList(List<Production> productions);
    }
}
