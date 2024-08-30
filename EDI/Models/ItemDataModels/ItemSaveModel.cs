using System.ComponentModel.DataAnnotations;

namespace EDI.Models.ItemDataModels
{
    public class ItemSaveModel
    {
        [MaxLength(4)]
        public string? AttributeId { get; set; }

        [MaxLength(8000)]
        public string? AttributeValue { get; set; }
    }
}
