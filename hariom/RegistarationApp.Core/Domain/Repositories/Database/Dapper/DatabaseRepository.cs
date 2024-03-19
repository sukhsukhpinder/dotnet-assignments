using Dapper;
using Microsoft.Data.SqlClient;
using RegistarationApp.Core.Domain.Entities;
using RegistarationApp.Core.Models.Setting;
using RegistartionApp.Core.Domain.Repositories;
using System.Data;

namespace RegistarationApp.Core.Domain.Repositories.Database.Dapper
{
    /// <summary>
    /// Database Repository for the Dapper DAL
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DatabaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Set Connetcion string
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Used to store T type table name
        /// </summary>
        private readonly string _tableName;

        public DatabaseRepository(RepositorySettings repositorySettings)
        {
            _connectionString = repositorySettings?.DatabaseSettings?.ConnectionString??string.Empty;
            _tableName = $"{typeof(T).Name}s";
        }
        /// <summary>
        /// Get T type entity record by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T?> GetById(string id)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var query = $"SELECT * FROM {_tableName} WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<T>(query, new { Id = id });
        }

        /// <summary>
        /// Get Record for T Type Entity
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>?> GetAll()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var query = $"SELECT * FROM {_tableName}";
            return await connection.QueryAsync<T>(query);
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
            await connection.ExecuteAsync(query, entity);
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
            await connection.ExecuteAsync(query, entity);
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
            await connection.ExecuteAsync(query, new { Id = id });
            return true;
        }
    }
}
