using Microsoft.Data.SqlClient;

namespace DataAcessLayer.Common
{
    public interface IDBCommand
    {
        Task<List<T>> GetDataAsync<T>(string query) where T : new();
        Task<List<T>> GetDataWithConditionsAsync<T>(string query, List<SqlParameter> parameters) where T : new();
        Task<bool> InsertUpdateDataAsync(string query, List<SqlParameter> parameters);
        Task<bool> IsRowExistsAsync(string query, List<SqlParameter> parameters);
        Task<bool> UpdateDataNoConditionsAsync(string query);
    }
}
