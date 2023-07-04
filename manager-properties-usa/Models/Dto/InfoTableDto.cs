namespace manager_properties_usa.Models.Dto
{
#nullable disable
    public partial class InfoTableDto
    {
        public bool SortOrderDesc { get; set; }
        public string CurrentFilter { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}