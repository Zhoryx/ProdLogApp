using System.Collections.Generic;
using ProdLogApp.Models;

namespace ProdLogApp.Interfaces
{
    public interface IOperatorMenuView
    {
        // Displays a message to the user
        void ShowMessage(string message);

        // Updates the production list in the view
        void UpdateProductionList(List<Production> productions);

        

    }
}
