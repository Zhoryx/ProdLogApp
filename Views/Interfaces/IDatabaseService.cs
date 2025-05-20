using Devart.Data.MySql;
using ProdLogApp.Models;

namespace ProdLogApp.Services
{
    public interface IDatabaseService
    {
        MySqlConnection GetConnection();
        bool TestConnection();

        public bool SavePartProductions(List<Production> productions,int userId);

        public List<Production> GetDailyProductions();
    }
}
