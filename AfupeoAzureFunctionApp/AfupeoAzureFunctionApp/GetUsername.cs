using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.DirectoryServices.ActiveDirectory;
using System.Security.Claims;
using System.Net.Http;
using System.Net;
using DAL;

namespace AfupeoAzureFunctionApp
{
    public  class GetUsername
    {

        [FunctionName("GetUsername")]
        public  async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log,
            ClaimsPrincipal claimsPrincipal)
        {
            try
            {

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(claimsPrincipal.Identity.Name ?? "jhon.smith@expaceo.com")
                };
            }
            catch (Exception e)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(e.Message)
                };


            }
        }
    }
}
