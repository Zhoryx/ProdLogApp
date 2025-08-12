using System.Windows.Controls;
using ProdLogApp.Models;

namespace ProdLogApp.Interfaces
{
    public interface IAddProductionUserView
    {
        // Datos crudos
        string HoraInicio { get; }
        string HoraFin { get; }
        string Cantidad { get; }

        // Construcción del modelo
        Production ObtenerDatosProduccion();

        // Feedback general
        void MostrarMensaje(string mensaje);
        void MostrarError(string mensaje);

        // Feedback visual por campo
        void MostrarError(TextBox campo, string mensaje);
        void LimpiarError(TextBox campo);
        void ActualizarHora(TextBox campo, string horaNormalizada);

        // Limpieza y cierre
        void LimpiarFormulario();
        void CerrarDialogo(bool result, Production produccion = null);
    }
}
