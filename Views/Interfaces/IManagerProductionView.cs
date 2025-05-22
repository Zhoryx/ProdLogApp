using System;

namespace ProdLogApp.Views.Interfaces
{
    public interface IManagerProductionView
    {
        event Action OnAgregarProduccion;
        event Action OnEliminarProduccion;
        event Action OnModificarProduccion;
        event Action OnVolver;

        void CerrarVentana();
        void NavegarAMenu(); 
    }
}
