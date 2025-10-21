using System;
using System.Collections.Generic;
using ProdLogApp.Models;

namespace ProdLogApp.Interfaces
{
    public interface IGestUsuariosVista
    {
        event Action OnAgregar;
        event Action OnModificar;
        event Action OnAlternarEstado;
        event Action OnResetearPassword;
        event Action OnEliminar;
        event Action OnVolverMenu;

        void MostrarUsuarios(List<Usuario> usuarios);
        Usuario ObtenerUsuarioSeleccionado();

        void AbrirVentanaAgregarUsuario();
        void AbrirVentanaModificarUsuario(Usuario usuario);

        void MostrarMensaje(string mensaje);
        void NavegarAMenu();
    }
}
