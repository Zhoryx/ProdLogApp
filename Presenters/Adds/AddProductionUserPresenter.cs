using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Services;

namespace ProdLogApp.Presenters
{
    public class AddProductionUserPresenter
    {
        private readonly IAddProductionUserView _view;
        private readonly IDatabaseService _db;
        private readonly int _usuarioId;

        public AddProductionUserPresenter(IAddProductionUserView view, IDatabaseService db, int usuarioId)
        {
            _view = view;
            _db = db;
            _usuarioId = usuarioId;
        }

        // Validación de horas (normaliza hh:mm y marca errores visuales)
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
            input = input.Trim();

            // Si el usuario escribe 3 o 4 dígitos sin ':'
            if (!input.Contains(":") && int.TryParse(input, out var val))
            {
                if (input.Length <= 2) // “8” o “08” => 08:00
                    return TimeSpan.TryParse($"{val:00}:00", out hora);
                if (input.Length == 3) // “830” => 08:30
                    return TimeSpan.TryParse($"{val / 100:00}:{val % 100:00}", out hora);
                if (input.Length == 4) // “1230” => 12:30
                    return TimeSpan.TryParse($"{val / 100:00}:{val % 100:00}", out hora);
            }

            return TimeSpan.TryParse(input, out hora);
        }


        // Validación de cantidad
        public void ValidarCantidad(TextBox campo, string texto)
        {
            if (int.TryParse(texto, out var cant) && cant > 0)
            {
                _view.LimpiarError(campo);
            }
            else
            {
                _view.MostrarError(campo, "Ingrese un entero mayor que 0.");
            }
        }

        // Flujo principal: asegurar Parte y guardar Producción
        public async Task CargarProduccionAsync()
        {
            var prod = _view.ObtenerDatosProduccion();
            if (prod == null) return;

            if (prod.HInicio >= prod.HFin)
            {
                _view.MostrarError("La hora de inicio debe ser menor que la hora de fin.");
                return;
            }

            try
            {
                // 1) Asegurar Parte del día (idempotente)
                var hoy = DateTime.Today;
                int parteId = await _db.EnsureParteAsync(_usuarioId, hoy);
                prod.ParteId = parteId;

                // 2) Insertar producción
                int newId = await _db.InsertProduccionAsync(prod);
                prod.ProductionId = newId;

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
