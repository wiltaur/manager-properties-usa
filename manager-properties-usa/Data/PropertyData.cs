using manager_properties_usa.Data.interfaces;
using manager_properties_usa.Models.Context;
using manager_properties_usa.Models.Dto;
using manager_properties_usa.Models.Model;
using Microsoft.EntityFrameworkCore;

namespace manager_properties_usa.Data
{
    public class PropertyData : IPropertyData
    {
        private readonly RealEstatePropertyContext _context;
        private readonly IConfiguration _config;
        public PropertyData(RealEstatePropertyContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        /// <summary>
        /// Public method to add a property and its images to the database.
        /// </summary>
        /// <param name="property">Dto that content all information for Property incluning the images list.</param>
        /// <returns>When added successfully, true is returned, otherwise false is returned.</returns>
        public async Task<bool> AddPropertyBuilding(PropertyAddDto property)
        {
            Property newProperty = MapInfoProperty(property);
            _context.Properties.Add(newProperty);
            if (property.Images.Any())
            {
                var lstPImages = MapInfoPropertyImage(property, newProperty);
                _context.PropertyImages.AddRange(lstPImages);
            }
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Public method to add a list of images from property with id to the database.
        /// </summary>
        /// <param name="propertyImages">Dto that content all information for images of Property incluning the property id.</param>
        /// <returns>When added successfully, true is returned, otherwise false is returned.</returns>
        public async Task<bool> AddImageFromProperty(PropertyImagesIdDto propertyImages)
        {
            var lstPImages = MapInfoPropertyIdImage(propertyImages);
            _context.PropertyImages.AddRange(lstPImages);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Method to change the price of Property in the database.
        /// </summary>
        /// <param name="propertyPrice">Dto that content id and new price of one Property.</param>
        /// <returns>When upadated successfully, true is returned, otherwise false are returned.</returns>
        public async Task<bool> UpdatePropertyPrice(PropertyPriceDto propertyPrice)
        {
            var existingProperty = (from prop in _context.Properties
                                    where prop.IdProperty == propertyPrice.Id
                                    select prop).FirstOrDefault();

            if (existingProperty != null)
            {
                existingProperty.Price = propertyPrice.Price;
                _context.Properties.Update(existingProperty);
                return await _context.SaveChangesAsync() > 0;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Method to update info of Property in the database.
        /// </summary>
        /// <param name="property">Dto that content id and columns to update of one Property.</param>
        /// <returns>When upadated successfully, true is returned, otherwise false are returned.</returns>
        public async Task<bool> UpdateProperty(PropertyModifyDto property)
        {
            var existingProperty = (from prop in _context.Properties
                                    where prop.IdProperty == property.Id
                                    select prop).FirstOrDefault();

            if (existingProperty != null)
            {
                existingProperty.Address = !string.IsNullOrEmpty(property.Address) ? property.Address : existingProperty.Address;
                existingProperty.Name = !string.IsNullOrEmpty(property.Name) ? property.Name : existingProperty.Name;
                existingProperty.Year = property.Year.HasValue ? property.Year.Value : existingProperty.Year;
                existingProperty.Price = property.Price.HasValue ? property.Price.Value : existingProperty.Price;
                existingProperty.CodeInternal = property.CodeInternal.HasValue ? property.CodeInternal.Value : existingProperty.CodeInternal;
                existingProperty.IdOwner = property.IdOwner.HasValue ? property.IdOwner.Value : existingProperty.IdOwner;
                _context.Properties.Update(existingProperty);
                return await _context.SaveChangesAsync() > 0;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// This is a method that receives a search filter and is matched against the property name, address, and owner's name. 
        /// It also receives the order to respond information and the current page and number of items per page.
        /// </summary>
        /// <param name="propertyRequest"></param>
        /// <returns>List of Properties with Owner information.</returns>
        public async Task<PropertyDetailResponseDto> GetProperties(PropertyDetailRequestDto propertyRequest)
        {
            PropertyDetailResponseDto propertiesResponse = new();
            propertiesResponse.SortOrderDesc = propertyRequest.SortOrderDesc;

            propertyRequest.SearchString ??= propertyRequest.CurrentFilter;
            propertiesResponse.CurrentFilter = propertyRequest.SearchString;

            int pageNumber = propertyRequest.PageNumber ?? 1;
            int pageZise = propertyRequest.PageSize ?? Convert.ToInt32(_config.GetSection("DefaultParams").GetSection("PageSize").Value);

            IQueryable<PropertyFilterDto> properties;

            properties = (from prop in _context.Properties.Include(c => c.IdOwnerNavigation)
                          orderby
                            !propertiesResponse.SortOrderDesc ? prop.Name : "",
                            !propertiesResponse.SortOrderDesc ? "" : prop.Name descending
                          where !string.IsNullOrEmpty(propertyRequest.SearchString) ? (prop.Name.Contains(propertyRequest.SearchString)
                           || prop.IdOwnerNavigation.Name.Contains(propertyRequest.SearchString)
                           || prop.Address.Contains(propertyRequest.SearchString)) : 0 == 0
                          select new PropertyFilterDto
                          {
                              Id = prop.IdProperty,
                              Name = prop.Name,
                              Address = prop.Address,
                              Price = prop.Price,
                              CodeInternal = prop.CodeInternal,
                              Year = prop.Year,
                              IdOwner = prop.IdOwnerNavigation.IdOwner,
                              NameOwner = prop.IdOwnerNavigation.Name
                          })
                        .Skip((pageNumber - 1) * pageZise)
                        .Take(pageZise);

            propertiesResponse.TotalRecords = !string.IsNullOrEmpty(propertyRequest.SearchString) ? await _context.Properties.Include(c => c.IdOwnerNavigation).Where(p => p.Name.Contains(propertyRequest.SearchString)
                                   || p.IdOwnerNavigation.Name.Contains(propertyRequest.SearchString)
                                   || p.Address.Contains(propertyRequest.SearchString)).CountAsync() : await _context.Properties.CountAsync();
            
            var totalPages = (double)propertiesResponse.TotalRecords / pageZise;
            propertiesResponse.TotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            propertiesResponse.PageNumber = pageNumber;
            propertiesResponse.PageSize = pageZise;

            propertiesResponse.Properties = await properties.AsNoTracking().ToListAsync();
            return propertiesResponse;
        }

        /// <summary>
        /// Private method to map Property information.
        /// </summary>
        /// <param name="property">Dto with Property information.</param>
        /// <returns>Property mapped.</returns>
        private static Property MapInfoProperty(PropertyAddDto property)
        {
            Property propertyInfo = new()
            {
                Name = property.Name,
                Address = property.Address,
                Price = property.Price,
                CodeInternal = property.CodeInternal,
                Year = property.Year,
                IdOwner = property.IdOwner
            };
            return propertyInfo;
        }

        /// <summary>
        /// Private method to map all images of the property.
        /// </summary>
        /// <param name="property">Dto with Property information.</param>
        /// <param name="newProperty">Property to add to the dababase.</param>
        /// <returns>List of images mapped.</returns>
        private static List<PropertyImage> MapInfoPropertyImage(PropertyAddDto property, Property newProperty)
        {
            List<PropertyImage> lstPImages = new();
            foreach (var image in property.Images)
            {
                PropertyImage propertyImage = new()
                {
                    IdPropertyNavigation = newProperty,
                    File = Convert.FromBase64String(image.File),
                    Enabled = image.Enabled
                };
                lstPImages.Add(propertyImage);
            }
            return lstPImages;
        }

        /// <summary>
        /// Private method to map all images of one property with id.
        /// </summary>
        /// <param name="propertyImages">Dto with Property id and images information.</param>
        /// <returns>List of images with property id mapped.</returns>
        private static List<PropertyImage> MapInfoPropertyIdImage(PropertyImagesIdDto propertyImages)
        {
            List<PropertyImage> lstPImages = new();
            foreach (var image in propertyImages.Images)
            {
                PropertyImage propertyImage = new()
                {
                    IdProperty = propertyImages.Id,
                    File = Convert.FromBase64String(image.File),
                    Enabled = image.Enabled
                };
                lstPImages.Add(propertyImage);
            }
            return lstPImages;
        }
    }
}