using System;

namespace ProdLogApp.Views.Interfaces
{
    public interface IPositionManagementView
    {
        event Action OnAgregarPuesto;
        event Action OnEliminarPuesto;
        event Action OnModificarPuesto;
        event Action OnVolver;

        void CerrarVentana();
        void NavegarAMenu();
    }
}
