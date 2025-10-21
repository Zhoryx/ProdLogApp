// Presenters/PasswordRequestPresenter.cs
using System;
using System.Threading.Tasks;
using ProdLogApp.Interfaces;
using ProdLogApp.Servicios;
using ProdLogApp.Models;

namespace ProdLogApp.Presenters
{
    public sealed class PasswordRequestPresenter
    {
        private readonly ISolicitudPasswordVista _vista;
        private readonly IServicioUsuarios _svcUsuarios;
        private readonly Usuario _usuario;
        private readonly Action _onSuccess;

        public PasswordRequestPresenter(
            ISolicitudPasswordVista vista,
            IServicioUsuarios svcUsuarios,
            Usuario usuario,
            Action onSuccess)
        {
            _vista = vista ?? throw new ArgumentNullException(nameof(vista));
            _svcUsuarios = svcUsuarios ?? throw new ArgumentNullException(nameof(svcUsuarios));
            _usuario = usuario ?? throw new ArgumentNullException(nameof(usuario));
            _onSuccess = onSuccess ?? (() => { });

            _vista.OnConfirmarSolicitud += async () => await ConfirmarAsync();
            _vista.OnCancelar += Cancelar;
        }

        private async Task ConfirmarAsync()
        {
            try
            {
                var pass = _vista.ObtenerPasswordIngresada()?.Trim() ?? string.Empty;
                if (string.IsNullOrEmpty(pass))
                {
                    _vista.MostrarMensaje("Ingresá la contraseña.");
                    return;
                }

                var usuarioOk = await _svcUsuarios.LoginAsync(_usuario.Dni, pass);
                if (usuarioOk == null)
                {
                    _vista.MostrarMensaje("Contraseña incorrecta.");
                    return;
                }

            
                _onSuccess.Invoke();
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al validar: {ex.Message}");
            }
        }

        private void Cancelar() => _vista.Cerrar();
    }
}
