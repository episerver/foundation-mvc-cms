using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;

namespace Foundation.Features.FileUploadPage
{
    [ContentType(DisplayName = "File upload page",
        GUID = "DBED4258-8213-48DB-A22F-99C034182454",
        Description = "File upload page",
        GroupName = GroupNames.Content)]
    public class FileUploadPage : PageData
    {
    }
}