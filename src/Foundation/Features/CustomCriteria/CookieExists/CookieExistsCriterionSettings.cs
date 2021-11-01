using EPiServer.Personalization.VisitorGroups;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.CustomCriteria.CookieExists
{
    public class CookieExistsCriterionSettings : CriterionModelBase
    {
        [Required]
        public string CookieName { get; set; }

        public override ICriterionModel Copy()
        {        
            return ShallowCopy();
        }
    }
}
