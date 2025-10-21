using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using ProdLogApp.Interfaces;     // IFormularioProduccionVista
using ProdLogApp.Models;         // Produccion
using ProdLogApp.Servicios;      // IServicioProducciones

namespace ProdLogApp.Presenters
{
    public sealed class AgregarProduccionPresenter
    {
        private readonly IFormularioProduccionVista _view;
        private readonly IServicioProducciones _svcProducciones;
        private readonly int _usuarioId;

        public AgregarProduccionPresenter(IFormularioProduccionVista view,
                                          IServicioProducciones svcProducciones,
                                          int usuarioId)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _svcProducciones = svcProducciones ?? throw new ArgumentNullException(nameof(svcProducciones));
            _usuarioId = usuarioId;
        }

        // ========= Validaciones de entrada =========

        public void ValidarHora(TextBox campo, string texto)
        {
            if (TryParseHoraFlexible(texto, out var ts)
                && ts >= TimeSpan.Zero
                && ts < TimeSpan.FromDays(1))
            {
                _view.LimpiarError(campo);
                _view.ActualizarHora(campo, $"{ts.Hours:00}:{ts.Minutes:00}");
            }
            else
            {
                _view.MostrarError(campo, "Formato de hora inválido (use HH:mm)");
            }
        }

        private bool TryParseHoraFlexible(string input, out TimeSpan hora)
        {
            hora = default;
            if (input == null) return false;

            input = input.Trim();

            // 1, 2, 3 o 4 dígitos sin ':'
            if (!input.Contains(":") && int.TryParse(input, out var val))
            {
                if (input.Length <= 2) // "8" -> 08:00
                    return TimeSpan.TryParse($"{val:00}:00", out hora);
                if (input.Length == 3) // "830" -> 08:30
                    return TimeSpan.TryParse($"{val / 100:00}:{val % 100:00}", out hora);
                if (input.Length == 4) // "1230" -> 12:30
                    return TimeSpan.TryParse($"{val / 100:00}:{val % 100:00}", out hora);
            }

            return TimeSpan.TryParse(input, out hora);
        }

        public void ValidarCantidad(TextBox campo, string texto)
        {
            if (int.TryParse(texto, out var cant) && cant > 0)
                _view.LimpiarError(campo);
            else
                _view.MostrarError(campo, "Ingrese un entero mayor que 0.");
        }

        // ========= Flujo principal =========

        public async Task CargarProduccionAsync()
        {
            var prod = _view.ObtenerDatosProduccion();
            if (prod == null) return;

            // Validaciones finales (por si el form dejó pasar algo)
            if (prod.HoraInicio >= prod.HoraFin)
            {
                _view.MostrarMensaje("La hora de inicio debe ser menor que la hora de fin.");
                return;
            }
            if (prod.Cantidad <= 0)
            {
                _view.MostrarMensaje("La cantidad debe ser mayor que 0.");
                return;
            }

            try
            {
                // 1) Asegurar cabecera de Parte (idempotente)
                int parteId = await _svcProducciones.AsegurarParteAsync(_usuarioId, DateTime.Today);
                prod.ParteId = parteId;

                // 2) Insertar producción
                int nuevoId = await _svcProducciones.InsertarProduccionAsync(prod, parteId);
                prod.ProduccionId = nuevoId;

                _view.MostrarMensaje("Producción guardada correctamente.");
                _view.CerrarDialogo(true, prod);
            }
            catch (Exception ex)
            {
                _view.MostrarError($"No se pudo guardar la producción: {ex.Message}");
            }
        }
    }
}
