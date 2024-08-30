using EDI.Models.ItemDataModels;
using EDI.Repositories;
using EDI.Services.DatabaseServices;
using EDI.Utilities.ItemDataUtilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace EDI.Controllers
{
    [Authorize(Roles = "IT_SoftwareDeveloper, Content_Manager, Content_Operator")]
    public class ItemDataController : Controller
    {
        private readonly DatabaseConnection _connection;
        private readonly Algorithms _algorithms;
        private readonly ImageCopy _imageCopy;
        private readonly IpAddress _ipAddress;
        private readonly ErrorController _errorController;
        private readonly Repository _repository;
        private readonly IConfiguration _configuration;

        public ItemDataController(IConfiguration configuration)
        {
            _connection = new DatabaseConnection(configuration);
            _algorithms = new Algorithms(configuration);
            _imageCopy = new ImageCopy(configuration);
            _ipAddress = new IpAddress();
            _errorController = new ErrorController(configuration);
            _repository = new Repository(configuration);
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Main()
        {
            try
            {
                ViewData["Title"] = "Item Data EDI";
                List<List<AttributeSetupModel>> attributes = new List<List<AttributeSetupModel>>();
                List<AttributeSetupModel> primaryAttributes = new List<AttributeSetupModel>();
                List<AttributeSetupModel> secondaryAttributes = new List<AttributeSetupModel>();
                List<SearchSuggestionModel> itemFamilySuggestions = new List<SearchSuggestionModel>();
                List<SearchSuggestionModel> seasonSuggestions = new List<SearchSuggestionModel>();
                List<SearchSuggestionModel> itemCategorySuggestions = new List<SearchSuggestionModel>();
                List<SearchSuggestionModel> divisionSuggestions = new List<SearchSuggestionModel>();

                using (SqlConnection connection = await _connection.OpenConnectionAsync())
                {
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        string query = _configuration?.GetSection("StoredProcedures:EDIA_AttributeSetup_SeachAll").Value;
                        var task1 = _repository.DBExcuteReader<AttributeSetupModel>(connection, transaction,query, new { LanguageCode = "en" });
                        var task2 = _repository.DBExcuteReader<AttributeSetupModel>(connection, transaction, query, new { LanguageCode = "ar" });
                        query = _configuration?.GetSection("StoredProcedures:EDIA_SearchSuggestion_SearchAll").Value;
                        var task3 = _repository.DBExcuteReader<SearchSuggestionModel>(connection, transaction, query, new { TableName = "HIDS$Item Family" });
                        var task4 = _repository.DBExcuteReader<SearchSuggestionModel>(connection, transaction, query, new { TableName = "HIDS$Season" });
                        var task5 = _repository.DBExcuteReader<SearchSuggestionModel>(connection, transaction, query, new { TableName = "HIDS$Item Category" });

                        await Task.WhenAll(task1!, task2!, task3!, task4!, task5!);
                        Constants.ITEM_FAMILY_SUGGESTIONS = task3!.Result;
                        Constants.SEASON_SUGGESTIONS = task4!.Result;
                        Constants.ITEM_CATEGORY_SUGGESTIONS = task5!.Result;
                        attributes.Add(task1!.Result);
                        attributes.Add(task2!.Result);
                    }     
                }
       
                return View("Main", attributes);
            }
            catch (Exception ex) 
            {
                string controllerName = ControllerContext.ActionDescriptor.ControllerName;
                string FuntionName = ControllerContext.ActionDescriptor.DisplayName!;
                _errorController.Insert(controllerName, FuntionName, ex.Message, ex.StackTrace!);
                return View("Main");
            }
        }

        [HttpGet]
        public async Task<IActionResult>? GetSuggestions(string field, string inputText)
        {
            try
            {
                List<SearchSuggestionModel>? listSuggestions = new List<SearchSuggestionModel>();
                using (SqlConnection connection = await _connection.OpenConnectionAsync())
                {          
                    switch (field)
                    {
                        case "f-item-family":
                            listSuggestions = await _algorithms.CheckSuggestions(connection, Constants.ITEM_FAMILY_SUGGESTIONS!, inputText)!;
                            break;
                        case "f-season":
                            listSuggestions = await _algorithms.CheckSuggestions(connection,Constants.SEASON_SUGGESTIONS!, inputText)!;
                            break;
                        case "f-item-category":
                            listSuggestions = await _algorithms.CheckSuggestions(connection,Constants.ITEM_CATEGORY_SUGGESTIONS!, inputText)!;
                            break;
                    }
                }
                var data = new
                {
                    Suggestions = listSuggestions
                };
                return Json(data);
            }
            catch(Exception ex) 
            {
                string controllerName = ControllerContext.ActionDescriptor.ControllerName;
                string FuntionName = ControllerContext.ActionDescriptor.DisplayName!;
                _errorController.Insert(controllerName, FuntionName, ex.Message, ex.StackTrace!);
                return Json(null);
            }
        }

        [HttpGet]
        public async Task<IActionResult>? SearchOne(int itemNo)
        {
            try 
            {
                //string imagePath = null;
                if ((itemNo > Constants.SEARCHED_LENGTH) || (itemNo < 1))
                {  
                    return Json(null);
                }
                else
                {
                    string item_No = Constants.SEARCHED_ITEMS![itemNo - 1].ItemNo;
                    List<List<AttributeValueModel>> attributeValueList = new List<List<AttributeValueModel>>();
                    using (SqlConnection connection = await _connection.OpenConnectionAsync())
                    {
                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            string query = _configuration?.GetSection("StoredProcedures:EDIA_ItemInfo_SearchOne").Value;
                            var task1 =  _repository.DBExcuteReader<ItemInfoModel>(connection, transaction, query, new { itemNo = item_No});
                            var task2 = _imageCopy.CopyImage(connection, item_No);
                            query = _configuration?.GetSection("StoredProcedures:EDIA_AttributeValues_SearchOne").Value;
                            var task3 =  _repository.DBExcuteReader<AttributeValueModel>(connection, transaction, query, new { itemNo = item_No , LanguageCode = "en", AttributeId = "%"});
                            var task4 = _repository.DBExcuteReader<AttributeValueModel>(connection, transaction, query, new { itemNo = item_No, LanguageCode = "ar", AttributeId = "%" });
                            query = _configuration?.GetSection("StoredProcedures:ERP_EDI_Product_DataView").Value;
                            var task5 = _repository.DBExcuteReader<ItemSupModel>(connection, transaction, query, new { itemNo = item_No });
                            await Task.WhenAll(task1!, task2!, task3, task4, task5);
                            attributeValueList.Add(task3.Result);
                            attributeValueList.Add(task4.Result);
                            var data = new
                            {
                                ItemInfo = task1!.Result,
                                ImagePath = task2!.Result,
                                AttributeValueList = attributeValueList,
                                ItemSup = task5!.Result
                            };
                            Constants.LOCKED_ITEMS[HttpContext?.Session?.GetString("UserEmail")] = item_No;
                            return Json(data);
                        }  
                    }      
                }
            }
            catch(Exception ex) 
            {

                string controllerName = ControllerContext.ActionDescriptor.ControllerName;
                string FuntionName = ControllerContext.ActionDescriptor.DisplayName!;
                _errorController.Insert(controllerName, FuntionName, ex.Message, ex.StackTrace!);
                return Json(null);
            }         
        }

        [HttpGet]
        public async Task<IActionResult> SearchAll(string itemNo, string season, string dateAfter, string dateBefore, string itemFamily, string itemCategory, string isApproved)
        {
            try
            {
                itemNo = itemNo.Trim();
                int searchedLength;
                if (itemNo == "%" && season == "%" && dateAfter.IsNullOrEmpty() && dateBefore.IsNullOrEmpty()  && itemFamily == "%" && itemCategory == "%")
                {
                    searchedLength = 0;
                    Constants.SEARCHED_LENGTH = 0;
                    return Json(null);
                }
                else
                {
                    using (SqlConnection connection = await _connection.OpenConnectionAsync())
                    {
                        using (SqlTransaction transaction = connection.BeginTransaction()) 
                        {
                            ItemSearchModel itemSearch = new ItemSearchModel();
                            itemSearch.ItemNo = itemNo;
                            itemSearch.SeasonCode = season;
                            dateAfter = dateAfter.IsNullOrEmpty() ? "9999-01-01" : dateAfter;
                            dateBefore = dateBefore.IsNullOrEmpty() ? "9999-01-01" : dateBefore;
                            itemSearch.DateAfter = DateTime.Parse(dateAfter);
                            itemSearch.DateBefore = DateTime.Parse(dateBefore);
                            itemSearch.ItemFamily = itemFamily;
                            itemSearch.ItemCategory = itemCategory;
                            itemSearch.IsApproved = int.Parse(isApproved);

                            string query = _configuration?.GetSection("StoredProcedures:EDIA_ItemInfo_SearchAll").Value;
                            var searchItems = await _repository.DBExcuteReader <ItemListModel>(connection, transaction, query, itemSearch);

                            if (!searchItems.IsNullOrEmpty())
                            {
                                Constants.SEARCHED_ITEMS = searchItems;
                                Constants.SEARCHED_LENGTH = searchItems.Count;
                                searchedLength = searchItems.Count;
                                string firstItemNo = searchItems[0].ItemNo;
                                List<List<AttributeValueModel>> attributeValueList = new List<List<AttributeValueModel>>();
                                query = _configuration?.GetSection("StoredProcedures:EDIA_ItemInfo_SearchOne").Value;
                                var task1 = _repository.DBExcuteReader<ItemInfoModel>(connection, transaction, query, new { itemNo = firstItemNo });
                                var task2 = _imageCopy.CopyImage(connection, firstItemNo);
                                query = _configuration?.GetSection("StoredProcedures:EDIA_AttributeValues_SearchOne").Value;
                                var task3 = _repository.DBExcuteReader<AttributeValueModel>(connection, transaction, query, new { itemNo = firstItemNo, LanguageCode = "en", AttributeId = "%" });
                                var task4 = _repository.DBExcuteReader<AttributeValueModel>(connection, transaction, query, new { itemNo = firstItemNo, LanguageCode = "ar", AttributeId = "%" });
                                query = _configuration?.GetSection("StoredProcedures:ERP_EDI_Product_DataView").Value;
                                var task5 = _repository.DBExcuteReader<ItemSupModel>(connection, transaction, query, new { itemNo = firstItemNo});
                                await Task.WhenAll(task1!, task2!, task3, task4, task5);
                                attributeValueList.Add(task3.Result);
                                attributeValueList.Add(task4.Result);
                                var data = new
                                {
                                    SearchedLength = searchedLength,
                                    ItemInfo = task1!.Result,
                                    ImagePath = task2!.Result,
                                    AttributeValueList = attributeValueList,
                                    ItemSup = task5!.Result
                                };
                                Constants.LOCKED_ITEMS[HttpContext?.Session?.GetString("UserEmail")] = firstItemNo;
                                return Json(data);
                            }
                            else
                            {
                                searchedLength = 0;
                                Constants.SEARCHED_LENGTH = 0;
                                var data = new
                                {
                                    SearchedLength = searchedLength,
                                };
                                return Json(data);
                            }
                        }  
                    }
                }                
            }
            catch (Exception ex) 
            {
                string controllerName = ControllerContext.ActionDescriptor.ControllerName;
                string FuntionName = ControllerContext.ActionDescriptor.DisplayName!;
                _errorController.Insert(controllerName, FuntionName, ex.Message, ex.StackTrace!);
                return Json(null);
            }           
        }

        [HttpPost]
        public async Task<IActionResult>? Save(string itemNo, List<ItemSaveModel> attributeList) 
        {
            try
            {
                if (itemNo.IsNullOrEmpty() || attributeList.IsNullOrEmpty())
                {
                    return Json(null);
                }
                else
                { 
                    using (SqlConnection connection = await _connection.OpenConnectionAsync())
                    {
                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            string ip = _ipAddress.Get();
                            string userId = HttpContext?.Session.GetString("UserEmail")!;
                            List<Task<bool>> updateTasks = new List<Task<bool>>();
                            List<Action> changeLogs = new List<Action>();
                            string query1 = _configuration?.GetSection("StoredProcedures:EDIA_AttributeValues_SearchOne").Value;
                            string query2 = _configuration?.GetSection("StoredProcedures:EDIA_AttributeValues_Update").Value;

                            Parallel.ForEach(attributeList, async item => 
                            {
                                string[] parts = item.AttributeId!.Split("_");
                                string[] subParts = parts[0].Split("-");
                                string languageCode = subParts[0];
                                string attributeId = subParts[1];
                                string attributeValue = item.AttributeValue == null ? "" : item.AttributeValue.Trim();
                                List<AttributeValueModel> oldValues = await _repository.DBExcuteReader<AttributeValueModel>(connection, transaction, query1, new { itemNo = itemNo, LanguageCode = languageCode, AttributeId = attributeId });
                                string oldValue = (oldValues.IsNullOrEmpty()) ? "" : oldValues[0].AttributeDesc!;
                                Task<bool> updateTask = _repository.DBExcuteNonQueryForce(connection, transaction, query2, new { ItemNo = itemNo, AttributeId = attributeId, AttributeValue = attributeValue, IpAddress = ip, UserID = userId, TableName = "EDI.dbo.EDI_AttributeValues", OldValue = oldValue });
                                updateTasks.Add(updateTask);

                            });

                            await Task.WhenAll(updateTasks);
                            bool hasFalseItem = updateTasks.Any(task => task.Result == false);

                            if (!hasFalseItem)
                            {
                                string userFirstName = HttpContext?.Session.GetString("UserFirstName")!;
                                string userLastName = HttpContext?.Session.GetString("UserLastName")!;
                                string query = _configuration?.GetSection("StoredProcedures:EDIA_ApprovedItems_Update").Value;
                                bool status = false;
                                bool response = await _repository.DBExcuteNonQuery(connection, transaction, query, new { ItemNo = itemNo, ApprovedBy = userId, Status = status });
                                var data = new
                                {
                                    ApprovedBy = $"{userFirstName} {userLastName}"
                                };
                                if (response)
                                {
                                    transaction.Commit();
                                    return Json(data);
                                }
                                else 
                                {
                                    transaction.Rollback();
                                    return Json(null);
                                }
                            }
                            else    
                            {
                                transaction.Rollback();
                                return Json(null);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                string controllerName = ControllerContext.ActionDescriptor.ControllerName;
                string FuntionName = ControllerContext.ActionDescriptor.DisplayName!;
                _errorController.Insert(controllerName, FuntionName, ex.Message, ex.StackTrace!);
                return Json(null);
            }
        }

        [HttpPost]
        [Authorize(Policy = "CanApproveContent")]
        public async Task<IActionResult>? Approve(string itemNo, bool status)
        {
            try
            {
                if (itemNo.IsNullOrEmpty()) 
                { 
                    return null!; 
                }
                else
                {
                    using (SqlConnection connection = await _connection.OpenConnectionAsync())
                    {
                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                           
                            string userId = HttpContext?.Session.GetString("UserEmail")!;
                            string userFirstName = HttpContext?.Session.GetString("UserFirstName")!;
                            string userLastName = HttpContext?.Session.GetString("UserLastName")!;
                            string query = _configuration?.GetSection("StoredProcedures:EDIA_ApprovedItems_Update").Value;
                            bool response = await _repository.DBExcuteNonQuery(connection, transaction, query, new {ItemNo = itemNo, ApprovedBy= userId , Status = status });
                            transaction.Commit();
                            var data = new
                            {
                                ApprovedBy = $"{userFirstName} {userLastName}"
                            };
                            return Json(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string controllerName = ControllerContext.ActionDescriptor.ControllerName;
                string FuntionName = ControllerContext.ActionDescriptor.DisplayName!;
                _errorController.Insert(controllerName, FuntionName, ex.Message, ex.StackTrace!);
                return Json(null);
            }
        }
    }
}
