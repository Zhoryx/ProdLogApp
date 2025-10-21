using System;

namespace ProdLogApp.Interfaces
{
    
    /// Vista de menú de Producción (perfil Gerente).
    /// Expone opciones de navegación y utilidades de UI para feedback.
   
    public interface IMenuProduccionGerenteVista
    {
        // --- Eventos de navegación/disparadores de acciones ---
        // Se disparan cuando el usuario hace clic en cada opción del menú.
        event Action AlAbrirConsultaPorFecha;   // Abre consulta de partes por una fecha específica
        event Action AlAbrirConsultaPorRango;   // Abre consulta de partes por rango de fechas
        event Action AlAbrirDashboard;          // Abre dashboard (gráficos / KPIs), si está implementado
        event Action AlExportar;                // Exportación (por ej. CSV/PDF), si está implementada
        event Action AlVolverMenuPrincipal;     // Vuelve al menú principal de gerente

        // --- Utilidades de UI ---
        void MostrarMensaje(string mensaje);    // Muestra mensajes informativos/errores al usuario
    }
}
