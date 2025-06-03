using System.Collections.Generic;
using ProdLogApp.Models;

namespace ProdLogApp.Views.Designs.Prompts
{
    public interface IPromptCategoryView
    {
        void MostrarCategorias(List<Categoria> categorias); // Muestra la lista de categorías
        void ObtenerSeleccion(out int categoriaId, out string descripcion); // Devuelve la selección
    }
}
