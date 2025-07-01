using System;
using System.Collections.Generic;
using ProdLogApp.Models;

namespace ProdLogApp.Interfaces
{
    public interface IPositionManagementView
    {
        event Action OnAddPosition;
        event Action OnDeletePosition;
        event Action OnModifyPosition;
        event Action OnReturn;

        void CloseWindow();
        void NavigateToMenu();

        void AbrirVentanaAgregar();
        void AbrirVentanaModificar(Position puesto);
        Position ObtenerPuestoSeleccionado();
        void MostrarMensaje(string mensaje);
        void MostrarPuestos(List<Position> puestos);
     
    }
}
