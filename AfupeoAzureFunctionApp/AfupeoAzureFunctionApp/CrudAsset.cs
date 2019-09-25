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
using System.Net.Http;

namespace AfupeoAzureFunctionApp
{
    public class CrudAsset
    {
        ISqlManager sqlManager;
        CRUD dao;

        public CrudAsset(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }
        [FunctionName("CrudAsset")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            string errorMessage = @"Missing parameter.
available Assets : 
- Société
- Banque
- Compte
- mode_paiement
- Classification
- Sous_Classification
available command : 
-create
-Read
-Update
-Delete
error : ";
            dynamic request = await req.Content.ReadAsAsync<object>();
            if (request?.asset == null || request?.asset == "" || request?.command == null || request?.command == "")
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(errorMessage + " no asset or command")
                };

            string result = "";
            try
            {

                switch (request?.asset.ToString().ToLower())
                {
                    case "societe":
                        this.dao = new SocieteDAO(this.sqlManager);
                        switch (request?.command.ToString().ToLower())
                        {
                            case "create":
                                this.dao.Create(request?.data.ToString());
                                break;
                            case "read":
                                result = this.dao.Read();
                                break;
                            case "update":
                                this.dao.Update(request?.data.ToString());
                                break;
                            case "delete":
                                this.dao.Delete(request?.data.ToString());
                                break;
                            default:
                                throw new Exception(errorMessage);

                        }
                        break;
                    case "banque":
                        this.dao = new BanqueDAO(this.sqlManager);
                        switch (request?.command.ToString().ToLower())
                        {
                            case "create":
                                this.dao.Create(request?.data.ToString());
                                break;
                            case "read":
                                result = this.dao.Read();
                                break;
                            case "update":
                                this.dao.Update(request?.data.ToString());
                                break;
                            case "delete":
                                  this.dao.Delete(request?.data.ToString());
                                break;
                            default:
                                throw new Exception(errorMessage);

                        }
                        break;
                    case "compte":
                        this.dao = new CompteDAO(this.sqlManager);
                        switch (request?.command.ToString().ToLower())
                        {
                            case "create":
                                this.dao.Create(request?.data.ToString());
                                break;
                            case "read":
                                result = this.dao.Read();
                                break;
                            case "update":
                                this.dao.Update(request?.data.ToString());
                                break;
                            case "delete":
                                this.dao.Delete(request?.data.ToString());
                                break;
                            default:
                                throw new Exception(errorMessage);
                        }
                        break;
                    case "mode_paiement":
                        this.dao = new PaiementMethodDAO(this.sqlManager);
                        switch (request?.command.ToString().ToLower())
                        {
                            case "create":
                                this.dao.Create(request?.data.ToString());
                                break;
                            case "read":
                                result = this.dao.Read();
                                break;
                            case "update":
                                this.dao.Update(request?.data.ToString());
                                break;
                            case "delete":
                                this.dao.Delete(request?.data.ToString());
                                break;
                            default:
                                throw new Exception(errorMessage);
                        }
                        break;
                    case "classification":
                        this.dao = new ClassificationDAO(this.sqlManager);
                        switch (request?.command.ToString().ToLower())
                        {
                            case "create":
                                this.dao.Create(request?.data.ToString());
                                break;
                            case "read":
                                result = this.dao.Read();
                                break;
                            case "update":
                                this.dao.Update(request?.data.ToString());
                                break;
                            case "delete":
                                this.dao.Delete(request?.data.ToString());
                                break;
                            default:
                                throw new Exception(errorMessage);
                        }
                        break;
                    case "sous_classification":
                        this.dao = new SousClassificationDao(this.sqlManager);
                        switch (request?.command.ToString().ToLower())
                        {
                            case "create":
                                this.dao.Create(request?.data.ToString());
                                break;
                            case "read":
                                result = this.dao.Read();
                                break;
                            case "update":
                                this.dao.Update(request?.data.ToString());
                                break;
                            case "delete":
                                this.dao.Delete(request?.data.ToString());
                                break;
                            default:
                                throw new Exception(errorMessage);
                        }
                        break;
                }
            }
            catch (Exception e)
            {

                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(errorMessage + e.Message)
                };
             }

            return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(result)
            };
        }
    }
}
