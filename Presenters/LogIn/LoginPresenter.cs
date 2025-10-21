using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Servicios;
using System;

namespace ProdLogApp.Presenters
{
    // Presenter del flujo de login por DNI.
    // Resuelve usuario por DNI, verifica estado y decide navegaci�n seg�n rol.
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

        // Maneja el intento de login: busca por DNI, guarda sesi�n y navega seg�n rol.
        private async void IntentarLogin()
        {
            try
            {
                var dni = (_vista.ObtenerDni() ?? string.Empty).Trim();
                if (dni.Length == 0)
                {
                    _vista.MostrarMensaje("Ingres� el DNI.");
                    return;
                }

                // Resoluci�n de usuario sin contrase�a en este punto.
                var usuario = await _svcUsuarios.ObtenerPorDniAsync(dni);
                if (usuario == null)
                {
                    _vista.MostrarMensaje("Usuario no encontrado.");
                    return;
                }
                if (!usuario.Activo)
                {
                    _vista.MostrarMensaje("El usuario est� inactivo.");
                    return;
                }

                // Establece sesi�n activa.
                UserSession.GetInstance().Set(usuario);

                // Navegaci�n por rol: el gerente requerir� contrase�a en el paso siguiente.
                if (usuario.EsGerente)
                    _vista.NavegarAMenuGerente();
                else
                    _vista.NavegarAMenuOperario();

                _vista.LimpiarCampos();
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al iniciar sesi�n: {ex.Message}");
            }
        }

        // Punto de extensi�n para abrir el di�logo de solicitud/cambio de contrase�a.
        private void AbrirSolicitudPassword()
        {
            _vista.MostrarMensaje("Abrir pantalla de solicitud/cambio de contrase�a.");
        }
    }
}
