using Devart.Data.MySql;
using ProdLogApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProdLogApp.Services
{
    public interface IDatabaseService
    {
        MySqlConnection GetConnection();
        bool TestConnection();

        bool SavePartProductions(List<Production> productions, int userId);
        List<Production> GetDailyProductions();

        // Métodos adicionales para productos y categorías
        Task<List<Categoria>> ObtenerCategoriasDesdeDBAsync();
        void AgregarProductoEnDB(Producto producto);
    }
}
