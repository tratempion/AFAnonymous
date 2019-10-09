using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DAL;

namespace AfupeoAzureFunctionApp
{
    public  class debug
    {
        private ISqlManager _sqlManager;

        public debug(ISqlManager sqlManager)
        {
            _sqlManager = sqlManager;
        }
        [FunctionName("debug")]
        public  async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
 
            string data = req.Query["data"];
            var doubl = double.Parse(data);

            return data != null
                ? (ActionResult)new OkObjectResult(doubl.ToString())
                : new BadRequestObjectResult("Please pass a name on the query string");
        }
    }
}
