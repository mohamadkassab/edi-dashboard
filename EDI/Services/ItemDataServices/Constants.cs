using EDI.Models.ItemDataModels;
using System.Collections.Concurrent;

namespace EDI.Utilities.ItemDataUtilities
{
    public static class Constants
    {
        public static List<ItemListModel>? SEARCHED_ITEMS;

        public static int SEARCHED_LENGTH;

        public static List<SearchSuggestionModel>? ITEM_FAMILY_SUGGESTIONS;

        public static List<SearchSuggestionModel>? SEASON_SUGGESTIONS;

        public static List<SearchSuggestionModel>? ITEM_CATEGORY_SUGGESTIONS;

        public static string FDS_PATH = @"\\filesvr\Admic\Pictures\Fds\Jpg\";

        public static string HDM_PATH = @"\\filesvr\Admic\Pictures\Hdm\Jpg\";

        public static string MAIN_PATH = @"\\filesvr\Admic\Pictures\Jpg";

        public static string LOCAL_PATH = @"./wwwroot/resources/images/itemImages/";

        public static string JS_PATH = @"../resources/images/itemImages/";

        public static Dictionary<string, string>? LOCKED_ITEMS = new Dictionary<string, string>();
    }
}
