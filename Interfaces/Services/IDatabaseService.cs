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


        bool SavePartProductions(List<Produccion> productions, int userId);
        

        // Lecturas
        List<Produccion> GetDailyProductions();
        Task<List<Produccion>> GetDailyProductionsAsync(CancellationToken ct = default);

        // Alta única (lo que te falta y usa el presenter)
        Task<int> ConfirmarProduccionAsync(Produccion production, int userId, CancellationToken ct = default);

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
        void AgregarPuesto(Puesto puesto);
        void ActualizarPuesto(Puesto puesto);
        List<Puesto> ObtenerTodosLosPuestos();
        List<Puesto> ObtenerPuestosActivos();
        List<Puesto> ObtenerPuestos();

        // Users
        Task<List<Usuario>> UsersGet(bool soloActivos = false);
        Task ToggleUserStatusAsync(int userId, bool currentActive);
        Task AddUser(Usuario user);
        Task UpdateUser(Usuario user);

        // Productions 
        Task<List<Produccion>> GetProductionsAsync(int operarioId, DateTime day);
        Task<int> EnsureParteAsync(int usuarioId, DateTime fecha);
        Task<int> InsertProduccionAsync(Produccion produccion);
        Task<Produccion?> GetProductionByIdAsync(int productionId);
        Task UpdateProduccionAsync(Produccion produccion);
        Task<Producto?> GetProductoByIdAsync(int productoId);
        Task<Puesto?> GetPuestoByIdAsync(int puestoId);
        Task DeleteProduccionAsync(int produccionId);
        Task<bool> SavePartProductionsAsync(List<Produccion> productions, int userId, DateTime fecha, CancellationToken ct = default);
        Task<Page<ParteHeaderItem>> GetParteHeadersPageAsync(
            DateTime from, DateTime to, int page, int pageSize, string operarioLike = null);

    }
}
