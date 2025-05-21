using System;

namespace ProdLogApp.Views.Interfaces
{
    public interface IUserManagementView
    {
        event Action OnAgregarUsuario;
        event Action OnEliminarUsuario;
        event Action OnModificarUsuario;
        event Action OnVolver;

      
        void NavegarAMenu(); 
    }
}
