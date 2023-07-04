#nullable disable
namespace manager_properties_usa.Models.Dto
{
    public partial class PropertyModifyDto : PropertyDto
    {
        public int Id { get; set; }
        public decimal? Price { get; set; }
        public int? CodeInternal { get; set; }
        public decimal? Year { get; set; }
        public int? IdOwner { get; set; }
    }
}