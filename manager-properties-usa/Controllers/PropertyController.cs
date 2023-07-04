using manager_properties_usa.Data.interfaces;
using manager_properties_usa.Models.Dto;
using manager_properties_usa.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace manager_properties_usa.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : Controller
    {
        private const string ERROR_PROCESS_INFORMATION = "Error processing the information.";
        private const string SUCCESS_SAVE_INFORMATION = "The information has been successfully saved.";
        private readonly IPropertyData _bus;

        public PropertyController(IPropertyData bus)
        {
            _bus = bus;
        }

        #region MainMethods

        /// <summary>
        /// Add a property and its images.
        /// </summary>
        /// <param name="property">Object that content all information for Property incluning the images list.</param>
        /// <returns>When added successfully, true and Ok are returned, otherwise false and BadRequest are returned.</returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<bool>))]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddPropertyBuilding([FromBody] PropertyAddDto property)
        {
            try
            {
                var result = await _bus.AddPropertyBuilding(property);
                return AnalyzeResult(result);
            }
            catch (Exception ex)
            {
                return BadRequest(BadRequestResult(ex));
            }
        }

        /// <summary>
        /// Add a list of images from property with id.
        /// </summary>
        /// <param name="propertyImages">Object that content all information for images of Property incluning the property id.</param>
        /// <returns>When added successfully, true and Ok are returned, otherwise false and BadRequest are returned.</returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<bool>))]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddImageFromProperty([FromBody] PropertyImagesIdDto propertyImages)
        {
            try
            {
                var result = await _bus.AddImageFromProperty(propertyImages);
                return AnalyzeResult(result);
            }
            catch (Exception ex)
            {
                return BadRequest(BadRequestResult(ex));
            }
        }

        /// <summary>
        /// Change the price of Property.
        /// </summary>
        /// <param name="propertyPrice">Object that content id and new price of Property.</param>
        /// <returns>When updated successfully, true and Ok are returned, otherwise false and BadRequest are returned.</returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<bool>))]
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdatePropertyPrice([FromBody] PropertyPriceDto propertyPrice)
        {
            try
            {
                var result = await _bus.UpdatePropertyPrice(propertyPrice);
                return AnalyzeResult(result);
            }
            catch (Exception ex)
            {
                return BadRequest(BadRequestResult(ex));
            }
        }

        /// <summary>
        /// Update fields of Property.
        /// </summary>
        /// <param name="property">Object that content id and other fields to update of Property.</param>
        /// <returns>When updated successfully, true and Ok are returned, otherwise false and BadRequest are returned.</returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<bool>))]
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateProperty([FromBody] PropertyModifyDto property)
        {
            try
            {
                var result = await _bus.UpdateProperty(property);
                return AnalyzeResult(result);
            }
            catch (Exception ex)
            {
                return BadRequest(BadRequestResult(ex));
            }
        }

        /// <summary>
        /// Search all properties that match the filters.
        /// </summary>
        /// <remarks>
        /// Parameters description:
        /// 
        ///      searchString => Field for the filter that matches the property name, address, and owner name.
        ///      sortOrderDesc => true for sort descending, default is ascending.
        ///      currentFilter => Field for admin the searchFilter when navigated in the pages of the table.
        ///      pageNumber => It is the actual page number of the table.
        ///      pageSize => It is the maximum number of items per page
        /// </remarks>
        /// <param name="property">Object that content the filters for print on tables.</param>
        /// <returns>When search is successfully, List of Properties with Owner information and Ok are returned, 
        /// otherwise BadRequest are returned.</returns>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<bool>))]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetProperties([FromQuery] PropertyDetailRequestDto property)
        {
            try
            {
                var result = await _bus.GetProperties(property);
                var response = new ApiResponse<PropertyDetailResponseDto>(result)
                {
                    IsSuccess = true,
                    ReturnMessage = $"Information has been successfully consulted."
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(BadRequestResult(ex));
            }
        }

        #endregion MainMethods

        #region PrivateMethods

        /// <summary>
        /// Private method for handling the message to print when exceptions occur.
        /// </summary>
        /// <param name="ex">Repesent an Exception.</param>
        /// <returns>Response of exception.</returns>
        [NonAction]
        private ApiResponse<string> BadRequestResult(Exception ex)
        {
            var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
            return new ApiResponse<string>(message)
            {
                IsSuccess = false,
                ReturnMessage = ERROR_PROCESS_INFORMATION
            };
        }

        /// <summary>
        /// Private method for handling the response of the database.
        /// </summary>
        /// <param name="result">Result of response of the database.</param>
        /// <returns>When true, Ok is returned, otherwise BadRequest is returned.</returns>
        [NonAction]
        private IActionResult AnalyzeResult(bool result)
        {
            var response = new ApiResponse<bool>(result)
            {
                IsSuccess = result
            };

            if (result)
            {
                response.ReturnMessage = SUCCESS_SAVE_INFORMATION;
                return Ok(response);
            }
            else
            {
                response.ReturnMessage = ERROR_PROCESS_INFORMATION;
                return BadRequest(response);
            };
        }

        #endregion PrivateMethods
    }
}
