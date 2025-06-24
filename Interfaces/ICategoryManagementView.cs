public interface ICategoryManagementView
{
    event Action OnAddCategory;
    event Action OnModifyCategory;
    event Action OnToggleCategoryStatus;

    void MostrarCategorias(List<Categoria> categorias);
    Categoria ObtenerCategoriaSeleccionada();
    void AbrirVentanaAgregarCategoria();
    void AbrirVentanaModificarCategoria(Categoria categoria);

}
