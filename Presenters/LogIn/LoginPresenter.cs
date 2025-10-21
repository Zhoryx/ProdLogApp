using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Servicios;
using System;

namespace ProdLogApp.Presenters
{
    // Presenter del flujo de login por DNI.
    // Resuelve usuario por DNI, verifica estado y decide navegación según rol.
    public sealed class LoginPresenter
    {
        private readonly ILoginVista _vista;
        private readonly IServicioUsuarios _svcUsuarios;

        public LoginPresenter(ILoginVista vista, IServicioUsuarios svcUsuarios)
        {
            _vista = vista ?? throw new ArgumentNullException(nameof(vista));
            _svcUsuarios = svcUsuarios ?? throw new ArgumentNullException(nameof(svcUsuarios));

            // Enlaza acciones de la vista con los manejadores del presenter.
            _vista.OnIntentarLogin += IntentarLogin;
            _vista.OnAbrirSolicitudPassword += AbrirSolicitudPassword;
        }

        // Maneja el intento de login: busca por DNI, guarda sesión y navega según rol.
        private async void IntentarLogin()
        {
            try
            {
                var dni = (_vista.ObtenerDni() ?? string.Empty).Trim();
                if (dni.Length == 0)
                {
                    _vista.MostrarMensaje("Ingresá el DNI.");
                    return;
                }

                // Resolución de usuario sin contraseña en este punto.
                var usuario = await _svcUsuarios.ObtenerPorDniAsync(dni);
                if (usuario == null)
                {
                    _vista.MostrarMensaje("Usuario no encontrado.");
                    return;
                }
                if (!usuario.Activo)
                {
                    _vista.MostrarMensaje("El usuario está inactivo.");
                    return;
                }

                // Establece sesión activa.
                UserSession.GetInstance().Set(usuario);

                // Navegación por rol: el gerente requerirá contraseña en el paso siguiente.
                if (usuario.EsGerente)
                    _vista.NavegarAMenuGerente();
                else
                    _vista.NavegarAMenuOperario();

                _vista.LimpiarCampos();
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al iniciar sesión: {ex.Message}");
            }
        }

        // Punto de extensión para abrir el diálogo de solicitud/cambio de contraseña.
        private void AbrirSolicitudPassword()
        {
            _vista.MostrarMensaje("Abrir pantalla de solicitud/cambio de contraseña.");
        }
    }
}
