using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Servicios;
using System;

namespace ProdLogApp.Presenters
{
    public sealed class LoginPresenter
    {
        private readonly ILoginVista _vista;
        private readonly IServicioUsuarios _svcUsuarios;

        public LoginPresenter(ILoginVista vista, IServicioUsuarios svcUsuarios)
        {
            _vista = vista ?? throw new ArgumentNullException(nameof(vista));
            _svcUsuarios = svcUsuarios ?? throw new ArgumentNullException(nameof(svcUsuarios));

            _vista.OnIntentarLogin += IntentarLogin;
            _vista.OnAbrirSolicitudPassword += AbrirSolicitudPassword;
        }

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

                // No pedimos password acá
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

                // Guardamos sesión activa
                UserSession.GetInstance().Set(usuario);

                // Flujo según rol
                if (usuario.EsGerente)
                    _vista.NavegarAMenuGerente();   // abrirá el popup de contraseña
                else
                    _vista.NavegarAMenuOperario();

                _vista.LimpiarCampos();
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al iniciar sesión: {ex.Message}");
            }
        }

        private void AbrirSolicitudPassword()
        {
            // Si tenés un botón “Olvidé mi contraseña”, podrías usar esto
            _vista.MostrarMensaje("Abrir pantalla de solicitud/cambio de contraseña.");
        }
    }
}
