
using System;
using System.Text.RegularExpressions;
using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Servicios;

namespace ProdLogApp.Presenters
{
    public sealed class AgregarUsuarioPresenter
    {
        private readonly IAgregarUsuarioVista _vista;
        private readonly IServicioUsuarios _svc;
        private readonly Usuario? _editar;

        public AgregarUsuarioPresenter(IAgregarUsuarioVista vista, IServicioUsuarios servicioUsuarios, Usuario? aEditar = null)
        {
            _vista = vista;
            _svc = servicioUsuarios ?? throw new ArgumentNullException(nameof(servicioUsuarios));
            _editar = aEditar;

            _vista.AlAceptar += Guardar;
            _vista.AlCancelar += () => _vista.Cerrar();

            if (_editar != null)
            {
                _vista.CargarDatosIniciales(
                    _editar.Nombre,
                    _editar.Dni,
                    _editar.EsGerente,
                    _editar.FechaIngreso,
                    _editar.Activo
                );
            }
        }

        private async void Guardar()
        {
            // Leo datos desde la vista
            var nombre = _vista.ObtenerNombre()?.Trim();
            var dni = _vista.ObtenerDni()?.Trim();
            var esGerente = _vista.ObtenerEsGerente();
            var fechaIng = _vista.ObtenerFechaIngreso();
            var activo = _vista.ObtenerActivo();
            var passPlano = _vista.ObtenerPasswordInicial(); // opcional (alta) o cambio (edición)

            // Validaciones mínimas
            if (string.IsNullOrWhiteSpace(nombre))
            {
                _vista.MostrarMensaje("Ingresá el nombre.");
                return;
            }
            if (string.IsNullOrWhiteSpace(dni) || !Regex.IsMatch(dni, @"^\d{7,10}$"))
            {
                _vista.MostrarMensaje("Ingresá un DNI válido (7 a 10 dígitos).");
                return;
            }

            try
            {
                if (_editar == null)
                {
                    // Alta
                    // Evito duplicado por DNI
                    var existente = await _svc.ObtenerPorDniAsync(dni);
                    if (existente != null)
                    {
                        _vista.MostrarMensaje("Ya existe un usuario con ese DNI.");
                        return;
                    }

                    var nuevo = new Usuario
                    {
                        Nombre = nombre,
                        Dni = dni,
                        EsGerente = esGerente,
                        FechaIngreso = fechaIng,
                        Activo = activo
                    };

                    await _svc.CrearAsync(nuevo, passPlano ?? "1234");
                    _vista.MostrarMensaje("Usuario creado correctamente.");
                }
                else
                {
                    // Edición
                    _editar.Nombre = nombre;
                    _editar.Dni = dni;
                    _editar.EsGerente = esGerente;
                    _editar.FechaIngreso = fechaIng;
                    _editar.Activo = activo;

                    await _svc.ActualizarAsync(_editar);

                    // Si escribiste una contraseña nueva, la cambio
                    if (!string.IsNullOrWhiteSpace(passPlano))
                        await _svc.CambiarPasswordAsync(_editar.Id, passPlano);

                    _vista.MostrarMensaje("Usuario actualizado correctamente.");
                }

                _vista.Cerrar();
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al guardar el usuario: {ex.Message}");
            }
        }
    }
}
