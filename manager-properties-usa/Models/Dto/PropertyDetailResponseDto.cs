#nullable disable

namespace manager_properties_usa.Models.Dto
{
    public partial class PropertyDetailResponseDto : InfoTableDto
    {
        public int? TotalPages { get; set; }
        public int? TotalRecords { get; set; }
        public virtual ICollection<PropertyFilterDto> Properties { get; set; }

    }
}