using System;

namespace ProdLogApp.Interfaces
{
    
    /// Vista del menú principal del Gerente (acciones de administración).
   
    public interface IMenuGerenteVista
    {
        // --- Eventos de navegación a módulos de gestión ---
        event Action OnAbrirGestionCategorias;  // ABM de Categorías
        event Action OnAbrirGestionProductos;   // ABM de Productos
        event Action OnAbrirGestionPuestos;     // ABM de Puestos
        event Action OnAbrirGestionUsuarios;    // ABM de Usuarios
        event Action OnAbrirPanelProduccion;    // Panel/consulta de producción (fecha/rango, etc.)
        event Action OnSalir;                   // Cerrar sesión / volver a login

        // --- Utilidades de UI ---
        void MostrarMensaje(string mensaje);    // Mensajes informativos/errores
    }
}
