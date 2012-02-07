using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebRole.Models;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace WebRole.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Show(string id)
        {
            var container = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("DataConnectionString")).CreateCloudBlobClient().GetContainerReference("videos");

            return View(new Video {
                url = new UriBuilder(container.GetBlobReference(id + ".ism/Manifest").Uri) { Host = RoleEnvironment.GetConfigurationSettingValue("CdnHost") }.Uri.AbsoluteUri,
                thumbnail = container.GetBlobReference(id + "_Thumb.jpg").Uri.AbsoluteUri
            });
        }
    }
}
