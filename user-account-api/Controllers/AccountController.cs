using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using service_harness.Core;
using service_harness.Security;
using service_harness.Services;

namespace user_account_api.Controllers
{
    [Route("api/[action]")]
    public class AccountController : ApiController
    {
        #region PROPERTIES
        private readonly IEmailService _emailService;
        private readonly IHttpClientFactory _clientFactory;
        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public AccountController(IHttpClientFactory clientFactory, IConfiguration configuration, IEmailService emailService) : base(configuration)
        {
            _clientFactory = clientFactory;
            _emailService = emailService;
        }
        #endregion

        #region GET
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("account/forget-password")]
        public async Task<string> ForgetPassword(string email)
        {
            HttpClient httpClient = _clientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("http://localhost:5002/");
            var response = await httpClient.GetStringAsync("/api/token/get-token?&email="+email);
            Token token = JsonConvert.DeserializeObject<Token>(response);
            string confirmationLink = $"https://localhost:5001/api/account/temp-login?useremail={email}&token={token.AccessToken}";
            _emailService.SendMail(confirmationLink);

            return "Mail sended!";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="useremail"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("account/temp-login")]
        public async Task<ActionResult<string>> TempLogin(string useremail)
        {
            bool result = await LoginByEmailAddress(useremail);

            if (result)
            {
                return "Hello Player,You login successfully";
            }
            else
            {
                return "Link is invalid";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        private async Task<bool>  LoginByEmailAddress(string email)
        {
            HttpClient httpClient = _clientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("http://localhost:5002/");
            var response = await httpClient.GetStringAsync("/api/token/has-valid-token?&email=" + email);
            bool isValid = bool.Parse(response);

            if(isValid)
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}
