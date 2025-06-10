using Devart.Data.MySql;
using ProdLogApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace ProdLogApp.Services
{
    public interface IDatabaseService
    {
        MySqlConnection GetConnection();
        bool TestConnection();

        bool SavePartProductions(List<Production> productions, int userId);
        List<Production> GetDailyProductions();

        Task<List<Categoria>> CategoriesGet(bool soloActivas = false);
        

        void AgregarProductoEnDB(Producto producto);

        List<Producto> ObtenerTodosLosProductos();

        void ModificarProductoEnDB(Producto producto);
        void ToggleProductState(int productoId, bool estado);


    }



}

