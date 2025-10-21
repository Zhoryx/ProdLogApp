using System;
using System.Linq;                   // ← necesario para .ToList()
using ProdLogApp.Interfaces;
using ProdLogApp.Servicios;

namespace ProdLogApp.Presenters
{
    // Presenter de gestión de usuarios.
    // Vincula eventos de la vista con operaciones del servicio y controla el ciclo de carga/refresco.
    public sealed class GestUsuariosPresenter
    {
        private readonly IGestUsuariosVista _vista;
        private readonly IServicioUsuarios _svc;

        public GestUsuariosPresenter(IGestUsuariosVista vista, IServicioUsuarios svc)
        {
            _vista = vista ?? throw new ArgumentNullException(nameof(vista));
            _svc = svc ?? throw new ArgumentNullException(nameof(svc));

            // Suscripciones a eventos de la vista
            _vista.OnAgregar += Agregar;
            _vista.OnModificar += Modificar;
            _vista.OnAlternarEstado += Alternar;
            _vista.OnResetearPassword += ResetearPassword;
            _vista.OnEliminar += Eliminar;
            _vista.OnVolverMenu += VolverMenu;

            // Carga inicial del listado
            Cargar();
        }

        // Carga/recarga los usuarios y los muestra en la vista
        private async void Cargar()
        {
            try
            {
                var lista = await _svc.ListarAsync();
                _vista.MostrarUsuarios(lista.ToList());
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al cargar usuarios: {ex.Message}");
            }
        }

        // Abre el diálogo de alta y al cerrar refresca el listado
        private void Agregar()
        {
            _vista.AbrirVentanaAgregarUsuario(); // modal
            Cargar(); // refresca al cerrar
        }

        // Abre el diálogo de edición para el elemento seleccionado
        private void Modificar()
        {
            var sel = _vista.ObtenerUsuarioSeleccionado();
            if (sel == null) { _vista.MostrarMensaje("Seleccioná un usuario."); return; }

            _vista.AbrirVentanaModificarUsuario(sel); // modal
            Cargar(); // refresca al cerrar
        }

        // Alterna el estado Activo del usuario seleccionado
        private async void Alternar()
        {
            var sel = _vista.ObtenerUsuarioSeleccionado();
            if (sel == null) { _vista.MostrarMensaje("Seleccioná un usuario."); return; }

            try
            {
                await _svc.AlternarEstadoAsync(sel.Id, !sel.Activo);
                Cargar();
                // Mensaje de éxito opcional:
                // _vista.MostrarMensaje(sel.Activo ? "Usuario desactivado." : "Usuario activado.");
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al alternar estado: {ex.Message}");
            }
        }

        // Restablece la contraseña del usuario seleccionado
        private async void ResetearPassword()
        {
            var sel = _vista.ObtenerUsuarioSeleccionado();
            if (sel == null) { _vista.MostrarMensaje("Seleccioná un usuario."); return; }

            try
            {
                await _svc.CambiarPasswordAsync(sel.Id, "1234");
                _vista.MostrarMensaje("Contraseña reseteada.");
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al resetear contraseña: {ex.Message}");
            }
        }

        // Elimina o inactiva el usuario seleccionado, según política
        private async void Eliminar()
        {
            var sel = _vista.ObtenerUsuarioSeleccionado();
            if (sel == null) { _vista.MostrarMensaje("Seleccioná un usuario."); return; }

            try
            {
                // Inactivación como alternativa a borrado físico
                await _svc.AlternarEstadoAsync(sel.Id, false);
                Cargar();
                // _vista.MostrarMensaje("Usuario inactivado."); // opcional
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al inactivar/eliminar: {ex.Message}");
            }
        }

        // Solicita a la vista la navegación correspondiente
        private void VolverMenu()
        {
            _vista.NavegarAMenu(); // la vista decide la acción concreta
        }
    }
}
