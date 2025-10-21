using System;
using System.Collections.Generic;
using ProdLogApp.Models;

namespace ProdLogApp.Interfaces
{
    public interface IGestPuestosVista
    {
        event Action OnAgregar;
        event Action OnModificar;
        event Action OnAlternarEstado;
        event Action OnEliminar;
        event Action OnVolverMenu;

        void MostrarPuestos(List<Puesto> puestos);
        Puesto ObtenerPuestoSeleccionado();

        void AbrirVentanaAgregarPuesto();
        void AbrirVentanaModificarPuesto(Puesto puesto);

        void MostrarMensaje(string mensaje);
        void NavegarAMenu();
    }
}
