using EDI.Controllers;
using EDI.Models.ItemDataModels;
using EDI.Models.UserModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace EDI.Repositories
{
    public class Repository
    {
        private readonly ErrorController _errorController;
        public Repository(IConfiguration configuration)
        {
            _errorController = new ErrorController(configuration);
        }


        public async Task<bool> DBExcuteNonQuery<T>(SqlConnection connection, SqlTransaction transaction, string query, T queryModel)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    var properties = typeof(T).GetProperties();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transaction;

                    Parallel.ForEach(properties, property =>
                    {
                        var propName = property.Name;
                        var propValue = property.GetValue(queryModel);
                        var propType = property.PropertyType.Name;
                        if ((propType == typeof(string).Name && propValue != null) || (propType == typeof(int).Name && (int)propValue != 0) || (propType == typeof(bool).Name && (bool)propValue != null) || (propType == typeof(DateTime).Name && (DateTime)propValue != null))
                        {
                            command.Parameters.AddWithValue(propName, propValue);
                        }
                    });
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0 ? true : false;

                }
            }
            catch (Exception ex)
            {
                string controllerName = this.GetType().Name;
                string FuntionName = MethodBase.GetCurrentMethod().Name;
                _errorController.Insert(controllerName, FuntionName, ex.Message, ex.StackTrace!);
                return false;
            }
        }

        public async Task<bool> DBExcuteNonQueryForce<T>(SqlConnection connection, SqlTransaction transaction, string query, T queryModel)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    var properties = typeof(T).GetProperties();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transaction;

                    Parallel.ForEach(properties, property =>
                    {
                        var propName = property.Name;
                        var propValue = property.GetValue(queryModel);
                        var propType = property.PropertyType.Name;
                        if ((propType == typeof(string).Name && propValue != null) || (propType == typeof(int).Name && (int)propValue != 0) || (propType == typeof(bool).Name && (bool)propValue != false) || (propType == typeof(DateTime).Name && (DateTime)propValue != null))
                        {
                            command.Parameters.AddWithValue(propName, propValue);
                        }
                    });
                    command.ExecuteNonQuery();
                    return true;

                }
            }
            catch (Exception ex)
            {
                string controllerName = this.GetType().Name;
                string FuntionName = MethodBase.GetCurrentMethod().Name;
                _errorController.Insert(controllerName, FuntionName, ex.Message, ex.StackTrace!);
                return false;
            }
        }

        public async Task<List<T>> DBExcuteReader<T>(SqlConnection connection, SqlTransaction transaction, string query, object queryModel)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    var queryProperties = queryModel?.GetType()?.GetProperties();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transaction;
                    List<T> results = new List<T>();

                    if (queryProperties != null) 
                    {
                        Parallel.ForEach(queryProperties, property =>
                        {
                            var propName = property.Name;
                            var propValue = property.GetValue(queryModel);
                            var propType = property.PropertyType.Name;
                            if ((propType == typeof(string).Name && propValue != null) || (propType == typeof(int).Name && (int)propValue != 0) || (propType == typeof(bool).Name && (bool)propValue != false) || (propType == typeof(DateTime).Name && (DateTime)propValue != null))
                            {
                                command.Parameters.AddWithValue(propName, propValue);
                            }
                        });
                    }
                    
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                try
                                {
                                    T model = Activator.CreateInstance<T>();
                                    var modelProperties = model.GetType().GetProperties();

                                    await Task.WhenAll(modelProperties.Select(async modelProperty =>
                                    {
                                        int i = Array.IndexOf(modelProperties, modelProperty);
                                        if (modelProperty.PropertyType == typeof(bool))
                                        {
                                            modelProperty.SetValue(model, reader.IsDBNull(i) ? false : Convert.ToBoolean(reader[i]));
                                        }
                                        else
                                        {
                                            modelProperty.SetValue(model, reader.IsDBNull(i) ? "" : reader[i]);
                                        }
                                    }));
                                    results.Add(model);
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            return results;
                        }
                        else
                        {
                            return results;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string controllerName = this.GetType().Name;
                string FuntionName = MethodBase.GetCurrentMethod().Name;
                _errorController.Insert(controllerName, FuntionName, ex.Message, ex.StackTrace!);
                return new List<T>();
            }
        }
    }
}
