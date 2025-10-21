using System;
using System.Collections.Generic;
using ProdLogApp.Models;

namespace ProdLogApp.Interfaces
{
    /// Interfaz que define el contrato de la vista para la gestión de usuarios.
    /// Implementa el patrón MVP: la vista expone eventos que el Presenter maneja.
    public interface IGestUsuariosVista
    {
        // Eventos que la vista dispara ante acciones del usuario (botones, etc.)
        event Action OnAgregar;           // Cuando se presiona "Agregar Usuario"
        event Action OnModificar;         // Cuando se presiona "Modificar Usuario"
        event Action OnAlternarEstado;    // Activa/desactiva usuario
        event Action OnResetearPassword;  // Permite reiniciar contraseña (solo gerente)
        event Action OnEliminar;          // Elimina usuario (si aplica lógica de borrado)
        event Action OnVolverMenu;        // Regresa al menú principal

        // Métodos que el Presenter invoca sobre la vista
        void MostrarUsuarios(List<Usuario> usuarios);           // Carga la lista de usuarios en la grilla
        Usuario ObtenerUsuarioSeleccionado();                   // Devuelve el usuario actualmente seleccionado

        void AbrirVentanaAgregarUsuario();                      // Abre ventana modal para alta
        void AbrirVentanaModificarUsuario(Usuario usuario);     // Abre ventana modal para edición

        void MostrarMensaje(string mensaje);                    // Muestra alertas o confirmaciones
        void NavegarAMenu();                                   // Retorna al menú principal
    }
}

