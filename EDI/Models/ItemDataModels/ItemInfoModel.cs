using System.ComponentModel.DataAnnotations;

namespace EDI.Models.ItemDataModels
{
    public class ItemInfoModel
    {
        [Key]
        public string? ItemNo { get; set; }
        public string? ItemFamily { get; set; }
        public string? ItemSeason { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? ItemCategory { get; set; }
        public string? ItemDivision { get; set; }
        public string? CountryOrigin { get; set; }
        public string? Vendor { get; set; }
        public string? ProductCategory { get; set; }
        public string? ProductSubGroup { get; set; }
        public string? ProductUnit { get; set; }
        public string? ProductGroup { get; set; }
        public string? Barcode { get; set; }
        public string? Description { get; set; }
        public string? CreatedBy { get; set; }
        public string? StatusGl { get; set; }
        public string? StatusHdm { get; set; }
        public bool IsActive { get; set; }
        public string? ApprovedBy { get; set; }

    }
}
