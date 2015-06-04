using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using CardReality.Data;
using CardReality.Data.Data;
using CardReality.Data.Models;
using CardReality.Models;
using Microsoft.AspNet.Identity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CardReality.Controllers
{
    public class BaseController : Controller
    {
        private IApplicationData data;
        protected CloudQueue thumbnailRequestQueue;
        protected static CloudBlobContainer imagesBlobContainer;

        //public ApplicationDbContext Data { get; private set; }

        public BaseController(IApplicationData data)
        {
            this.Data = data;
            InitializeStorage();
        }

        protected IApplicationData Data 
        { 
            get { return this.data; }
            private set { this.data = value; }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var identity = filterContext.RequestContext.HttpContext.User.Identity;
            bool isSubscribed = false;
            if (identity.IsAuthenticated)
            {
                Player player = this.Data.Players.Find(identity.GetUserId());
                isSubscribed = player.BattleSubscribed;
            }

            this.ViewData["isSubscribed"] = isSubscribed;

            base.OnActionExecuting(filterContext);
        }

        private void InitializeStorage()
        {
            // Open storage account using credentials from .cscfg file.
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ToString());

            // Get context object for working with blobs, and 
            // set a default retry policy appropriate for a web user interface.
            var blobClient = storageAccount.CreateCloudBlobClient();
            //blobClient.DefaultRequestOptions.RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(3), 3);

            // Get a reference to the blob container.
            imagesBlobContainer = blobClient.GetContainerReference("images");

            // Get context object for working with queues, and 
            // set a default retry policy appropriate for a web user interface.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            //queueClient.DefaultRequestOptions.RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(3), 3);

            // Get a reference to the queue.
            thumbnailRequestQueue = queueClient.GetQueueReference("thumbnailrequest");
        }
    }
}