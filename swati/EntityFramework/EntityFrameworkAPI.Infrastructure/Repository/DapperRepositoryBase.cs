using AuthenticationManager.Data;
using Dapper;
using EntityFramework.EntityFrameworkAPI.Core.Common;
using EntityFrameworkAPI.Core.Repository;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace EntityFrameworkAPI.Infrastructure.Repository
{
    public class DapperRepositoryBase<T> : IDapperRepositoryBase<T> where T : EntityBase
    {
        private readonly string _connectionString;
        public DapperRepositoryBase()
        {
            _connectionString = ConfigSettings.ConnectionString;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            IEnumerable<T> result = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    string tableName = GetTableName();
                    string query = $"SELECT * FROM {tableName}";

                    result = await connection.QueryAsync<T>(query);
                }
                catch (Exception ex) { }
            }

            return result.ToList();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string tableName = GetTableName();
                string keyColumn = GetKeyColumnName();
                string query = $"SELECT * FROM {tableName} WHERE {predicate}";
                var result = await connection.QueryAsync<T>(query);

                return result.ToList();
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            IEnumerable<T> result = null;
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    string tableName = GetTableName();
                    string keyColumn = GetKeyColumnName();
                    string keyProperty = GetKeyPropertyName();
                    string query = $"SELECT * FROM {tableName} WHERE StudentId = '{id}'";

                    result = await connection.QueryAsync<T>(query);
                }
            }
            catch (Exception ex) { }

            return result.FirstOrDefault();
        }

        public async Task<bool> AddAsync(T entity)
        {
            int rowsEffected = 0;
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    string tableName = GetTableName();
                    string columns = GetColumns(excludeKey: true);
                    string properties = GetPropertyNames(excludeKey: true);
                    string query = $"INSERT INTO {tableName} ({columns}) VALUES ({properties})";

                    rowsEffected = await connection.ExecuteAsync(query, entity);
                }
            }
            catch (Exception ex) { }

            return rowsEffected > 0 ? true : false;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            int rowsEffected = 0;
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    string tableName = GetTableName();
                    string keyColumn = GetKeyColumnName();
                    string keyProperty = GetKeyPropertyName();

                    StringBuilder query = new StringBuilder();
                    query.Append($"UPDATE {tableName} SET ");

                    foreach (var property in GetProperties(true))
                    {
                        var columnAttr = property.GetCustomAttribute<ColumnAttribute>();

                        string propertyName = property.Name;
                        //string columnName = columnAttr.Name;

                        query.Append($"{propertyName} = @{propertyName},");
                    }

                    query.Remove(query.Length - 1, 1);

                    query.Append($" WHERE {keyProperty} = @{keyProperty}");

                    rowsEffected = await connection.ExecuteAsync(query.ToString(), entity);
                }
            }
            catch (Exception ex) { }

            return rowsEffected > 0 ? true : false;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            int rowsEffected = 0;
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    string tableName = GetTableName();
                    string keyColumn = GetKeyColumnName();
                    string keyProperty = GetKeyPropertyName();
                    string query = $"DELETE FROM {tableName} WHERE {keyProperty} = @{keyProperty}";

                    rowsEffected = await connection.ExecuteAsync(query, entity);
                }
            }
            catch (Exception ex) { }

            return rowsEffected > 0 ? true : false;
        }

        private string GetTableName()
        {
            string tableName = "";
            var type = typeof(T);
            var tableAttr = type.GetCustomAttribute<TableAttribute>();
            if (tableAttr != null)
            {
                tableName = tableAttr.Name;
                return tableName;
            }

            return type.Name + "s";
        }

        private static string GetKeyColumnName()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object[] keyAttributes = property.GetCustomAttributes(typeof(KeyAttribute), true);

                if (keyAttributes != null && keyAttributes.Length > 0)
                {
                    object[] columnAttributes = property.GetCustomAttributes(typeof(ColumnAttribute), true);

                    if (columnAttributes != null && columnAttributes.Length > 0)
                    {
                        ColumnAttribute columnAttribute = (ColumnAttribute)columnAttributes[0];
                        return columnAttribute.Name;
                    }
                    else
                    {
                        return property.Name;
                    }
                }
            }

            return null;
        }

        private string GetColumns(bool excludeKey = false)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

            var values = string.Join(", ", properties.Select(p =>
            {
                return $"{p.Name}";
            }));

            return values;
        }

        private string GetColumns1(bool excludeKey = false)
        {
            var type = typeof(T);
            var columns = string.Join(", ", type.GetProperties()
                .Where(p => !excludeKey || !p.IsDefined(typeof(KeyAttribute)))
                .Select(p =>
                {
                    var columnAttr = p.GetCustomAttribute<ColumnAttribute>();
                    return columnAttr != null ? columnAttr.Name : p.Name;
                }));

            return columns;
        }

        protected string GetPropertyNames(bool excludeKey = false)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

            var values = string.Join(", ", properties.Select(p =>
            {
                return $"@{p.Name}";
            }));

            return values;
        }

        protected IEnumerable<PropertyInfo> GetProperties(bool excludeKey = false)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

            return properties;
        }

        protected string GetKeyPropertyName()
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.GetCustomAttribute<KeyAttribute>() != null);

            if (properties.Any())
            {
                return properties.FirstOrDefault().Name;
            }

            return null;
        }
    }
}
