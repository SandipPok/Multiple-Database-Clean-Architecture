using Domain_Layer.Modal;

namespace CleanArchitecture.Application.Mapper
{
    public sealed class ItemWithDetailsDto
    {
        public ItemDto Item { get; set; }
        public List<ItemDetailDto> ItemDetails { get; set; }
    }
}
