using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace service_harness.Core
{
    [Controller]
    public class ApiController : Controller
    {
        private readonly IConfiguration _configuration;
        protected ApiController(IConfiguration configuration)
        {
           _configuration = configuration;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }
    }
}
