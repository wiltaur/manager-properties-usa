using manager_properties_usa.Models.Dto;

namespace manager_properties_usa.Data.interfaces
{
    public interface IPropertyData
    {
        Task<bool> AddPropertyBuilding(PropertyAddDto property);
        Task<bool> AddImageFromProperty(PropertyImagesIdDto propertyImages);
        Task<bool> UpdatePropertyPrice(PropertyPriceDto propertyPrice);
        Task<bool> UpdateProperty(PropertyModifyDto property);
        Task<PropertyDetailResponseDto> GetProperties(PropertyDetailRequestDto propertyRequest);
    }
}