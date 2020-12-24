using token_api.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using service_harness.Core;
using service_harness.Models;
using service_harness.Security;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace token_api.Controllers
{
    [Route("api/[action]")]
    public class TokenController : ApiController
    {
        #region PROPERTIES
        readonly IConfiguration _configuration;
        readonly EmailTokenContext _context;
        #endregion

        #region CONSTRUCTOR
        public TokenController(EmailTokenContext context, IConfiguration configuration) : base(configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        #endregion

        #region GET
        [HttpGet]
        [ActionName("token/get-token")]
        public async Task<Token> GetToken(string email)
        {
            TokenHandler tokenHandler = new TokenHandler(_configuration);
            Token token = tokenHandler.CreateAccessToken();
            EmailToken emailToken = new EmailToken() { Email = email, RefreshToken = token.RefreshToken, RefreshTokenEndDate = token.Expiration };

            var currentEmailToken = await _context.EmailTokens.FirstOrDefaultAsync(x => x.Email == email);

            if (currentEmailToken != null)
            {
                currentEmailToken.RefreshToken = emailToken.RefreshToken;
                currentEmailToken.RefreshTokenEndDate = emailToken.RefreshTokenEndDate;
            }
            else
            {
                _context.EmailTokens.Add(emailToken);
            }
            await _context.SaveChangesAsync();

            return token;
        }

        [HttpGet]
        [ActionName("token/has-valid-token")]
        public async Task<bool> HasValidToken(string email)
        {
            var token = await _context.EmailTokens.FirstOrDefaultAsync(x => x.Email == email);

            if (token != null && token.RefreshTokenEndDate < DateTime.Now)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
