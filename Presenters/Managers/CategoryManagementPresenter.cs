using ProdLogApp.Interfaces;

public class CategoryManagementPresenter
{
    private readonly ICategoryManagementView _view;
    private readonly IDatabaseService _databaseService;

    public CategoryManagementPresenter(IDatabaseService databaseService, ICategoryManagementView view)
    {
        _view = view;
        _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));

        _view.OnAddCategory += AddCategory;
        _view.OnModifyCategory += ModifyCategory;
        _view.OnToggleCategoryStatus += ToggleCategoryStatus; 
        CategoriesGet();
    }

    private async void CategoriesGet()
    {
        var categorias = await _databaseService.CategoriesGet();
        _view.MostrarCategorias(categorias);
    }

    private void AddCategory()
    {
        _view.AbrirVentanaAgregarCategoria();
        CategoriesGet();
    }

    private void ModifyCategory()
    {
        Categoria categoriaSeleccionada = _view.ObtenerCategoriaSeleccionada();
        if (categoriaSeleccionada == null)
        {
            _view.MostrarMensaje("Seleccione una categoría para modificar.");
            return;
        }

        _view.AbrirVentanaModificarCategoria(categoriaSeleccionada);
        CategoriesGet();
    }

    private void ToggleCategoryStatus()
    {
        Categoria categoriaSeleccionada = _view.ObtenerCategoriaSeleccionada();

        if (categoriaSeleccionada == null)
        {
            _view.MostrarMensaje("Seleccione una categoría para cambiar su estado.");
            return;
        }

        categoriaSeleccionada.Activo = !categoriaSeleccionada.Activo;
        _databaseService.ToggleCategoryStatus(categoriaSeleccionada.CategoryId, categoriaSeleccionada.Activo);
        _view.MostrarMensaje(categoriaSeleccionada.Activo ? "Puesto desactivado correctamente." : "Puesto activado correctamente.");

        CategoriesGet(); 
    }

   
}
