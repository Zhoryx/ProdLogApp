// Presenters/MenuOperarioPresenter.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using ProdLogApp.Interfaces;
using ProdLogApp.Servicios;
using ProdLogApp.Models;

namespace ProdLogApp.Presenters
{
    public sealed class MenuOperarioPresenter
    {
        private readonly IMenuOperarioVista _vista;
        private readonly IServicioProducciones _svcProd;
        private readonly int _usuarioIdActual;
        private readonly DateTime? _fechaFija; // fecha fija cuando se abre desde Gerente

        public MenuOperarioPresenter(
            IMenuOperarioVista vista,
            IServicioProducciones svcProd,
            int usuarioIdActual,
            DateTime? fechaFija = null)
        {
            _vista = vista ?? throw new ArgumentNullException(nameof(vista));
            _svcProd = svcProd ?? throw new ArgumentNullException(nameof(svcProd));
            _usuarioIdActual = usuarioIdActual;
            _fechaFija = fechaFija;

            _vista.OnCargarProduccion += async () => await CargarProduccionAsync();
            _vista.OnEliminarProduccionSeleccionada += async () => await EliminarSeleccionadaAsync();
            _vista.OnRefrescarListado += async () => await RefrescarListadoAsync();
            _vista.OnSalir += Salir;

            _ = RefrescarListadoAsync();
        }

        private async Task CargarProduccionAsync()
        {
            try
            {
                var prod = _vista.ObtenerProduccionIngresada();
                if (prod == null) return;

                if (!EsProduccionValida(prod, out _)) return;

                var fechaTrabajo = (_fechaFija ?? DateTime.Now.Date); // usar fecha fija si viene de Gerente
                var parteId = await _svcProd.AsegurarParteAsync(_usuarioIdActual, fechaTrabajo);

                var nuevoId = await _svcProd.InsertarProduccionAsync(prod, parteId);
                prod.ProduccionId = nuevoId;

                await RefrescarListadoAsync(); // refresca desde BD para traer nombres
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al cargar producción: {ex.Message}");
            }
        }

        private async Task RefrescarListadoAsync()
        {
            try
            {
                var fechaTrabajo = (_fechaFija ?? DateTime.Now.Date);
                var lista = await _svcProd.ListarPorFechaAsync(_usuarioIdActual, fechaTrabajo);
                _vista.ActualizarListadoProducciones(lista?.ToList() ?? []);
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al listar producciones: {ex.Message}");
            }
        }

        private async Task EliminarSeleccionadaAsync()
        {
            var sel = _vista.ObtenerProduccionSeleccionada();
            if (sel == null) return;

            var ok = System.Windows.MessageBox.Show(
                "¿Eliminar la producción seleccionada?",
                "Confirmar",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes;

            if (!ok) return;

            try
            {
                await _svcProd.EliminarAsync(sel.ProduccionId);
                await RefrescarListadoAsync();
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje($"Error al eliminar: {ex.Message}");
            }
        }

        private void Salir()
        {
            // hook por si más adelante necesitás algo al salir
        }

        private static bool EsProduccionValida(Produccion p, out string motivo)
        {
            if (p == null) { motivo = "formulario vacío."; return false; }
            if (p.ProductoId <= 0) { motivo = "producto inválido."; return false; }
            if (p.PuestoId <= 0) { motivo = "puesto inválido."; return false; }
            if (p.Cantidad <= 0) { motivo = "cantidad inválida."; return false; }
            if (p.HoraFin <= p.HoraInicio) { motivo = "horario inválido."; return false; }
            motivo = null;
            return true;
        }
    }
}
