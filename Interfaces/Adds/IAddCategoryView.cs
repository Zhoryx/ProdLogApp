using ProdLogApp.Models;

namespace ProdLogApp.Views.Interfaces
{
    public interface IAddCategoryView
    {
        void CargarDatosCategoria(Categoria categoria); 
        void CerrarVentana();
        void MostrarMensaje(string mensaje);
    }
}
