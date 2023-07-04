using manager_properties_usa.Models.Dto;
using manager_properties_usa.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace manager_properties_usa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private IUAuthentication _auth;

        public AuthenticationController(IUAuthentication auth)
        {
            _auth = auth;
        }

        /// <summary>
        /// Get token to consume the APIs.
        /// </summary>
        /// <param name="userId">For authenticate the user.</param>
        /// <returns> One token.</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> AuthenticateUser([FromQuery]string userId)
        {
            try
            {
                var result = await _auth.Authenticate(userId);
                var response = new ApiResponse<TokenDataDto>(result)
                {
                    IsSuccess = true,
                    ReturnMessage = $"Token has been successfully generated."
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var result = new ApiResponse<string>(ex.InnerException == null ? ex.Message : ex.InnerException.Message)
                {
                    IsSuccess = false,
                    ReturnMessage = "Error getting token."
                };
                return BadRequest(result);
            }
        }
    }
}