using System;

namespace ProdLogApp.Interfaces
{
    /// Vista de alta/edición de Producto.
    public interface IAgregarProductoVista
    {
        // --- Eventos ---
        event Action OnAceptar;             // Confirmar alta/edición
        event Action OnCancelar;            // Cancelar
        event Action OnSeleccionarCategoria; // Abrir prompt de selección de categoría

        // --- Datos de la vista ---
        string ObtenerNombre();
        int ObtenerCategoriaId(); // Seleccionado vía prompt
        bool ObtenerActivo();

        // --- Utilidades de UI ---
        void CargarDatosIniciales(string nombre, int categoriaId, bool activo);
        void MostrarMensaje(string mensaje);
        void Cerrar();

        // --- Integración con prompt de categoría ---
        // La vista puede abrir el prompt y luego recibir el resultado (Id + Nombre) para pintar en UI.
        void AbrirPromptCategoria();
        void EstablecerCategoriaSeleccionada(int categoriaId, string categoriaNombre);
    }
}
