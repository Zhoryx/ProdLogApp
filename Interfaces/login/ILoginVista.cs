using System;

namespace ProdLogApp.Interfaces
{
    public interface ILoginVista
    {
        event Action OnIntentarLogin;
        event Action OnAbrirSolicitudPassword;

        string ObtenerDni();
        string ObtenerPassword(); // la vista actual puede devolver string.Empty

        void MostrarMensaje(string mensaje);
        void LimpiarCampos();
        void NavegarAMenuOperario();
        void NavegarAMenuGerente();
    }
}
