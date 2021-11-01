using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Web.Mvc;
using Foundation.Features.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using EPiServer.Framework.Blobs;
using EPiServer.PlugIn;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing;
using System;
using System.IO;
using System.Security.Policy;
using System.Text;
namespace Foundation.Features.FileUploadPage
{
    public class FileUploadPageController : PageController<FileUploadPage>
    {
        private readonly IBlobFactory _blobFactory;
        private readonly IContentRepository _contentRepository;
        private readonly ContentAssetHelper _contentAssetHelper;
        // private readonly IPageRouteHelper _pageRouteHelper;
        
        public FileUploadPageController(IBlobFactory blobFactory, IContentRepository contentRepository, ContentAssetHelper contentAssetHelper)
        {
            _blobFactory = blobFactory;
            _contentRepository = contentRepository;
            _contentAssetHelper = contentAssetHelper;
            // _pageRouteHelper = pageRouteHelper;
        }
        
        public ActionResult Index(FileUploadPage currentPage)
        {
            var model = ContentViewModel.Create(currentPage);
            return View(model);
        }

        [HttpPost]
        [Route("Local")]
        public void FileUploadLocal(IFormFile file)
        {
            
            Console.WriteLine("Start uploading local");
            Console.WriteLine(file == null ? "null" : "not null");
            Console.WriteLine(file.Name + "-" + file.FileName);
            Console.WriteLine(_blobFactory == null ? "blob factory null" : "blob factory not tull");
            Console.WriteLine(_contentRepository == null ? "content repository null" : "content repository not tull");
            Console.WriteLine(_contentAssetHelper == null ? "content asset null" : "content asset not tull");
            
            var pageRouteHelper = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<IPageRouteHelper>();

            Console.WriteLine(pageRouteHelper == null ? "page route helper null" : "page route helper not tull");

            var currentPage = pageRouteHelper.Page;
            Console.WriteLine(currentPage == null ? "current page null" : "current page not null");

            var file1 = _contentRepository.GetDefault<GenericFile>(_contentAssetHelper.GetOrCreateAssetFolder(currentPage.ContentLink).ContentLink);
            Console.WriteLine(file1 == null ? "file is null" : "file is not null");
            file1.Name = file.FileName;
            var blob = _blobFactory.CreateBlob(file1.BinaryDataContainer, ".txt");
            using (var s = blob.OpenWrite())
            {
                file.CopyTo(s);
            }

            file1.BinaryData = blob;
            var file1ID = _contentRepository.Save(file1, SaveAction.Publish);
            Console.WriteLine("file id", file1ID);

            //return RedirectToAction("Index", "FileUploadPage");
        }
        
        [HttpPost]
        [Route("Global")]

        public void FileUploadGlobal(IFormFile file)
        {
            Console.WriteLine("Start uploading global");
            Console.WriteLine(file.Name);
            // var model = ContentViewModel.Create(new FileUploadPage());
            // return View(model);
            Console.WriteLine(file == null ? "null" : "not null");
            Console.WriteLine(file.Name + "-" + file.FileName);
            Console.WriteLine(_blobFactory == null ? "blob factory null" : "blob factory not tull");
            Console.WriteLine(_contentRepository == null ? "content repository null" : "content repository not tull");
            
            var file1 = _contentRepository.GetDefault<GenericFile>(SiteDefinition.Current.GlobalAssetsRoot);
            file1.Name = file.FileName;
            var blob = _blobFactory.CreateBlob(file1.BinaryDataContainer, ".txt");
            using (var s = blob.OpenWrite())
            {
                file.CopyTo(s);
            }

            file1.BinaryData = blob;
            var file1ID = _contentRepository.Save(file1, SaveAction.Publish);
            Console.WriteLine("file id", file1ID);
            //return RedirectToAction("Index", "FileUploadPage");
        }
    }
}