namespace Domain_Layer.Modal
{
    public class ItemDetailBackup
    {
        public int DetailId { get; set; }
        public int ItemId { get; set; }
        public string DetailDescription { get; set; }

        
        public Item Item { get; set; }
    }
}
