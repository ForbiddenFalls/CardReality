using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CardReality.Data;
using CardReality.Data.Data;
using CardReality.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace CardReality.Controllers
{
    [Authorize]
    public class ProfileController : BaseController
    {
        public ProfileController(IApplicationData data) : base(data)
        {
        }

        // GET: Profile
        [HttpGet]
        public ActionResult Index()
        {
            Player player = this.Data.Players.Find(User.Identity.GetUserId());
            return View(player);
        }

        // GET: Profile/Edit/5
        public ActionResult Edit()
        {
            string currentUser = User.Identity.GetUserId();
            Player player = this.Data.Players.All().FirstOrDefault(p => p.Id == currentUser);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // POST: Profile/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(HttpPostedFileBase imageFile)
        {
            string currentUser = User.Identity.GetUserId();
            var user = this.Data.Players.All().FirstOrDefault(p => p.Id == currentUser);
            if (user == null)
            {
                return HttpNotFound();
            }
            var oldUserName = user.UserName;
            CloudBlockBlob imageBlob = null;

            if (imageFile != null && imageFile.ContentLength != 0)
            {
                imageBlob = await UploadAndSaveBlobAsync(imageFile);
                user.ImageURL = imageBlob.Uri.ToString();
            }

            string modelUserName = this.Request.Form.Get("UserName");
            if (modelUserName != null && modelUserName.Length >= 4)
            {
                user.UserName = this.Request.Form.Get("UserName");
            }

            this.Data.Players.Update(user);
            try
            {
                await this.Data.SaveChangesAsync();
            }
            catch (Exception)
            {
                user.UserName = oldUserName;
                this.Data.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            Trace.TraceInformation("Created PlayerId {0} in database", user.Id);

            if (imageBlob != null)
            {
                BlobInformation blobInfo = new BlobInformation() { PlayerId = user.Id, BlobUri = new Uri(user.ImageURL) };
                var queueMessage = new CloudQueueMessage(JsonConvert.SerializeObject(blobInfo));
                await thumbnailRequestQueue.AddMessageAsync(queueMessage);
                Trace.TraceInformation("Created queue message for PlayerId {0}", user.Id);
            }
            return RedirectToAction("Index");
        }

        private async Task<CloudBlockBlob> UploadAndSaveBlobAsync(HttpPostedFileBase imageFile)
        {
            Trace.TraceInformation("Uploading image file {0}", imageFile.FileName);

            string blobName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            // Retrieve reference to a blob. 
            CloudBlockBlob imageBlob = imagesBlobContainer.GetBlockBlobReference(blobName);
            // Create the blob by uploading a local file.
            using (var fileStream = imageFile.InputStream)
            {
                await imageBlob.UploadFromStreamAsync(fileStream);
            }

            Trace.TraceInformation("Uploaded image file to {0}", imageBlob.Uri.ToString());

            return imageBlob;
        }
    }
}
