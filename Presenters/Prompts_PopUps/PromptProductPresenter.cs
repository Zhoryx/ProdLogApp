using ProdLogApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdLogApp.Presenters.Prompts_PopUps
{
    internal class PromptProductPresenter
    {
        private readonly IPromptProductView _view;
        private readonly IDatabaseService _databaseService;

        public PromptProductPresenter(IPromptProductView view, IDatabaseService databaseService)
        {
            _view = view;
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
         
            //_view.OnReturn += ReturnToMenu;
        }
        public void CargarProductos()
        {
            var productos = _databaseService.ObtenerTodosLosProductos();
            _view.MostrarProductos(productos);
        }

        public async Task CargarCategorias()
        {
            var categorias = await _databaseService.CategoriesGet(true); // solo activas si querés
            _view.MostrarCategorias(categorias);
        }

    }
}
