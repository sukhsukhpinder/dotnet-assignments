using Microsoft.Data.SqlClient;
using RegistarationApp.Core.Domain.Entities;
using RegistarationApp.Core.Models.Setting;
using RegistartionApp.Core.Domain.Repositories;
using System.Data;

namespace RegistarationApp.Core.Domain.Repositories.Database.ADO
{
    /// <summary>
    /// Database Repository for the ADO DAL
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DatabaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly string _connectionString;
        private readonly string _tableName;

        public DatabaseRepository(RepositorySettings repositorySettings)
        {
            _connectionString = repositorySettings?.DatabaseSettings?.ConnectionString ?? string.Empty;
            _tableName = typeof(T).Name + "s";
        }

        /// <summary>
        /// Get Record for T Type Entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T?> GetById(string id)
        {
            T? result = null;
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var query = $"SELECT * FROM {_tableName} WHERE Id = @Id";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            using var reader = await command.ExecuteReaderAsync();
            if (reader.Read())
            {
                result = MapToObject(reader);
            }

            return await Task.FromResult(result);
        }

        /// <summary>
        /// Get All Record for T Type Entity
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>?> GetAll()
        {
            var entities = new List<T>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = $"SELECT * FROM {_tableName}";

            using var command = new SqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                entities.Add(MapToObject(reader));
            }
            return entities;
        }

        /// <summary>
        /// Add T Type Entity record
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task Add(T entity)
        {
            var properties = typeof(T).GetProperties().Where(p => p.Name != "Id").ToList();
            var columns = string.Join(", ", properties.Select(p => p.Name));
            var values = string.Join(", ", properties.Select(p => $"@{p.Name}"));

            var query = $"INSERT INTO {_tableName} ({columns}) VALUES ({values})";

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var command = new SqlCommand(query, connection);
            foreach (var property in properties)
            {
                command.Parameters.AddWithValue($"@{property.Name}", property.GetValue(entity));
            }

            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Update T Type Entity record
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<T?> Update(T entity)
        {
            var properties = typeof(T).GetProperties().Where(p => p.Name != "Id").ToList();
            var setStatements = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));
            var query = $"UPDATE {_tableName} SET {setStatements} WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var command = new SqlCommand(query, connection);
            foreach (var property in properties)
            {
                command.Parameters.AddWithValue($"@{property.Name}", property.GetValue(entity));
            }
            command.Parameters.AddWithValue("@Id", typeof(T)?.GetProperty("Id")?.GetValue(entity));

            await command.ExecuteNonQueryAsync();
            return entity;
        }

        /// <summary>
        /// Delete T Type Entity record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(string id)
        {
            var entity = await GetById(id);
            if (entity == null)
                return false;

            var query = $"DELETE FROM {_tableName} WHERE Id = @Id";
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
            return true;
        }

        /// <summary>
        /// Private funtion for map to objects
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        private T MapToObject(IDataRecord record)
        {
            // Create an instance of the entity type
            T entity = Activator.CreateInstance<T>();

            // Loop through the properties of the entity type
            foreach (var property in typeof(T).GetProperties())
            {
                // Check if the column exists in the IDataRecord
                if (record[property.Name] != DBNull.Value)
                {
                    // Set the property value from the IDataRecord
                    property.SetValue(entity, Convert.ChangeType(record[property.Name], property.PropertyType));
                }
            }

            return entity;
        }

    }
}
