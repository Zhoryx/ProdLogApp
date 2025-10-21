// Interfaces/IMenuOperarioVista.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProdLogApp.Models;

namespace ProdLogApp.Interfaces
{
    public interface IMenuOperarioVista
    {
        // Eventos que maneja el Presenter
        event Func<Task> OnCargarProduccion;
        event Func<Task> OnEliminarProduccionSeleccionada;
        event Func<Task> OnRefrescarListado;
        event Action OnSalir;

        // Métodos que el Presenter usa para interactuar con la UI
        void MostrarMensaje(string mensaje);
        void ActualizarListadoProducciones(List<Produccion> items);
        void AgregarProduccionAListado(Produccion produccion);
        Produccion ObtenerProduccionSeleccionada();

        // 👉 La vista abre el formulario y si el usuario confirma,
        //    devuelve el modelo creado; si cancela, devuelve null.
        Produccion ObtenerProduccionIngresada();
    }
}
