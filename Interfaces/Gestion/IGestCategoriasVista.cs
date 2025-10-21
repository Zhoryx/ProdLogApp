using ProdLogApp.Models;

public interface IGestCategoriasVista
{
    event Action OnAgregar;
    event Action OnModificar;
    event Action OnAlternarEstado;
    event Action OnEliminar;
    event Action OnVolverMenu;

    void MostrarCategorias(List<Categoria> categorias);
    Categoria ObtenerCategoriaSeleccionada();

    void AbrirVentanaAgregarCategoria();
    void AbrirVentanaModificarCategoria(Categoria categoria);

    void MostrarMensaje(string mensaje);

    // ← agregá esto si faltara
    void NavegarAMenu();
}
