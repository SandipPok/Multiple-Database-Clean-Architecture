using Domain_Layer.Modal;

namespace CleanArchitecture.Application.Mapper
{
    public sealed class ItemWithDetailsDto
    {
        public Item Item { get; set; }
        public List<ItemDetail> ItemDetails { get; set; }
    }
}
