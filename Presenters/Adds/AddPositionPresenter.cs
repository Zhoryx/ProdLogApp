using ProdLogApp.Interfaces;
using ProdLogApp.Models;
using ProdLogApp.Services;

namespace ProdLogApp.Presenters
{
    public class AddPositionPresenter
    {
        private readonly IPositionAddView _view;
        private readonly IDatabaseService _databaseService;
        private readonly Position _puestoEditando;

        public AddPositionPresenter(IPositionAddView view, IDatabaseService databaseService, Position puesto = null)
        {
            _view = view;
            _databaseService = databaseService;
            _puestoEditando = puesto;

            if (_puestoEditando != null)
                _view.CargarDatos(_puestoEditando);
        }

        public void Confirmar()
        {
            var nombre = _view.NombreIngresado;

            if (string.IsNullOrWhiteSpace(nombre))
            {
                _view.MostrarMensaje("El nombre del puesto no puede estar vacío.");
                return;
            }

            if (_puestoEditando != null)
            {
                _puestoEditando.Nombre = nombre;
                _databaseService.ActualizarPuesto(_puestoEditando);
            }
            else
            {
                var nuevo = new Position { Nombre = nombre };
                _databaseService.AgregarPuesto(nuevo);
            }

            _view.CerrarConExito();
        }

        public void Cancelar()
        {
            _view.CerrarSinGuardar();
        }
    }
}
