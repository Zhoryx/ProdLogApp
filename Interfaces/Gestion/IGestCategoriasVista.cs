using System;
using System.Collections.Generic;
using ProdLogApp.Models;


namespace ProdLogApp.Interfaces
{
    public interface IGestCategoriasVista
    {
        // Eventos lanzados por la vista (acciones de usuario)
        event Action OnAgregar;          // Agregar nueva categoría
        event Action OnModificar;        // Editar categoría existente
        event Action OnAlternarEstado;   // Activar/Desactivar categoría
        event Action OnEliminar;         // Eliminar categoría
        event Action OnVolverMenu;       // Volver al menú principal

        // Métodos que el Presenter puede invocar
        void MostrarCategorias(List<Categoria> categorias);          // Cargar lista en la grilla
        Categoria ObtenerCategoriaSeleccionada();                    // Obtener registro activo

        void AbrirVentanaAgregarCategoria();                         // Ventana modal de alta
        void AbrirVentanaModificarCategoria(Categoria categoria);    // Ventana modal de edición

        void MostrarMensaje(string mensaje);                         // Mostrar feedback

        // Mantiene consistencia con las demás vistas
        void NavegarAMenu();                                         // Retornar al menú
    }
}