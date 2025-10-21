using System;

namespace ProdLogApp.Interfaces
{
    public interface IAgregarProductoVista
    {
        event Action OnAceptar;
        event Action OnCancelar;
        event Action OnSeleccionarCategoria; 

        string ObtenerNombre();
        int ObtenerCategoriaId();
        bool ObtenerActivo();

        void CargarDatosIniciales(string nombre, int categoriaId, bool activo);
        void MostrarMensaje(string mensaje);
        void Cerrar();

        // Interacción con prompt de categoría
        void AbrirPromptCategoria();
        void EstablecerCategoriaSeleccionada(int categoriaId, string categoriaNombre);
    }
}
