using System;

namespace ProdLogApp.Interfaces
{
   
    public interface IMenuProduccionGerenteVista
    {
        // Eventos de navegación
        event Action AlAbrirConsultaPorFecha;   
        event Action AlAbrirConsultaPorRango;   
        event Action AlAbrirDashboard;         
        event Action AlExportar;                
        event Action AlVolverMenuPrincipal;

        // Utilidades de UI
        void MostrarMensaje(string mensaje);
    }
}
