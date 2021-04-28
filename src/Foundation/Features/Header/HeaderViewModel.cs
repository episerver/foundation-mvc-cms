﻿using EPiServer.Core;
using EPiServer.SpecializedProperties;
using Foundation.Infrastructure.Cms.Users;
using Foundation.Features.Blocks.MenuItemBlock;
using Foundation.Features.Home;
using Foundation.Features.Settings;
using System;
using System.Collections.Generic;

namespace Foundation.Features.Header
{
    public class HeaderViewModel
    {
        public virtual HomePage HomePage { get; set; }
        public virtual LayoutSettings LayoutSettings { get; set; }
        public virtual LabelSettings LabelSettings { get; set; }
        public virtual ReferencePageSettings ReferencePageSettings { get; set; }
        public virtual SearchSettings SearchSettings { get; set; }
        public ContentReference CurrentContentLink { get; set; }
        public Guid CurrentContentGuid { get; set; }
        public LinkItemCollection UserLinks { get; set; }
        public string Name { get; set; }
        public List<MenuItemViewModel> MenuItems { get; set; }
        public bool IsReadonlyMode { get; set; }
        public bool LargeHeaderMenu { get; set; }
        public bool ShowCommerceControls { get; set; }
        public LoginViewModel LoginViewModel { get; set; }
        public RegisterAccountViewModel RegisterAccountViewModel { get; set; }
        public bool ShowSharedCart { get; set; }
        public PageData StorePage { get; set; }
        public LinkItemCollection RestrictedMenu { get; set; }
        public bool HasOrganization { get; set; }
        public bool IsBookmarked { get; set; }
    }
}