using ProdLogApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdLogApp.Interfaces
{
   public interface IPromptPositionView
    {
       void MostrarPuestos(List<Position> puestos);
       void ObtenerPuestoSeleccionado(out int puestoId, out string descripcion);


    }
}
