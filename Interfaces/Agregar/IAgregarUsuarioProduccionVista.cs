using System;

namespace ProdLogApp.Interfaces
{
    /// Vista de alta de Usuario (variante utilizada en flujo de Producción).
    /// Nota: La interfaz es muy similar a IAgregarUsuarioVista; conviene revisar
    ///       si ambas se usan en presenters distintos o si podrían unificarse.
    public interface IAgregarUsuarioProduccionVista
    {
        // --- Eventos ---
        event Action OnAceptar;   // Confirmar alta dentro del flujo de Producción
        event Action OnCancelar;  // Cancelar

        // --- Lectura de datos de la vista ---
        string ObtenerNombre();
        string ObtenerDni();
        bool ObtenerEsGerente();
        DateTime? ObtenerFechaIngreso();
        bool ObtenerActivo();
        string ObtenerPasswordInicial(); // A diferencia de IAgregarUsuarioVista, acá no es nullable

        // --- Utilidades de UI ---
        void CargarDatosIniciales(string nombre, string dni, bool esGerente, DateTime? fechaIngreso, bool activo);
        void MostrarMensaje(string mensaje);
        void Cerrar();
    }
}
