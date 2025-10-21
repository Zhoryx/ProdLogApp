using System;
using System.Collections.Generic;
using ProdLogApp.Models;

namespace ProdLogApp.Interfaces
{
    // Vista de selección de Puesto (prompt o ventana modal).
    // Permite al usuario elegir un Puesto desde una lista y confirmar o cancelar.
    public interface IPromptPuestoVista
    {
        // Eventos que la vista dispara
        event Action OnConfirmarSeleccion; // El usuario confirma la selección
        event Action OnCancelar;            // El usuario cancela el diálogo

        // Métodos que el Presenter invoca sobre la vista
        void CargarPuestos(List<Puesto> puestos);        // Carga los puestos disponibles en la lista/grilla
        Puesto ObtenerPuestoSeleccionado();              // Devuelve el puesto actualmente seleccionado
        void MostrarMensaje(string mensaje);             // Muestra advertencias o errores
        void Cerrar();                                   // Cierra la ventana/modal
    }
}
