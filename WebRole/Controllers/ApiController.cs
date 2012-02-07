using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.IO;
using WebRole.Models;
using System.Xml.Linq;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace WebRole.Controllers
{
    public class ApiController : Controller
    {
        [HttpPost]
        public JsonResult Videos()
        {
            var container = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("DataConnectionString")).CreateCloudBlobClient().GetContainerReference("videos");
            var blobs = new Dictionary<string, CloudBlob>();
            foreach (var blob in container.ListBlobs().OfType<CloudBlob>())
            {
                blobs[blob.Uri.Segments.Last().TrimEnd('/').Replace("%20", " ")] = blob;
            }
            return Json(new
            {
                videos = blobs
                            .Where(pair => Path.GetExtension(pair.Key) == ".ism")
                            .Where(pair => XDocument.Load(pair.Value.Uri.AbsoluteUri).Elements("video").Select(v => v.Attribute("src").Value).All(ismv => blobs.ContainsKey(ismv)))
                            .OrderByDescending(pair => pair.Value.Properties.LastModifiedUtc)
                            .Select(pair => Path.GetFileNameWithoutExtension(pair.Key))
                            .Where(n => blobs.ContainsKey(n + "_Thumb.jpg"))
                            .Select(n => new Video
                            {
                                id = n,
                                thumbnail = container.GetBlobReference(n + "_Thumb.jpg").Uri.AbsoluteUri
                            })
            });
        }
    }
}