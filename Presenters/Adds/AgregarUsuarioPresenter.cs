using System;
using System.Text.RegularExpressions;
using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Servicios;

namespace ProdLogApp.Presenters
{
    // Presenter para alta/edición de Usuario.
    // Toma datos desde la vista, valida, coordina alta/edición y cambio de password con el servicio.
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

            // Suscripción a eventos de la vista
            _vista.AlAceptar += Guardar;
            _vista.AlCancelar += () => _vista.Cerrar();

            // Precarga de datos en modo edición
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

        // Manejador principal del flujo de guardado (alta o edición)
        private async void Guardar()
        {
            // Lectura de datos provenientes de la vista
            var nombre = _vista.ObtenerNombre()?.Trim();
            var dni = _vista.ObtenerDni()?.Trim();
            var esGerente = _vista.ObtenerEsGerente();
            var fechaIng = _vista.ObtenerFechaIngreso();
            var activo = _vista.ObtenerActivo();
            var passPlano = _vista.ObtenerPasswordInicial(); // Puede ser null/empty según flujo

            // Validaciones mínimas de campos
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
                    // Alta: evita duplicado por DNI
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

                    // Password inicial por defecto si no se ingresa uno
                    await _svc.CrearAsync(nuevo, passPlano ?? "1234");
                    _vista.MostrarMensaje("Usuario creado correctamente.");
                }
                else
                {
                    // Edición: aplica cambios en datos básicos
                    _editar.Nombre = nombre;
                    _editar.Dni = dni;
                    _editar.EsGerente = esGerente;
                    _editar.FechaIngreso = fechaIng;
                    _editar.Activo = activo;

                    await _svc.ActualizarAsync(_editar);

                    // Cambio de contraseña opcional
                    if (!string.IsNullOrWhiteSpace(passPlano))
                        await _svc.CambiarPasswordAsync(_editar.Id, passPlano);

                    _vista.MostrarMensaje("Usuario actualizado correctamente.");
                }

                // Cierra el diálogo tras completar operación
                _vista.Cerrar();
            }
            catch (Exception ex)
            {
                // Mensaje de error controlado
                _vista.MostrarMensaje($"Error al guardar el usuario: {ex.Message}");
            }
        }
    }
}
