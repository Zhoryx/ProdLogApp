// Presenters/PasswordRequestPresenter.cs
using System;
using System.Threading.Tasks;
using ProdLogApp.Interfaces;
using ProdLogApp.Servicios;
using ProdLogApp.Models;

namespace ProdLogApp.Presenters
{
    // Presenter para validar contraseña de un usuario ya identificado por DNI.
    // Orquesta la lectura de la contraseña desde la vista y delega la verificación en el servicio.
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

            // Enlaza confirmación/cancelación del diálogo con los handlers del presenter.
            _vista.OnConfirmarSolicitud += async () => await ConfirmarAsync();
            _vista.OnCancelar += Cancelar;
        }

        // Ejecuta la validación: lee la contraseña, valida no vacío y consulta al servicio.
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

                // Éxito: ejecuta la acción indicada por el llamador (por ejemplo, navegar a menú gerente)
                _onSuccess.Invoke();
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al validar: {ex.Message}");
            }
        }

        // Cierra la vista sin cambios.
        private void Cancelar() => _vista.Cerrar();
    }
}
