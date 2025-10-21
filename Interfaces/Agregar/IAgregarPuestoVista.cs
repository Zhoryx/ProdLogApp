using System;

namespace ProdLogApp.Interfaces
{
    /// Vista de alta/edición de Puesto.
    public interface IAgregarPuestoVista
    {
        // --- Eventos ---
        event Action OnAceptar;  // Confirmar
        event Action OnCancelar; // Cancelar

        // --- Datos de la vista ---
        string ObtenerNombre();
        bool ObtenerActivo();

        // --- Utilidades de UI ---
        void CargarDatosIniciales(string nombre, bool activo); // Inicializar campos (en edición o valores por defecto)
        void MostrarMensaje(string mensaje);
        void Cerrar();
    }
}
