using System.ComponentModel.DataAnnotations;

namespace EDI.Models.ItemDataModels
{
    public class AttributeSetupModel
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(3)]
        public string LanguageCode { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string SFCCName { get; set; }

        public int Position { get; set; }
    }
}
