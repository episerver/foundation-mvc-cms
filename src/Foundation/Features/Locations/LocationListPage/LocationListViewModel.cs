using EPiServer.Find.Cms;
using EPiServer.Personalization;
using System.Collections.Specialized;

namespace Foundation.Find.Cms.Locations.ViewModels
{
    public class LocationListViewModel : ContentViewModel<LocationListPage>
    {
        public LocationListViewModel(LocationListPage currentPage) : base(currentPage) { }

        public GeoCoordinate MapCenter { get; set; }
        public IGeolocationResult UserLocation { get; set; }
        public IContentResult<LocationItemPage> Locations { get; set; }
        public NameValueCollection QueryString { get; set; }
    }
}