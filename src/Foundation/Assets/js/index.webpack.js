import "bootstrap";
import "lazysizes";
import "../scss/main.scss"

// import Locations from "./features/selective/Locations"
// import People from "./features/selective/People"
// import BlockTracking from "./features/BlockTracking"
// import Blog from "./features/Blog"
// import Dropdown from "./features/Dropdown"
// import FoundationCms from "./features/foundation.cms"
// import Header from "./features/Header"
// import MobileNavigation from "./features/mobile-navigation"
// import PdfPreview from "./features/PdfPreview"
// import { ProductSearch,FilterOption ,ContentSearch ,NewProductsSearch, SalesSearch } from "./features/Search";
// import SearchBox from "./features/SearchBox"
// import Selection from "./features/Selection"
import MyProfile from "./features/MyProfile"
import NotifyHelper from "./features/NotifyHelper"
import FoundationInit from "./features/foundation.init"

var foudationInit = new FoundationInit();
foudationInit.init();

// // convert json to formdata and append __RequestVerificationToken key for CORS
// window.convertFormData = function (data, containerToken) {
//     var formData = new FormData();
//     var isAddedToken = false;
//     for (var key in data) {
//         if (key == "__RequestVerificationToken") {
//             isAddedToken = true;
//         }
//         formData.append(key, data[key]);
//     }

//     if (!isAddedToken) {
//         if (containerToken) {
//             formData.append("__RequestVerificationToken", $(containerToken + ' input[name=__RequestVerificationToken]').val());
//         } else {
//             formData.append("__RequestVerificationToken", $('input[name=__RequestVerificationToken]').val());
//         }
//     }

//     return formData;
// };

// window.serializeObject = function (form) {
//     var datas = form.serializeArray();
//     var jsonData = {};
//     for (var d in datas) {
//         jsonData[datas[d].name] = datas[d].value;
//     }

//     return jsonData;
// };

// PdfPreview();

// var header = new Header();
// header.init();

// var params = {
//     searchBoxId: "#mobile-searchbox",
//     openSearchBoxId: "#open-searh-box",
//     closeSearchBoxId: "#close-search-box",
//     sideBarId: "#offside-menu-mobile",
//     openSideBarId: "#open-offside-menu"
// }

// var mobileNavigation = new MobileNavigation(params);
// mobileNavigation.init();

// var blockTracking = new BlockTracking();
// blockTracking.init();

// var selection = new Selection();
// selection.init();

// var dropdown = new Dropdown();
// dropdown.init();

// var searchBox = new SearchBox();
// searchBox.init();

// var blog = new Blog();
// blog.init();

// var productSearch = new ProductSearch();
// productSearch.init();

// var locations = new Locations();
// locations.init();

// var people = new People();
// people.init();

// var cms = new FoundationCms();
// cms.init();