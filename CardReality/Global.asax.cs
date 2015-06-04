using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CardReality.Controllers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CardReality
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public const string ErrorController = "Errors";
        public const string NotFoundAction = "NotFound404";
        public const string GeneralErrorAction = "Index";
        public const string ErrorPageContentType = "text/html";
        public const int NotFoundStatusCode = 404;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            InitializeStorage();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var lastError = Server.GetLastError();
            Server.ClearError();
            var routeData = new RouteData();
            routeData.Values["controller"] = ErrorController;

            
            var isHttpException = lastError is HttpException;
            var statusCode = -1;
            if (isHttpException)
            {
                statusCode = (lastError as HttpException).GetHttpCode();
            }

            if (statusCode == NotFoundStatusCode)
            {
                Response.StatusCode = NotFoundStatusCode;
                routeData.Values["action"] = NotFoundAction;
            }
            else
            {
                routeData.Values["action"] = GeneralErrorAction;
            }
              
            Response.TrySkipIisCustomErrors = true; // IIS7 fix
            Response.ContentType = ErrorPageContentType;
            IController errorsController = new ErrorsController();
            HttpContextWrapper wrapper = new HttpContextWrapper(Context);
            var rc = new System.Web.Routing.RequestContext(wrapper, routeData);
            errorsController.Execute(rc);
        }

        private void InitializeStorage()
        {
            // Open storage account using credentials from .cscfg file.
            var storageAccount = CloudStorageAccount.Parse
                (ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ToString());

            Trace.TraceInformation("Creating images blob container");
            var blobClient = storageAccount.CreateCloudBlobClient();
            var imagesBlobContainer = blobClient.GetContainerReference("images");
            if (imagesBlobContainer.CreateIfNotExists())
            {
                // Enable public access on the newly created "images" container.
                imagesBlobContainer.SetPermissions(
                    new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    });
            }

            Trace.TraceInformation("Creating queues");
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            var blobnameQueue = queueClient.GetQueueReference("thumbnailrequest");
            blobnameQueue.CreateIfNotExists();

            Trace.TraceInformation("Storage initialized");
        }
    }
}
