using MySqlConnector;
using ProdLogApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace ProdLogApp.Interfaces
{
    public interface IDatabaseService
    {
        MySqlConnection GetConnection();
        bool TestConnection();

        bool SavePartProductions(List<Production> productions, int userId);
        List<Production> GetDailyProductions();

     
        
        //product
        void AgregarProductoEnDB(Producto producto);
        List<Producto> ObtenerTodosLosProductos();
        void ModificarProductoEnDB(Producto producto);
        void ToggleProductState(int productoId, bool estado);

        //categories
        void ToggleCategoryStatus(int CategoryId, bool estado);
        Task AgregarCategoria(Categoria categoria);
        Task ActualizarCategoria(Categoria categoria); 
        Task<List<Categoria>> CategoriesGet(bool soloActivas = false);

        //Puestos
        void TogglePositionState(int puestoId, bool estado);
        void AgregarPuesto(Position puesto);
        void ActualizarPuesto(Position puesto);
        List<Position> ObtenerTodosLosPuestos();
        List<Position> ObtenerPuestosActivos();




    }



}

