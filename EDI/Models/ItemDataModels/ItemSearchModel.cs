namespace EDI.Models.ItemDataModels
{
    public class ItemSearchModel
    {
        public string ItemNo { get; set; }
        public string SeasonCode { get; set; }
        public DateTime DateBefore{ get; set; }
        public DateTime DateAfter { get; set; }
        public string ItemFamily { get; set; }
        public string ItemCategory { get; set; }
        public int IsApproved {  get; set; }

    }
}
