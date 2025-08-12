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

        // Batch (sync existente + async recomendado)
        bool SavePartProductions(List<Production> productions, int userId);
        Task<bool> SavePartProductionsAsync(List<Production> productions, int userId, CancellationToken ct = default);

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
        Task AddUser(User user);
        Task UpdateUser(User user);

        // Productions (lecturas avanzadas podrían ir acá en el futuro)
        Task<List<Production>> GetProductionsAsync(int operarioId, DateTime day);
        Task<int> EnsureParteAsync(int usuarioId, DateTime fecha);
        Task<int> InsertProduccionAsync(Production produccion);

    }
}
