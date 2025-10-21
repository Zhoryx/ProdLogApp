using System;

namespace ProdLogApp.Interfaces
{
    public interface IAgregarUsuarioVista
    {
        // Eventos
        event Action AlAceptar;
        event Action AlCancelar;

        // Datos ingresados en la vista
        string ObtenerNombre();
        string ObtenerDni();
        bool ObtenerEsGerente();
        DateTime? ObtenerFechaIngreso();
        bool ObtenerActivo();
        string? ObtenerPasswordInicial(); 

        // Utilidades de UI
        void CargarDatosIniciales(string nombre, string dni, bool esGerente, DateTime? fechaIngreso, bool activo);
        void MostrarMensaje(string mensaje);
        void Cerrar();
    }
}
