using System;

namespace ProdLogApp.Interfaces
{
    public interface IAgregarPuestoVista
    {
        event Action OnAceptar;
        event Action OnCancelar;

        string ObtenerNombre();
        bool ObtenerActivo();

        void CargarDatosIniciales(string nombre, bool activo);
        void MostrarMensaje(string mensaje);
        void Cerrar();
    }
}
