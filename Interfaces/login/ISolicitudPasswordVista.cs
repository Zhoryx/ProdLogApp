using System;

namespace ProdLogApp.Interfaces
{
    /// Contrato de la vista que solicita contraseña (ej. cuando el DNI pertenece a un gerente).
    /// Generalmente se abre como diálogo/modal desde la pantalla de login.
    public interface ISolicitudPasswordVista
    {
        // --- Eventos que la vista dispara ---
        event Action OnConfirmarSolicitud; // Cuando el usuario confirma la contraseña ingresada
        event Action OnCancelar;           // Cuando se cancela el diálogo

        // --- Lectura del dato ingresado ---
        string ObtenerPasswordIngresada(); // Devuelve el texto del campo PasswordBox o TextBox

        // --- Utilitarios de UI ---
        void MostrarMensaje(string mensaje); // Mensaje de error o información
        void Cerrar();                       // Cierra la ventana/modal (tras confirmar o cancelar)
    }
}
