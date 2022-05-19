﻿using EPiServer.DataAbstraction;
using EPiServer.Framework.Web;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using Foundation.Features.Blocks.ButtonBlock;
using Foundation.Features.Shared;

namespace Foundation.Infrastructure.Display
{
    [ServiceConfiguration(typeof(IViewTemplateModelRegistrator))]
    public class ViewTemplateModelRegistrator : IViewTemplateModelRegistrator
    {
        public static void OnTemplateResolved(object sender, TemplateResolverEventArgs args)
        {

        }

        public const string FoundationFolder = "~/Features/Shared/Views/";

        public void Register(TemplateModelCollection viewTemplateModelRegistrator)
        {
            viewTemplateModelRegistrator.Add(typeof(FoundationPageData), new TemplateModel
            {
                Name = "PartialPage",
                Inherit = true,
                AvailableWithoutTag = true,
                TemplateTypeCategory = TemplateTypeCategories.MvcPartialView,
                Path = $"{FoundationFolder}_Page.cshtml"
            });

            //viewTemplateModelRegistrator.Add(typeof(ButtonBlock), new TemplateModel
            //{
            //    Name = "ButtonBlock",
            //    Inherit = true,
            //    AvailableWithoutTag = true,
            //    TemplateTypeCategory = TemplateTypeCategories.MvcPartialView,
            //    Path = "~/Features/Blocks/ButtonBlock/ButtonBlock.cshtml"
            //});
        }
    }
}