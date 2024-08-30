using System.ComponentModel.DataAnnotations;

namespace EDI.Models.ItemDataModels
{
    public class SearchSuggestionModel
    {
        [MaxLength(10)]
        public string? Code { get; set; }

        [MaxLength(30)]
        public string? Description { get; set; }
    }
}
