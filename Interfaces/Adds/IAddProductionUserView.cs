using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProdLogApp.Interfaces
{
    public interface IAddProductionUserView
    {
        string HoraInicio { get; }
        string HoraFin { get; }
        string Cantidad { get; }

        void MostrarError(TextBox campo, string mensaje);
        void LimpiarError(TextBox campo);
        void ActualizarHora(TextBox campo, string horaNormalizada);
    }
}