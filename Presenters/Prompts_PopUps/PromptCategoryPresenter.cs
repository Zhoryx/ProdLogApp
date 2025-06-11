using ProdLogApp.Models;
using ProdLogApp.Services;
using ProdLogApp.Views.Designs.Prompts;
using System.Windows.Controls;
using ProdLogApp.Interfaces;

public class PromptCategoryPresenter
{
    private readonly IPromptCategoryView _view;
    private readonly IDatabaseService _databaseService;
    private List<Categoria> _categorias;

    public PromptCategoryPresenter(IPromptCategoryView view, IDatabaseService databaseService)
    {
        _view = view;
        _databaseService = databaseService;
        CargarCategorias();
    }

    private async void CargarCategorias() 
    {
        _categorias = await _databaseService.CategoriesGet(true);
        _view.MostrarCategorias(_categorias);
    }



    public void FiltrarCategorias(string filtroNombre, string filtroCodigo)
    {
        var resultado = _categorias.Where(c =>
            (string.IsNullOrEmpty(filtroNombre) || c.Nombre.Contains(filtroNombre, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrEmpty(filtroCodigo) || c.Id.ToString().StartsWith(filtroCodigo))
        ).ToList();

        _view.MostrarCategorias(resultado);
    }



}
