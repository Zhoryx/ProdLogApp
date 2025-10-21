using System;

namespace ProdLogApp.Interfaces
{
    public interface ISolicitudPasswordVista
    {
        // Eventos UI
        event Action OnConfirmarSolicitud;
        event Action OnCancelar;

        // Dato de entrada
        string ObtenerPasswordIngresada();

        // Utilitarios
        void MostrarMensaje(string mensaje);
        void Cerrar();
    }
}
