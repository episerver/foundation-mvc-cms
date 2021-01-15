﻿using EPiServer.Core;
using EPiServer.SpecializedProperties;

namespace Foundation.Features.Header
{
    public enum MyAccountPageType
    {
        Link,
        Organization,
    }

    public class MyAccountNavigationViewModel
    {
        public ContentReference OrganizationPage { get; set; }
        public ContentReference SubOrganizationPage { get; set; }
        public MyAccountPageType CurrentPageType { get; set; }
        public string CurrentPageText { get; set; }
        public LinkItemCollection MenuItemCollection { get; set; }
    }
}