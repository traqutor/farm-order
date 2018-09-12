using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;


namespace FarmOrder.Controllers
{
    [RoutePrefix("")]
    public class HomeController : ApiController
    {
        [Route("")]
        [HttpGet]
        public HttpResponseMessage Index()
        {
            var response = Request.CreateResponse(HttpStatusCode.Moved);
        #if DEBUG

            response.Headers.Location = new Uri(Request.RequestUri + "/swagger");
            return response;

        #else

            response.Headers.Location = new Uri(Request.RequestUri + "/app");
            return response;
    
        #endif
        }
    }
}
