namespace Domain_Layer.Modal
{
    public class ItemDetail
    {
        public int DetailId { get; set; }
        public int ItemId { get; set; }
        public string DetailDescription { get; set; }
        public Item Item { get; set; }
    }
}
