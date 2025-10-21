using System;

namespace ProdLogApp.Interfaces
{
    /// Vista para el alta de Usuario (diálogo/modal de creación).
    public interface IAgregarUsuarioVista
    {
        // --- Eventos que la vista dispara ---
        event Action AlAceptar;   // Se confirma el alta
        event Action AlCancelar;  // Se cancela el alta

        // --- Lectura de datos de entrada desde la vista ---
        string ObtenerNombre();
        string ObtenerDni();             // Formato esperado: solo dígitos (Presenter valida)
        bool ObtenerEsGerente();
        DateTime? ObtenerFechaIngreso();    // Puede venir null si el control lo permite
        bool ObtenerActivo();
        string? ObtenerPasswordInicial(); // Puede ser null si no se requiere para operarios

        // --- Utilidades de UI ---
        // Permite inicializar la vista con valores por defecto (útil en reintentos)
        void CargarDatosIniciales(string nombre, string dni, bool esGerente, DateTime? fechaIngreso, bool activo);

        void MostrarMensaje(string mensaje); // Mensajes informativos/errores
        void Cerrar();                       // Cerrar el modal
    }
}
