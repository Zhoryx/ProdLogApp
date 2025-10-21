using System;
using System.Collections.Generic;
using ProdLogApp.Models;

namespace ProdLogApp.Interfaces
{
    public interface IPromptPuestoVista
    {
        event Action OnConfirmarSeleccion;
        event Action OnCancelar;

        void CargarPuestos(List<Puesto> puestos);
        Puesto ObtenerPuestoSeleccionado();
        void MostrarMensaje(string mensaje);
        void Cerrar();
    }
}
