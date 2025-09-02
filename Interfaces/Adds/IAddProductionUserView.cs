using System.Windows.Controls;
using ProdLogApp.Models;

namespace ProdLogApp.Interfaces
{
    public interface IAddProductionUserView
    {
        // Lectura de datos crudos del formulario
        string HoraInicio { get; }
        string HoraFin { get; }
        string Cantidad { get; }

        // Construcción del modelo desde la vista (usado por el form)
        Production ObtenerDatosProduccion();

        // Feedback general
        void MostrarMensaje(string mensaje);
        void MostrarError(string mensaje);

        // Feedback visual por campo (validaciones)
        void MostrarError(TextBox campo, string mensaje);
        void LimpiarError(TextBox campo);
        void ActualizarHora(TextBox campo, string horaNormalizada);

        // Limpieza y cierre de diálogo
        void LimpiarFormulario();
        void CerrarDialogo(bool result, Production produccion = null);
    }
}
