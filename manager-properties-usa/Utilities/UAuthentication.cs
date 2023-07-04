using System.Runtime.ExceptionServices;
using manager_properties_usa.Models.Dto;

namespace manager_properties_usa.Utilities
{
    public class UAuthentication : IUAuthentication
    {
        private readonly IUConfiguration _cnf;
        public UAuthentication(IUConfiguration cnf)
        {
            _cnf = cnf;
        }

        /// <summary>
        /// Get token to consume the APIs.
        /// </summary>
        /// <param name="userId">For authenticate the user.</param>
        /// <returns></returns>
        public Task<TokenDataDto> Authenticate(string userId)
        {
            TokenDataDto tkData = new();
            try
            {
                var tokenInfo = _cnf.GenerateToken(userId);
                tkData = new()
                {
                    Token = tokenInfo
                };
            }
            catch (Exception ex)
            {
                ExceptionDispatchInfo.Capture(ex).Throw();
            }
            return Task.FromResult(tkData);
        }
    }

    public interface IUAuthentication
    {
        Task<TokenDataDto> Authenticate(string userId);
    }
}