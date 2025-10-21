using System;
using System.Collections.Generic;
using ProdLogApp.Models;

namespace ProdLogApp.Interfaces
{
    /// Interfaz para la vista de gestión de puestos.
    /// Cada evento corresponde a un botón o acción en la interfaz gráfica.
    public interface IGestPuestosVista
    {
        event Action OnAgregar;          // Botón "Agregar Puesto"
        event Action OnModificar;        // Botón "Modificar"
        event Action OnAlternarEstado;   // Activar/Desactivar puesto
        event Action OnEliminar;         // Borrar registro (si corresponde)
        event Action OnVolverMenu;       // Volver al menú principal

        void MostrarPuestos(List<Puesto> puestos);              // Carga datos en la grilla/listado
        Puesto ObtenerPuestoSeleccionado();                     // Devuelve el puesto actualmente seleccionado

        void AbrirVentanaAgregarPuesto();                       // Ventana modal para alta
        void AbrirVentanaModificarPuesto(Puesto puesto);        // Ventana modal para edición

        void MostrarMensaje(string mensaje);                    // Mensajes informativos o de error
        void NavegarAMenu();                                   // Regresa al menú
    }
}
