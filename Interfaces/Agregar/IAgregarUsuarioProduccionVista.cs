using System;

namespace ProdLogApp.Interfaces
{
    
    public interface IAgregarUsuarioProduccionVista
    {
        event Action OnAceptar;
        event Action OnCancelar;

        string ObtenerNombre();
        string ObtenerDni();
        bool ObtenerEsGerente();
        DateTime? ObtenerFechaIngreso();
        bool ObtenerActivo();
        string ObtenerPasswordInicial();

        void CargarDatosIniciales(string nombre, string dni, bool esGerente, DateTime? fechaIngreso, bool activo);
        void MostrarMensaje(string mensaje);
        void Cerrar();
    }
}
