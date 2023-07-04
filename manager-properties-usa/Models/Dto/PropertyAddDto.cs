#nullable disable
namespace manager_properties_usa.Models.Dto
{
    public partial class PropertyAddDto : PropertyDto
    {
        public decimal Price { get; set; }
        public int CodeInternal { get; set; }
        public decimal Year { get; set; }
        public int IdOwner { get; set; }
        public virtual ICollection<PropertyImageDto> Images { get; set; }
    }
}