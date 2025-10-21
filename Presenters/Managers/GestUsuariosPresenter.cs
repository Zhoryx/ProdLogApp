using System;
using System.Linq;                   // ← importante para .ToList()
using ProdLogApp.Interfaces;
using ProdLogApp.Servicios;

namespace ProdLogApp.Presenters
{
    public sealed class GestUsuariosPresenter
    {
        private readonly IGestUsuariosVista _vista;
        private readonly IServicioUsuarios _svc;

        public GestUsuariosPresenter(IGestUsuariosVista vista, IServicioUsuarios svc)
        {
            _vista = vista ?? throw new ArgumentNullException(nameof(vista));
            _svc = svc ?? throw new ArgumentNullException(nameof(svc));

            _vista.OnAgregar += Agregar;
            _vista.OnModificar += Modificar;
            _vista.OnAlternarEstado += Alternar;
            _vista.OnResetearPassword += ResetearPassword;
            _vista.OnEliminar += Eliminar;
            _vista.OnVolverMenu += VolverMenu;

            Cargar();
        }

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

        private void Agregar()
        {
            _vista.AbrirVentanaAgregarUsuario(); // modal
            Cargar(); // refresca al cerrar
        }

        private void Modificar()
        {
            var sel = _vista.ObtenerUsuarioSeleccionado();
            if (sel == null) { _vista.MostrarMensaje("Seleccioná un usuario."); return; }

            _vista.AbrirVentanaModificarUsuario(sel); // modal
            Cargar(); // refresca al cerrar
        }

        private async void Alternar()
        {
            var sel = _vista.ObtenerUsuarioSeleccionado();
            if (sel == null) { _vista.MostrarMensaje("Seleccioná un usuario."); return; }

            try
            {
                await _svc.AlternarEstadoAsync(sel.Id, !sel.Activo);
                Cargar();
                // Si no querés cartel de éxito, comentá la línea siguiente:
                // _vista.MostrarMensaje(sel.Activo ? "Usuario desactivado." : "Usuario activado.");
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al alternar estado: {ex.Message}");
            }
        }

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

        private async void Eliminar()
        {
            var sel = _vista.ObtenerUsuarioSeleccionado();
            if (sel == null) { _vista.MostrarMensaje("Seleccioná un usuario."); return; }

            try
            {
                // si no tenés delete físico, inactivar:
                await _svc.AlternarEstadoAsync(sel.Id, false);
                Cargar();
                // _vista.MostrarMensaje("Usuario inactivado."); // opcional
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al inactivar/eliminar: {ex.Message}");
            }
        }

        private void VolverMenu()
        {
            _vista.NavegarAMenu(); // la vista decide: cerrar y volver al owner
        }
    }
}
