using EDI.Controllers;
using EDI.Middlewares;
using EDI.Models.ItemDataModels;
using EDI.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Xml.Linq;

namespace EDI.Utilities.ItemDataUtilities
{
    public class Algorithms
    {
        private readonly ErrorController _errorController;

        public Algorithms(IConfiguration configuration)
        {
            _errorController = new ErrorController(configuration);
        }
        public  async Task<List<SearchSuggestionModel>>? CheckSuggestions(SqlConnection connection, List<SearchSuggestionModel> data, string inputText)
        {
            try
            {
                List<SearchSuggestionModel>? listSuggestions = new List<SearchSuggestionModel>();
                if (data != null && !inputText.IsNullOrEmpty())
                {
                    foreach (SearchSuggestionModel suggestion in data)
                    {
                        if (suggestion.Description.ToLower().Contains(inputText.ToLower()))
                        {
                            listSuggestions.Add(suggestion);
                        }
                    }
                    if (listSuggestions != null)
                    {
                        return listSuggestions;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string controllerName = this.GetType().Name;
                string FuntionName = MethodBase.GetCurrentMethod().Name;
                _errorController.Insert(controllerName, FuntionName, ex.Message, ex.StackTrace!);
                return null!;
            }
        }
    }
}
