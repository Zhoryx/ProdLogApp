using System;

namespace ProdLogApp.Interfaces
{
    /// Contrato de la vista de inicio de sesión (pantalla principal de acceso).
    /// Forma parte del patrón MVP: la vista expone eventos y el Presenter maneja la lógica.
    public interface ILoginVista
    {
        // --- Eventos que la vista dispara ---
        event Action OnIntentarLogin;         // Se lanza al presionar "Iniciar sesión"
        event Action OnAbrirSolicitudPassword; // Se lanza si el usuario requiere ingresar contraseña (modo gerente)

        // --- Lectura de datos ingresados ---
        string ObtenerDni();       // DNI del usuario (operario o gerente)
        string ObtenerPassword();  // Password; puede ser string.Empty si operario (sin contraseña)

        // --- Feedback general ---
        void MostrarMensaje(string mensaje);  // Mostrar alerta, error o info (MessageBox, label, etc.)
        void LimpiarCampos();                 // Limpia los campos de entrada (para nuevo intento)

        // --- Navegación según tipo de usuario ---
        void NavegarAMenuOperario(); // Si login corresponde a un operario
        void NavegarAMenuGerente();  // Si login corresponde a un gerente
    }
}
