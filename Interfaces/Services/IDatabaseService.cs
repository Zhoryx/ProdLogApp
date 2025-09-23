using MySqlConnector;
using ProdLogApp.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProdLogApp.Interfaces
{
    public interface IDatabaseService
    {
        MySqlConnection GetConnection();
        bool TestConnection();


        bool SavePartProductions(List<Production> productions, int userId);
        

        // Lecturas
        List<Production> GetDailyProductions();
        Task<List<Production>> GetDailyProductionsAsync(CancellationToken ct = default);

        // Alta única (lo que te falta y usa el presenter)
        Task<int> ConfirmarProduccionAsync(Production production, int userId, CancellationToken ct = default);

        // Product
        void AgregarProductoEnDB(Producto producto);
        List<Producto> ObtenerTodosLosProductos();
        void ModificarProductoEnDB(Producto producto);
        void ToggleProductState(int productoId, bool estado);

        // Categories
        void ToggleCategoryStatus(int CategoryId, bool estado);
        Task AgregarCategoria(Categoria categoria);
        Task ActualizarCategoria(Categoria categoria);
        Task<List<Categoria>> CategoriesGet(bool soloActivas = false);

        // Puestos
        void TogglePositionState(int puestoId, bool estado);
        void AgregarPuesto(Position puesto);
        void ActualizarPuesto(Position puesto);
        List<Position> ObtenerTodosLosPuestos();
        List<Position> ObtenerPuestosActivos();
        List<Position> ObtenerPuestos();

        // Users
        Task<List<User>> UsersGet(bool soloActivos = false);
        Task ToggleUserStatusAsync(int userId, bool currentActive);
        Task AddUser(User user);
        Task UpdateUser(User user);

        // Productions 
        Task<List<Production>> GetProductionsAsync(int operarioId, DateTime day);
        Task<int> EnsureParteAsync(int usuarioId, DateTime fecha);
        Task<int> InsertProduccionAsync(Production produccion);
        Task<Production?> GetProductionByIdAsync(int productionId);
        Task UpdateProduccionAsync(Production produccion);
        Task<Producto?> GetProductoByIdAsync(int productoId);
        Task<Position?> GetPuestoByIdAsync(int puestoId);
        Task DeleteProduccionAsync(int produccionId);
        Task<bool> SavePartProductionsAsync(List<Production> productions, int userId, DateTime fecha, CancellationToken ct = default);
        Task<Page<ParteHeaderItem>> GetParteHeadersPageAsync(
            DateTime from, DateTime to, int page, int pageSize, string operarioLike = null);

    }
}
