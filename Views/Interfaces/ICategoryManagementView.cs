using System;

namespace ProdLogApp.Views.Interfaces
{
    public interface ICategoryManagementView
    {
        event Action OnAgregarCategoria;
        event Action OnEliminarCategoria;
        event Action OnModificarCategoria;
        event Action OnVolver;

        void CerrarVentana();
        void NavegarAMenu();
    }
}
