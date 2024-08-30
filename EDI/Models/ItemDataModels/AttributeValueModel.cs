using System.ComponentModel.DataAnnotations;

namespace EDI.Models.ItemDataModels
{
    public class AttributeValueModel
    {
        [MaxLength(40)]
        public int AttributeId { get; set; }

        [MaxLength(4)]
        public string? AttributeDesc { get; set; }

        [MaxLength(50)]
        public string? UserId { get; set; }

        public DateTime? TimeStamp { get; set; }

    }
}
