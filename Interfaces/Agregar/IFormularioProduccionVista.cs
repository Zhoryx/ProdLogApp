using System.Windows.Controls;
using ProdLogApp.Models;

namespace ProdLogApp.Interfaces
{
    /// Contrato de la vista del formulario de Producción (alta/edición).
    /// Nota: esta interfaz expone tipos de WPF (TextBox) hacia el Presenter.
    ///       Es cómodo, pero acopla la capa de interfaz. Lo marco para evaluar luego.
    public interface IFormularioProduccionVista
    {
        // --- Lecturas directas de campos de la UI ---
        // Se exponen como string para que el Presenter pueda validar y normalizar.
        string HoraInicio { get; }   // Ej. "08:00"
        string HoraFin { get; }    // Ej. "13:30"
        string Cantidad { get; }    // Ej. "25"

        // --- Validación y feedback por campo ---
        // MostrarError/LimpiarError permiten marcar un campo inválido con mensaje contextual.
        // ActualizarHora permite que el Presenter normalice formateo (HH:mm) y lo reescriba en el TextBox.
        void MostrarError(TextBox campo, string mensaje);
        void LimpiarError(TextBox campo);
        void ActualizarHora(TextBox campo, string horaNormalizada);

        // --- Mensajes generales (no asociados a un campo puntual) ---
        void MostrarMensaje(string mensaje);
        void MostrarError(string mensaje); // Sobrecarga de nombre con distinta firma (String); OK por claridad

        // --- Flujo de cierre del diálogo/modal ---
        // result: true si el alta/edición fue confirmada; false si se canceló.
        // produccion: devolver el modelo resultante (si aplica) para que el Presenter lo reciba en el cierre.
        void CerrarDialogo(bool result, Produccion produccion = null);

        // --- Construcción del modelo desde la UI ---
        // Alternativa a leer los tres strings por separado. El Presenter puede:
        // 1) Validar strings => 2) Si OK, pedir el modelo consolidado a la vista.
        Produccion ObtenerDatosProduccion();

        // --- Limpieza del formulario (útil tras un alta exitosa) ---
        void LimpiarFormulario();
    }
}
