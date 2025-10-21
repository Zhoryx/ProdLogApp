using System.Windows.Controls;
using ProdLogApp.Models;

namespace ProdLogApp.Interfaces
{
    public interface IFormularioProduccionVista
    {
        // Lecturas de la UI
        string HoraInicio { get; }
        string HoraFin { get; }
        string Cantidad { get; }

        // Validación/feedback de campos
        void MostrarError(TextBox campo, string mensaje);
        void LimpiarError(TextBox campo);
        void ActualizarHora(TextBox campo, string horaNormalizada);

        // Mensajes generales
        void MostrarMensaje(string mensaje);
        void MostrarError(string mensaje);

        // Flujo de cierre
        void CerrarDialogo(bool result, Produccion produccion = null);

        // Construcción del modelo desde la UI
        Produccion ObtenerDatosProduccion();

        // Limpieza (tras alta exitosa, etc.)
        void LimpiarFormulario();
    }
}
