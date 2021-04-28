using Foundation.Infrastructure.Helpers;
using System;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Features.ErrorPage
{
    public class ErrorController : Controller
    {

        [HttpGet]
        public ActionResult Index(Exception exception)
        {
            //Response.StatusCode = 500;
            var model = Url.GetUserViewModel("/", "Error");
            model.ErrorMessage = exception.Message;
            model.StackTrace = exception.StackTrace;
            return View("ErrorPage", model);
        }
    }
}