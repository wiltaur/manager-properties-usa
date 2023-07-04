#nullable disable
namespace manager_properties_usa.Models.Dto
{
    public partial class PropertyImagesIdDto
    {
        public int Id { get; set; }
        public virtual ICollection<PropertyImageDto> Images { get; set; }
    }
}