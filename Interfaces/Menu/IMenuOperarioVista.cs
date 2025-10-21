// Interfaces/IMenuOperarioVista.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProdLogApp.Models;

namespace ProdLogApp.Interfaces
{
    
    /// Vista principal del Operario para gestionar sus producciones del día.
    /// Nota: esta interfaz mezcla eventos async (Func<Task>) y sync (Action).
    ///       Es útil cuando la acción del botón dispara lógica asincrónica en el Presenter.
    
    public interface IMenuOperarioVista
    {
        // --- Eventos manejados por el Presenter ---
        // Se usan Func<Task> para permitir await en la lógica del Presenter (carga/IO).
        event Func<Task> OnCargarProduccion;                // Alta de una nueva producción (abre formulario)
        event Func<Task> OnEliminarProduccionSeleccionada;  // Elimina la producción actualmente seleccionada
        event Func<Task> OnRefrescarListado;                // Refresca la grilla/listado
        event Action OnSalir;                               // Cierra sesión / vuelve a login

        // --- Métodos que el Presenter usa para interactuar con la UI ---
        void MostrarMensaje(string mensaje);                               // Feedback general
        void ActualizarListadoProducciones(List<Produccion> items);        // Pinta el listado completo
        void AgregarProduccionAListado(Produccion produccion);             // Agrega un item a la grilla
        Produccion ObtenerProduccionSeleccionada();                        // Devuelve el item seleccionado

        // --- Flujo de alta desde la vista ---
        // La vista abre el formulario de alta de Producción y, si el usuario confirma,
        // devuelve el modelo; si cancela, devuelve null. El Presenter debe validar null.
        Produccion ObtenerProduccionIngresada();
    }
}
