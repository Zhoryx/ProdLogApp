using ProdLogApp.Models;

namespace ProdLogApp.Interfaces
{
    public interface IPositionAddView
    {
        string NombreIngresado { get; }
        void CargarDatos(Position puesto);
        void MostrarMensaje(string mensaje);
        void CerrarConExito();
        void CerrarSinGuardar();
    }
}
