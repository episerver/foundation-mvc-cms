using EPiServer.Personalization.VisitorGroups;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using System.Linq;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Statistics;
using EPiServer.Framework.Localization;
using Foundation.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Web;

namespace Foundation.Features.CustomCriteria.CookieExists
{
    [VisitorGroupCriterion(   
        Category = "Technical",
        DisplayName = "Cookie Exists",
        Description = "Checks if a specific cookie exists")]

    public class CookieExistsCriterion : CriterionBase<CookieExistsCriterionSettings>
    {
        public override bool IsMatch(IPrincipal principal, HttpContext httpContext)
        { 
            return httpContext.Request.Cookies[Model.CookieName] != null;
        }
    }
}
