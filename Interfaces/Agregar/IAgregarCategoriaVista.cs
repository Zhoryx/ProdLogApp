using System;

namespace ProdLogApp.Interfaces
{
    /// Vista de alta/edición de Categoría.
    public interface IAgregarCategoriaVista
    {
        // --- Eventos ---
        event Action OnAceptar;  // Confirmar
        event Action OnCancelar; // Cancelar

        // --- Datos de la vista ---
        string ObtenerNombre();
        bool ObtenerActivo();

        // --- Utilidades de UI ---
        void CargarDatosIniciales(string nombre, bool activo);
        void MostrarMensaje(string mensaje);
        void Cerrar();
    }
}
