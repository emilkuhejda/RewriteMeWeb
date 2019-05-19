(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["main"],{

/***/ "./src/$$_lazy_route_resource lazy recursive":
/*!**********************************************************!*\
  !*** ./src/$$_lazy_route_resource lazy namespace object ***!
  \**********************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

function webpackEmptyAsyncContext(req) {
	// Here Promise.resolve().then() is used instead of new Promise() to prevent
	// uncaught exception popping up in devtools
	return Promise.resolve().then(function() {
		var e = new Error("Cannot find module '" + req + "'");
		e.code = 'MODULE_NOT_FOUND';
		throw e;
	});
}
webpackEmptyAsyncContext.keys = function() { return []; };
webpackEmptyAsyncContext.resolve = webpackEmptyAsyncContext;
module.exports = webpackEmptyAsyncContext;
webpackEmptyAsyncContext.id = "./src/$$_lazy_route_resource lazy recursive";

/***/ }),

/***/ "./src/app/app-routing.module.ts":
/*!***************************************!*\
  !*** ./src/app/app-routing.module.ts ***!
  \***************************************/
/*! exports provided: AppRoutingModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AppRoutingModule", function() { return AppRoutingModule; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/fesm5/router.js");



var routes = [];
var AppRoutingModule = /** @class */ (function () {
    function AppRoutingModule() {
    }
    AppRoutingModule = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["NgModule"])({
            imports: [_angular_router__WEBPACK_IMPORTED_MODULE_2__["RouterModule"].forRoot(routes)],
            exports: [_angular_router__WEBPACK_IMPORTED_MODULE_2__["RouterModule"]]
        })
    ], AppRoutingModule);
    return AppRoutingModule;
}());



/***/ }),

/***/ "./src/app/app.component.css":
/*!***********************************!*\
  !*** ./src/app/app.component.css ***!
  \***********************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2FwcC5jb21wb25lbnQuY3NzIn0= */"

/***/ }),

/***/ "./src/app/app.component.html":
/*!************************************!*\
  !*** ./src/app/app.component.html ***!
  \************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<!-- Page Wrapper -->\n<div id=\"wrapper\">\n\n\t<!-- Sidebar -->\n\t<ul class=\"navbar-nav bg-gradient-primary sidebar sidebar-dark accordion\" id=\"accordionSidebar\">\n\n\t\t<!-- Sidebar - Brand -->\n\t\t<a class=\"sidebar-brand d-flex align-items-center justify-content-center\" href=\"index.html\">\n\t\t\t<div class=\"sidebar-brand-icon rotate-n-15\">\n\t\t\t\t<i class=\"fas fa-laugh-wink\"></i>\n\t\t\t</div>\n\t\t\t<div class=\"sidebar-brand-text mx-3\">SB Admin <sup>2</sup></div>\n\t\t</a>\n\n\t\t<!-- Divider -->\n\t\t<hr class=\"sidebar-divider my-0\">\n\n\t\t<!-- Nav Item - Dashboard -->\n\t\t<li class=\"nav-item active\">\n\t\t\t<a class=\"nav-link\" href=\"index.html\">\n\t\t\t\t<i class=\"fas fa-fw fa-tachometer-alt\"></i>\n\t\t\t\t<span>Dashboard</span></a>\n\t\t</li>\n\n\t\t<!-- Divider -->\n\t\t<hr class=\"sidebar-divider\">\n\n\t\t<!-- Heading -->\n\t\t<div class=\"sidebar-heading\">\n\t\t\tInterface\n\t\t</div>\n\n\t\t<!-- Nav Item - Pages Collapse Menu -->\n\t\t<li class=\"nav-item\">\n\t\t\t<a class=\"nav-link collapsed\" href=\"#\" data-toggle=\"collapse\" data-target=\"#collapseTwo\"\n\t\t\t\taria-expanded=\"true\" aria-controls=\"collapseTwo\">\n\t\t\t\t<i class=\"fas fa-fw fa-cog\"></i>\n\t\t\t\t<span>Components</span>\n\t\t\t</a>\n\t\t\t<div id=\"collapseTwo\" class=\"collapse\" aria-labelledby=\"headingTwo\" data-parent=\"#accordionSidebar\">\n\t\t\t\t<div class=\"bg-white py-2 collapse-inner rounded\">\n\t\t\t\t\t<h6 class=\"collapse-header\">Custom Components:</h6>\n\t\t\t\t\t<a class=\"collapse-item\" href=\"buttons.html\">Buttons</a>\n\t\t\t\t\t<a class=\"collapse-item\" href=\"cards.html\">Cards</a>\n\t\t\t\t</div>\n\t\t\t</div>\n\t\t</li>\n\n\t\t<!-- Nav Item - Utilities Collapse Menu -->\n\t\t<li class=\"nav-item\">\n\t\t\t<a class=\"nav-link collapsed\" href=\"#\" data-toggle=\"collapse\" data-target=\"#collapseUtilities\"\n\t\t\t\taria-expanded=\"true\" aria-controls=\"collapseUtilities\">\n\t\t\t\t<i class=\"fas fa-fw fa-wrench\"></i>\n\t\t\t\t<span>Utilities</span>\n\t\t\t</a>\n\t\t\t<div id=\"collapseUtilities\" class=\"collapse\" aria-labelledby=\"headingUtilities\"\n\t\t\t\tdata-parent=\"#accordionSidebar\">\n\t\t\t\t<div class=\"bg-white py-2 collapse-inner rounded\">\n\t\t\t\t\t<h6 class=\"collapse-header\">Custom Utilities:</h6>\n\t\t\t\t\t<a class=\"collapse-item\" href=\"utilities-color.html\">Colors</a>\n\t\t\t\t\t<a class=\"collapse-item\" href=\"utilities-border.html\">Borders</a>\n\t\t\t\t\t<a class=\"collapse-item\" href=\"utilities-animation.html\">Animations</a>\n\t\t\t\t\t<a class=\"collapse-item\" href=\"utilities-other.html\">Other</a>\n\t\t\t\t</div>\n\t\t\t</div>\n\t\t</li>\n\n\t\t<!-- Divider -->\n\t\t<hr class=\"sidebar-divider\">\n\n\t\t<!-- Heading -->\n\t\t<div class=\"sidebar-heading\">\n\t\t\tAddons\n\t\t</div>\n\n\t\t<!-- Nav Item - Pages Collapse Menu -->\n\t\t<li class=\"nav-item\">\n\t\t\t<a class=\"nav-link collapsed\" href=\"#\" data-toggle=\"collapse\" data-target=\"#collapsePages\"\n\t\t\t\taria-expanded=\"true\" aria-controls=\"collapsePages\">\n\t\t\t\t<i class=\"fas fa-fw fa-folder\"></i>\n\t\t\t\t<span>Pages</span>\n\t\t\t</a>\n\t\t\t<div id=\"collapsePages\" class=\"collapse\" aria-labelledby=\"headingPages\" data-parent=\"#accordionSidebar\">\n\t\t\t\t<div class=\"bg-white py-2 collapse-inner rounded\">\n\t\t\t\t\t<h6 class=\"collapse-header\">Login Screens:</h6>\n\t\t\t\t\t<a class=\"collapse-item\" href=\"login.html\">Login</a>\n\t\t\t\t\t<a class=\"collapse-item\" href=\"register.html\">Register</a>\n\t\t\t\t\t<a class=\"collapse-item\" href=\"forgot-password.html\">Forgot Password</a>\n\t\t\t\t\t<div class=\"collapse-divider\"></div>\n\t\t\t\t\t<h6 class=\"collapse-header\">Other Pages:</h6>\n\t\t\t\t\t<a class=\"collapse-item\" href=\"404.html\">404 Page</a>\n\t\t\t\t\t<a class=\"collapse-item\" href=\"blank.html\">Blank Page</a>\n\t\t\t\t</div>\n\t\t\t</div>\n\t\t</li>\n\n\t\t<!-- Nav Item - Charts -->\n\t\t<li class=\"nav-item\">\n\t\t\t<a class=\"nav-link\" href=\"charts.html\">\n\t\t\t\t<i class=\"fas fa-fw fa-chart-area\"></i>\n\t\t\t\t<span>Charts</span></a>\n\t\t</li>\n\n\t\t<!-- Nav Item - Tables -->\n\t\t<li class=\"nav-item\">\n\t\t\t<a class=\"nav-link\" href=\"tables.html\">\n\t\t\t\t<i class=\"fas fa-fw fa-table\"></i>\n\t\t\t\t<span>Tables</span></a>\n\t\t</li>\n\n\t\t<!-- Divider -->\n\t\t<hr class=\"sidebar-divider d-none d-md-block\">\n\n\t\t<!-- Sidebar Toggler (Sidebar) -->\n\t\t<div class=\"text-center d-none d-md-inline\">\n\t\t\t<button class=\"rounded-circle border-0\" id=\"sidebarToggle\"></button>\n\t\t</div>\n\n\t</ul>\n\t<!-- End of Sidebar -->\n\n\t<!-- Content Wrapper -->\n\t<div id=\"content-wrapper\" class=\"d-flex flex-column\">\n\n\t\t<!-- Main Content -->\n\t\t<div id=\"content\">\n\n\t\t\t<!-- Topbar -->\n\t\t\t<nav class=\"navbar navbar-expand navbar-light bg-white topbar mb-4 static-top shadow\">\n\n\t\t\t\t<!-- Sidebar Toggle (Topbar) -->\n\t\t\t\t<button id=\"sidebarToggleTop\" class=\"btn btn-link d-md-none rounded-circle mr-3\">\n\t\t\t\t\t<i class=\"fa fa-bars\"></i>\n\t\t\t\t</button>\n\n\t\t\t\t<!-- Topbar Search -->\n\t\t\t\t<form class=\"d-none d-sm-inline-block form-inline mr-auto ml-md-3 my-2 my-md-0 mw-100 navbar-search\">\n\t\t\t\t\t<div class=\"input-group\">\n\t\t\t\t\t\t<input type=\"text\" class=\"form-control bg-light border-0 small\" placeholder=\"Search for...\"\n\t\t\t\t\t\t\taria-label=\"Search\" aria-describedby=\"basic-addon2\">\n\t\t\t\t\t\t<div class=\"input-group-append\">\n\t\t\t\t\t\t\t<button class=\"btn btn-primary\" type=\"button\">\n\t\t\t\t\t\t\t\t<i class=\"fas fa-search fa-sm\"></i>\n\t\t\t\t\t\t\t</button>\n\t\t\t\t\t\t</div>\n\t\t\t\t\t</div>\n\t\t\t\t</form>\n\n\t\t\t\t<!-- Topbar Navbar -->\n\t\t\t\t<ul class=\"navbar-nav ml-auto\">\n\n\t\t\t\t\t<!-- Nav Item - Search Dropdown (Visible Only XS) -->\n\t\t\t\t\t<li class=\"nav-item dropdown no-arrow d-sm-none\">\n\t\t\t\t\t\t<a class=\"nav-link dropdown-toggle\" href=\"#\" id=\"searchDropdown\" role=\"button\"\n\t\t\t\t\t\t\tdata-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">\n\t\t\t\t\t\t\t<i class=\"fas fa-search fa-fw\"></i>\n\t\t\t\t\t\t</a>\n\t\t\t\t\t\t<!-- Dropdown - Messages -->\n\t\t\t\t\t\t<div class=\"dropdown-menu dropdown-menu-right p-3 shadow animated--grow-in\"\n\t\t\t\t\t\t\taria-labelledby=\"searchDropdown\">\n\t\t\t\t\t\t\t<form class=\"form-inline mr-auto w-100 navbar-search\">\n\t\t\t\t\t\t\t\t<div class=\"input-group\">\n\t\t\t\t\t\t\t\t\t<input type=\"text\" class=\"form-control bg-light border-0 small\"\n\t\t\t\t\t\t\t\t\t\tplaceholder=\"Search for...\" aria-label=\"Search\" aria-describedby=\"basic-addon2\">\n\t\t\t\t\t\t\t\t\t<div class=\"input-group-append\">\n\t\t\t\t\t\t\t\t\t\t<button class=\"btn btn-primary\" type=\"button\">\n\t\t\t\t\t\t\t\t\t\t\t<i class=\"fas fa-search fa-sm\"></i>\n\t\t\t\t\t\t\t\t\t\t</button>\n\t\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t</form>\n\t\t\t\t\t\t</div>\n\t\t\t\t\t</li>\n\n\t\t\t\t\t<!-- Nav Item - Alerts -->\n\t\t\t\t\t<li class=\"nav-item dropdown no-arrow mx-1\">\n\t\t\t\t\t\t<a class=\"nav-link dropdown-toggle\" href=\"#\" id=\"alertsDropdown\" role=\"button\"\n\t\t\t\t\t\t\tdata-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">\n\t\t\t\t\t\t\t<i class=\"fas fa-bell fa-fw\"></i>\n\t\t\t\t\t\t\t<!-- Counter - Alerts -->\n\t\t\t\t\t\t\t<span class=\"badge badge-danger badge-counter\">3+</span>\n\t\t\t\t\t\t</a>\n\t\t\t\t\t\t<!-- Dropdown - Alerts -->\n\t\t\t\t\t\t<div class=\"dropdown-list dropdown-menu dropdown-menu-right shadow animated--grow-in\"\n\t\t\t\t\t\t\taria-labelledby=\"alertsDropdown\">\n\t\t\t\t\t\t\t<h6 class=\"dropdown-header\">\n\t\t\t\t\t\t\t\tAlerts Center\n\t\t\t\t\t\t\t</h6>\n\t\t\t\t\t\t\t<a class=\"dropdown-item d-flex align-items-center\" href=\"#\">\n\t\t\t\t\t\t\t\t<div class=\"mr-3\">\n\t\t\t\t\t\t\t\t\t<div class=\"icon-circle bg-primary\">\n\t\t\t\t\t\t\t\t\t\t<i class=\"fas fa-file-alt text-white\"></i>\n\t\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t<div>\n\t\t\t\t\t\t\t\t\t<div class=\"small text-gray-500\">December 12, 2019</div>\n\t\t\t\t\t\t\t\t\t<span class=\"font-weight-bold\">A new monthly report is ready to download!</span>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t</a>\n\t\t\t\t\t\t\t<a class=\"dropdown-item d-flex align-items-center\" href=\"#\">\n\t\t\t\t\t\t\t\t<div class=\"mr-3\">\n\t\t\t\t\t\t\t\t\t<div class=\"icon-circle bg-success\">\n\t\t\t\t\t\t\t\t\t\t<i class=\"fas fa-donate text-white\"></i>\n\t\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t<div>\n\t\t\t\t\t\t\t\t\t<div class=\"small text-gray-500\">December 7, 2019</div>\n\t\t\t\t\t\t\t\t\t$290.29 has been deposited into your account!\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t</a>\n\t\t\t\t\t\t\t<a class=\"dropdown-item d-flex align-items-center\" href=\"#\">\n\t\t\t\t\t\t\t\t<div class=\"mr-3\">\n\t\t\t\t\t\t\t\t\t<div class=\"icon-circle bg-warning\">\n\t\t\t\t\t\t\t\t\t\t<i class=\"fas fa-exclamation-triangle text-white\"></i>\n\t\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t<div>\n\t\t\t\t\t\t\t\t\t<div class=\"small text-gray-500\">December 2, 2019</div>\n\t\t\t\t\t\t\t\t\tSpending Alert: We've noticed unusually high spending for your account.\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t</a>\n\t\t\t\t\t\t\t<a class=\"dropdown-item text-center small text-gray-500\" href=\"#\">Show All Alerts</a>\n\t\t\t\t\t\t</div>\n\t\t\t\t\t</li>\n\n\t\t\t\t\t<!-- Nav Item - Messages -->\n\t\t\t\t\t<li class=\"nav-item dropdown no-arrow mx-1\">\n\t\t\t\t\t\t<a class=\"nav-link dropdown-toggle\" href=\"#\" id=\"messagesDropdown\" role=\"button\"\n\t\t\t\t\t\t\tdata-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">\n\t\t\t\t\t\t\t<i class=\"fas fa-envelope fa-fw\"></i>\n\t\t\t\t\t\t\t<!-- Counter - Messages -->\n\t\t\t\t\t\t\t<span class=\"badge badge-danger badge-counter\">7</span>\n\t\t\t\t\t\t</a>\n\t\t\t\t\t\t<!-- Dropdown - Messages -->\n\t\t\t\t\t\t<div class=\"dropdown-list dropdown-menu dropdown-menu-right shadow animated--grow-in\"\n\t\t\t\t\t\t\taria-labelledby=\"messagesDropdown\">\n\t\t\t\t\t\t\t<h6 class=\"dropdown-header\">\n\t\t\t\t\t\t\t\tMessage Center\n\t\t\t\t\t\t\t</h6>\n\t\t\t\t\t\t\t<a class=\"dropdown-item d-flex align-items-center\" href=\"#\">\n\t\t\t\t\t\t\t\t<div class=\"dropdown-list-image mr-3\">\n\t\t\t\t\t\t\t\t\t<img class=\"rounded-circle\" src=\"https://source.unsplash.com/fn_BT9fwg_E/60x60\"\n\t\t\t\t\t\t\t\t\t\talt=\"\">\n\t\t\t\t\t\t\t\t\t<div class=\"status-indicator bg-success\"></div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t<div class=\"font-weight-bold\">\n\t\t\t\t\t\t\t\t\t<div class=\"text-truncate\">Hi there! I am wondering if you can help me with a\n\t\t\t\t\t\t\t\t\t\tproblem I've been having.</div>\n\t\t\t\t\t\t\t\t\t<div class=\"small text-gray-500\">Emily Fowler · 58m</div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t</a>\n\t\t\t\t\t\t\t<a class=\"dropdown-item d-flex align-items-center\" href=\"#\">\n\t\t\t\t\t\t\t\t<div class=\"dropdown-list-image mr-3\">\n\t\t\t\t\t\t\t\t\t<img class=\"rounded-circle\" src=\"https://source.unsplash.com/AU4VPcFN4LE/60x60\"\n\t\t\t\t\t\t\t\t\t\talt=\"\">\n\t\t\t\t\t\t\t\t\t<div class=\"status-indicator\"></div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t<div>\n\t\t\t\t\t\t\t\t\t<div class=\"text-truncate\">I have the photos that you ordered last month, how would\n\t\t\t\t\t\t\t\t\t\tyou like them sent to you?</div>\n\t\t\t\t\t\t\t\t\t<div class=\"small text-gray-500\">Jae Chun · 1d</div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t</a>\n\t\t\t\t\t\t\t<a class=\"dropdown-item d-flex align-items-center\" href=\"#\">\n\t\t\t\t\t\t\t\t<div class=\"dropdown-list-image mr-3\">\n\t\t\t\t\t\t\t\t\t<img class=\"rounded-circle\" src=\"https://source.unsplash.com/CS2uCrpNzJY/60x60\"\n\t\t\t\t\t\t\t\t\t\talt=\"\">\n\t\t\t\t\t\t\t\t\t<div class=\"status-indicator bg-warning\"></div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t<div>\n\t\t\t\t\t\t\t\t\t<div class=\"text-truncate\">Last month's report looks great, I am very happy with the\n\t\t\t\t\t\t\t\t\t\tprogress so far, keep up the good work!</div>\n\t\t\t\t\t\t\t\t\t<div class=\"small text-gray-500\">Morgan Alvarez · 2d</div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t</a>\n\t\t\t\t\t\t\t<a class=\"dropdown-item d-flex align-items-center\" href=\"#\">\n\t\t\t\t\t\t\t\t<div class=\"dropdown-list-image mr-3\">\n\t\t\t\t\t\t\t\t\t<img class=\"rounded-circle\" src=\"https://source.unsplash.com/Mv9hjnEUHR4/60x60\"\n\t\t\t\t\t\t\t\t\t\talt=\"\">\n\t\t\t\t\t\t\t\t\t<div class=\"status-indicator bg-success\"></div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t<div>\n\t\t\t\t\t\t\t\t\t<div class=\"text-truncate\">Am I a good boy? The reason I ask is because someone told\n\t\t\t\t\t\t\t\t\t\tme that people say this to all dogs, even if they aren't good...</div>\n\t\t\t\t\t\t\t\t\t<div class=\"small text-gray-500\">Chicken the Dog · 2w</div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t</a>\n\t\t\t\t\t\t\t<a class=\"dropdown-item text-center small text-gray-500\" href=\"#\">Read More Messages</a>\n\t\t\t\t\t\t</div>\n\t\t\t\t\t</li>\n\n\t\t\t\t\t<div class=\"topbar-divider d-none d-sm-block\"></div>\n\n\t\t\t\t\t<!-- Nav Item - User Information -->\n\t\t\t\t\t<li class=\"nav-item dropdown no-arrow\">\n\t\t\t\t\t\t<a class=\"nav-link dropdown-toggle\" href=\"#\" id=\"userDropdown\" role=\"button\"\n\t\t\t\t\t\t\tdata-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">\n\t\t\t\t\t\t\t<span class=\"mr-2 d-none d-lg-inline text-gray-600 small\">Valerie Luna</span>\n\t\t\t\t\t\t\t<img class=\"img-profile rounded-circle\" src=\"https://source.unsplash.com/QAB-WJcbgJk/60x60\">\n\t\t\t\t\t\t</a>\n\t\t\t\t\t\t<!-- Dropdown - User Information -->\n\t\t\t\t\t\t<div class=\"dropdown-menu dropdown-menu-right shadow animated--grow-in\"\n\t\t\t\t\t\t\taria-labelledby=\"userDropdown\">\n\t\t\t\t\t\t\t<a class=\"dropdown-item\" href=\"#\">\n\t\t\t\t\t\t\t\t<i class=\"fas fa-user fa-sm fa-fw mr-2 text-gray-400\"></i>\n\t\t\t\t\t\t\t\tProfile\n\t\t\t\t\t\t\t</a>\n\t\t\t\t\t\t\t<a class=\"dropdown-item\" href=\"#\">\n\t\t\t\t\t\t\t\t<i class=\"fas fa-cogs fa-sm fa-fw mr-2 text-gray-400\"></i>\n\t\t\t\t\t\t\t\tSettings\n\t\t\t\t\t\t\t</a>\n\t\t\t\t\t\t\t<a class=\"dropdown-item\" href=\"#\">\n\t\t\t\t\t\t\t\t<i class=\"fas fa-list fa-sm fa-fw mr-2 text-gray-400\"></i>\n\t\t\t\t\t\t\t\tActivity Log\n\t\t\t\t\t\t\t</a>\n\t\t\t\t\t\t\t<div class=\"dropdown-divider\"></div>\n\t\t\t\t\t\t\t<a class=\"dropdown-item\" href=\"#\" data-toggle=\"modal\" data-target=\"#logoutModal\">\n\t\t\t\t\t\t\t\t<i class=\"fas fa-sign-out-alt fa-sm fa-fw mr-2 text-gray-400\"></i>\n\t\t\t\t\t\t\t\tLogout\n\t\t\t\t\t\t\t</a>\n\t\t\t\t\t\t</div>\n\t\t\t\t\t</li>\n\n\t\t\t\t</ul>\n\n\t\t\t</nav>\n\t\t\t<!-- End of Topbar -->\n\n\t\t\t<!-- Begin Page Content -->\n\t\t\t<div class=\"container-fluid\">\n\n\t\t\t\t<!-- Page Heading -->\n\t\t\t\t<div class=\"d-sm-flex align-items-center justify-content-between mb-4\">\n\t\t\t\t\t<h1 class=\"h3 mb-0 text-gray-800\">Dashboard</h1>\n\t\t\t\t\t<a href=\"#\" class=\"d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm\"><i\n\t\t\t\t\t\t\tclass=\"fas fa-download fa-sm text-white-50\"></i> Generate Report</a>\n\t\t\t\t</div>\n\n\t\t\t\t<!-- Content Row -->\n\t\t\t\t<div class=\"row\">\n\n\t\t\t\t\t<!-- Earnings (Monthly) Card Example -->\n\t\t\t\t\t<div class=\"col-xl-3 col-md-6 mb-4\">\n\t\t\t\t\t\t<div class=\"card border-left-primary shadow h-100 py-2\">\n\t\t\t\t\t\t\t<div class=\"card-body\">\n\t\t\t\t\t\t\t\t<div class=\"row no-gutters align-items-center\">\n\t\t\t\t\t\t\t\t\t<div class=\"col mr-2\">\n\t\t\t\t\t\t\t\t\t\t<div class=\"text-xs font-weight-bold text-primary text-uppercase mb-1\">Earnings\n\t\t\t\t\t\t\t\t\t\t\t(Monthly)</div>\n\t\t\t\t\t\t\t\t\t\t<div class=\"h5 mb-0 font-weight-bold text-gray-800\">$40,000</div>\n\t\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t\t<div class=\"col-auto\">\n\t\t\t\t\t\t\t\t\t\t<i class=\"fas fa-calendar fa-2x text-gray-300\"></i>\n\t\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t</div>\n\t\t\t\t\t</div>\n\n\t\t\t\t\t<!-- Earnings (Monthly) Card Example -->\n\t\t\t\t\t<div class=\"col-xl-3 col-md-6 mb-4\">\n\t\t\t\t\t\t<div class=\"card border-left-success shadow h-100 py-2\">\n\t\t\t\t\t\t\t<div class=\"card-body\">\n\t\t\t\t\t\t\t\t<div class=\"row no-gutters align-items-center\">\n\t\t\t\t\t\t\t\t\t<div class=\"col mr-2\">\n\t\t\t\t\t\t\t\t\t\t<div class=\"text-xs font-weight-bold text-success text-uppercase mb-1\">Earnings\n\t\t\t\t\t\t\t\t\t\t\t(Annual)</div>\n\t\t\t\t\t\t\t\t\t\t<div class=\"h5 mb-0 font-weight-bold text-gray-800\">$215,000</div>\n\t\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t\t<div class=\"col-auto\">\n\t\t\t\t\t\t\t\t\t\t<i class=\"fas fa-dollar-sign fa-2x text-gray-300\"></i>\n\t\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t</div>\n\t\t\t\t\t</div>\n\n\t\t\t\t\t<!-- Earnings (Monthly) Card Example -->\n\t\t\t\t\t<div class=\"col-xl-3 col-md-6 mb-4\">\n\t\t\t\t\t\t<div class=\"card border-left-info shadow h-100 py-2\">\n\t\t\t\t\t\t\t<div class=\"card-body\">\n\t\t\t\t\t\t\t\t<div class=\"row no-gutters align-items-center\">\n\t\t\t\t\t\t\t\t\t<div class=\"col mr-2\">\n\t\t\t\t\t\t\t\t\t\t<div class=\"text-xs font-weight-bold text-info text-uppercase mb-1\">Tasks</div>\n\t\t\t\t\t\t\t\t\t\t<div class=\"row no-gutters align-items-center\">\n\t\t\t\t\t\t\t\t\t\t\t<div class=\"col-auto\">\n\t\t\t\t\t\t\t\t\t\t\t\t<div class=\"h5 mb-0 mr-3 font-weight-bold text-gray-800\">50%</div>\n\t\t\t\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t\t\t\t<div class=\"col\">\n\t\t\t\t\t\t\t\t\t\t\t\t<div class=\"progress progress-sm mr-2\">\n\t\t\t\t\t\t\t\t\t\t\t\t\t<div class=\"progress-bar bg-info\" role=\"progressbar\"\n\t\t\t\t\t\t\t\t\t\t\t\t\t\tstyle=\"width: 50%\" aria-valuenow=\"50\" aria-valuemin=\"0\"\n\t\t\t\t\t\t\t\t\t\t\t\t\t\taria-valuemax=\"100\"></div>\n\t\t\t\t\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t\t<div class=\"col-auto\">\n\t\t\t\t\t\t\t\t\t\t<i class=\"fas fa-clipboard-list fa-2x text-gray-300\"></i>\n\t\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t</div>\n\t\t\t\t\t</div>\n\n\t\t\t\t\t<!-- Pending Requests Card Example -->\n\t\t\t\t\t<div class=\"col-xl-3 col-md-6 mb-4\">\n\t\t\t\t\t\t<div class=\"card border-left-warning shadow h-100 py-2\">\n\t\t\t\t\t\t\t<div class=\"card-body\">\n\t\t\t\t\t\t\t\t<div class=\"row no-gutters align-items-center\">\n\t\t\t\t\t\t\t\t\t<div class=\"col mr-2\">\n\t\t\t\t\t\t\t\t\t\t<div class=\"text-xs font-weight-bold text-warning text-uppercase mb-1\">Pending\n\t\t\t\t\t\t\t\t\t\t\tRequests</div>\n\t\t\t\t\t\t\t\t\t\t<div class=\"h5 mb-0 font-weight-bold text-gray-800\">18</div>\n\t\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t\t<div class=\"col-auto\">\n\t\t\t\t\t\t\t\t\t\t<i class=\"fas fa-comments fa-2x text-gray-300\"></i>\n\t\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t</div>\n\t\t\t\t\t</div>\n\t\t\t\t</div>\n\n\t\t\t\t<!-- Content Row -->\n\n\t\t\t\t<div class=\"row\">\n\n\t\t\t\t\t<!-- Area Chart -->\n\t\t\t\t\t<div class=\"col-xl-8 col-lg-7\">\n\t\t\t\t\t\t<div class=\"card shadow mb-4\">\n\t\t\t\t\t\t\t<!-- Card Header - Dropdown -->\n\t\t\t\t\t\t\t<div class=\"card-header py-3 d-flex flex-row align-items-center justify-content-between\">\n\t\t\t\t\t\t\t\t<h6 class=\"m-0 font-weight-bold text-primary\">Earnings Overview</h6>\n\t\t\t\t\t\t\t\t<div class=\"dropdown no-arrow\">\n\t\t\t\t\t\t\t\t\t<a class=\"dropdown-toggle\" href=\"#\" role=\"button\" id=\"dropdownMenuLink\"\n\t\t\t\t\t\t\t\t\t\tdata-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">\n\t\t\t\t\t\t\t\t\t\t<i class=\"fas fa-ellipsis-v fa-sm fa-fw text-gray-400\"></i>\n\t\t\t\t\t\t\t\t\t</a>\n\t\t\t\t\t\t\t\t\t<div class=\"dropdown-menu dropdown-menu-right shadow animated--fade-in\"\n\t\t\t\t\t\t\t\t\t\taria-labelledby=\"dropdownMenuLink\">\n\t\t\t\t\t\t\t\t\t\t<div class=\"dropdown-header\">Dropdown Header:</div>\n\t\t\t\t\t\t\t\t\t\t<a class=\"dropdown-item\" href=\"#\">Action</a>\n\t\t\t\t\t\t\t\t\t\t<a class=\"dropdown-item\" href=\"#\">Another action</a>\n\t\t\t\t\t\t\t\t\t\t<div class=\"dropdown-divider\"></div>\n\t\t\t\t\t\t\t\t\t\t<a class=\"dropdown-item\" href=\"#\">Something else here</a>\n\t\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t<!-- Card Body -->\n\t\t\t\t\t\t\t<div class=\"card-body\">\n\t\t\t\t\t\t\t\t<div class=\"chart-area\">\n\t\t\t\t\t\t\t\t\t<canvas id=\"myAreaChart\"></canvas>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t</div>\n\t\t\t\t\t</div>\n\n\t\t\t\t\t<!-- Pie Chart -->\n\t\t\t\t\t<div class=\"col-xl-4 col-lg-5\">\n\t\t\t\t\t\t<div class=\"card shadow mb-4\">\n\t\t\t\t\t\t\t<!-- Card Header - Dropdown -->\n\t\t\t\t\t\t\t<div class=\"card-header py-3 d-flex flex-row align-items-center justify-content-between\">\n\t\t\t\t\t\t\t\t<h6 class=\"m-0 font-weight-bold text-primary\">Revenue Sources</h6>\n\t\t\t\t\t\t\t\t<div class=\"dropdown no-arrow\">\n\t\t\t\t\t\t\t\t\t<a class=\"dropdown-toggle\" href=\"#\" role=\"button\" id=\"dropdownMenuLink\"\n\t\t\t\t\t\t\t\t\t\tdata-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">\n\t\t\t\t\t\t\t\t\t\t<i class=\"fas fa-ellipsis-v fa-sm fa-fw text-gray-400\"></i>\n\t\t\t\t\t\t\t\t\t</a>\n\t\t\t\t\t\t\t\t\t<div class=\"dropdown-menu dropdown-menu-right shadow animated--fade-in\"\n\t\t\t\t\t\t\t\t\t\taria-labelledby=\"dropdownMenuLink\">\n\t\t\t\t\t\t\t\t\t\t<div class=\"dropdown-header\">Dropdown Header:</div>\n\t\t\t\t\t\t\t\t\t\t<a class=\"dropdown-item\" href=\"#\">Action</a>\n\t\t\t\t\t\t\t\t\t\t<a class=\"dropdown-item\" href=\"#\">Another action</a>\n\t\t\t\t\t\t\t\t\t\t<div class=\"dropdown-divider\"></div>\n\t\t\t\t\t\t\t\t\t\t<a class=\"dropdown-item\" href=\"#\">Something else here</a>\n\t\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t<!-- Card Body -->\n\t\t\t\t\t\t\t<div class=\"card-body\">\n\t\t\t\t\t\t\t\t<div class=\"chart-pie pt-4 pb-2\">\n\t\t\t\t\t\t\t\t\t<canvas id=\"myPieChart\"></canvas>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t<div class=\"mt-4 text-center small\">\n\t\t\t\t\t\t\t\t\t<span class=\"mr-2\">\n\t\t\t\t\t\t\t\t\t\t<i class=\"fas fa-circle text-primary\"></i> Direct\n\t\t\t\t\t\t\t\t\t</span>\n\t\t\t\t\t\t\t\t\t<span class=\"mr-2\">\n\t\t\t\t\t\t\t\t\t\t<i class=\"fas fa-circle text-success\"></i> Social\n\t\t\t\t\t\t\t\t\t</span>\n\t\t\t\t\t\t\t\t\t<span class=\"mr-2\">\n\t\t\t\t\t\t\t\t\t\t<i class=\"fas fa-circle text-info\"></i> Referral\n\t\t\t\t\t\t\t\t\t</span>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t</div>\n\t\t\t\t\t</div>\n\t\t\t\t</div>\n\n\t\t\t\t<!-- Content Row -->\n\t\t\t\t<div class=\"row\">\n\n\t\t\t\t\t<!-- Content Column -->\n\t\t\t\t\t<div class=\"col-lg-6 mb-4\">\n\n\t\t\t\t\t\t<!-- Project Card Example -->\n\t\t\t\t\t\t<div class=\"card shadow mb-4\">\n\t\t\t\t\t\t\t<div class=\"card-header py-3\">\n\t\t\t\t\t\t\t\t<h6 class=\"m-0 font-weight-bold text-primary\">Projects</h6>\n\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t<div class=\"card-body\">\n\t\t\t\t\t\t\t\t<h4 class=\"small font-weight-bold\">Server Migration <span class=\"float-right\">20%</span>\n\t\t\t\t\t\t\t\t</h4>\n\t\t\t\t\t\t\t\t<div class=\"progress mb-4\">\n\t\t\t\t\t\t\t\t\t<div class=\"progress-bar bg-danger\" role=\"progressbar\" style=\"width: 20%\"\n\t\t\t\t\t\t\t\t\t\taria-valuenow=\"20\" aria-valuemin=\"0\" aria-valuemax=\"100\"></div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t<h4 class=\"small font-weight-bold\">Sales Tracking <span class=\"float-right\">40%</span>\n\t\t\t\t\t\t\t\t</h4>\n\t\t\t\t\t\t\t\t<div class=\"progress mb-4\">\n\t\t\t\t\t\t\t\t\t<div class=\"progress-bar bg-warning\" role=\"progressbar\" style=\"width: 40%\"\n\t\t\t\t\t\t\t\t\t\taria-valuenow=\"40\" aria-valuemin=\"0\" aria-valuemax=\"100\"></div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t<h4 class=\"small font-weight-bold\">Customer Database <span\n\t\t\t\t\t\t\t\t\t\tclass=\"float-right\">60%</span></h4>\n\t\t\t\t\t\t\t\t<div class=\"progress mb-4\">\n\t\t\t\t\t\t\t\t\t<div class=\"progress-bar\" role=\"progressbar\" style=\"width: 60%\" aria-valuenow=\"60\"\n\t\t\t\t\t\t\t\t\t\taria-valuemin=\"0\" aria-valuemax=\"100\"></div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t<h4 class=\"small font-weight-bold\">Payout Details <span class=\"float-right\">80%</span>\n\t\t\t\t\t\t\t\t</h4>\n\t\t\t\t\t\t\t\t<div class=\"progress mb-4\">\n\t\t\t\t\t\t\t\t\t<div class=\"progress-bar bg-info\" role=\"progressbar\" style=\"width: 80%\"\n\t\t\t\t\t\t\t\t\t\taria-valuenow=\"80\" aria-valuemin=\"0\" aria-valuemax=\"100\"></div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t<h4 class=\"small font-weight-bold\">Account Setup <span\n\t\t\t\t\t\t\t\t\t\tclass=\"float-right\">Complete!</span></h4>\n\t\t\t\t\t\t\t\t<div class=\"progress\">\n\t\t\t\t\t\t\t\t\t<div class=\"progress-bar bg-success\" role=\"progressbar\" style=\"width: 100%\"\n\t\t\t\t\t\t\t\t\t\taria-valuenow=\"100\" aria-valuemin=\"0\" aria-valuemax=\"100\"></div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t</div>\n\n\t\t\t\t\t\t<!-- Color System -->\n\t\t\t\t\t\t<div class=\"row\">\n\t\t\t\t\t\t\t<div class=\"col-lg-6 mb-4\">\n\t\t\t\t\t\t\t\t<div class=\"card bg-primary text-white shadow\">\n\t\t\t\t\t\t\t\t\t<div class=\"card-body\">\n\t\t\t\t\t\t\t\t\t\tPrimary\n\t\t\t\t\t\t\t\t\t\t<div class=\"text-white-50 small\">#4e73df</div>\n\t\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t<div class=\"col-lg-6 mb-4\">\n\t\t\t\t\t\t\t\t<div class=\"card bg-success text-white shadow\">\n\t\t\t\t\t\t\t\t\t<div class=\"card-body\">\n\t\t\t\t\t\t\t\t\t\tSuccess\n\t\t\t\t\t\t\t\t\t\t<div class=\"text-white-50 small\">#1cc88a</div>\n\t\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t<div class=\"col-lg-6 mb-4\">\n\t\t\t\t\t\t\t\t<div class=\"card bg-info text-white shadow\">\n\t\t\t\t\t\t\t\t\t<div class=\"card-body\">\n\t\t\t\t\t\t\t\t\t\tInfo\n\t\t\t\t\t\t\t\t\t\t<div class=\"text-white-50 small\">#36b9cc</div>\n\t\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t<div class=\"col-lg-6 mb-4\">\n\t\t\t\t\t\t\t\t<div class=\"card bg-warning text-white shadow\">\n\t\t\t\t\t\t\t\t\t<div class=\"card-body\">\n\t\t\t\t\t\t\t\t\t\tWarning\n\t\t\t\t\t\t\t\t\t\t<div class=\"text-white-50 small\">#f6c23e</div>\n\t\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t<div class=\"col-lg-6 mb-4\">\n\t\t\t\t\t\t\t\t<div class=\"card bg-danger text-white shadow\">\n\t\t\t\t\t\t\t\t\t<div class=\"card-body\">\n\t\t\t\t\t\t\t\t\t\tDanger\n\t\t\t\t\t\t\t\t\t\t<div class=\"text-white-50 small\">#e74a3b</div>\n\t\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t<div class=\"col-lg-6 mb-4\">\n\t\t\t\t\t\t\t\t<div class=\"card bg-secondary text-white shadow\">\n\t\t\t\t\t\t\t\t\t<div class=\"card-body\">\n\t\t\t\t\t\t\t\t\t\tSecondary\n\t\t\t\t\t\t\t\t\t\t<div class=\"text-white-50 small\">#858796</div>\n\t\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t</div>\n\n\t\t\t\t\t</div>\n\n\t\t\t\t\t<div class=\"col-lg-6 mb-4\">\n\n\t\t\t\t\t\t<!-- Illustrations -->\n\t\t\t\t\t\t<div class=\"card shadow mb-4\">\n\t\t\t\t\t\t\t<div class=\"card-header py-3\">\n\t\t\t\t\t\t\t\t<h6 class=\"m-0 font-weight-bold text-primary\">Illustrations</h6>\n\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t<div class=\"card-body\">\n\t\t\t\t\t\t\t\t<div class=\"text-center\">\n\t\t\t\t\t\t\t\t\t<img class=\"img-fluid px-3 px-sm-4 mt-3 mb-4\" style=\"width: 25rem;\"\n\t\t\t\t\t\t\t\t\t\tsrc=\"img/undraw_posting_photo.svg\" alt=\"\">\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t\t<p>Add some quality, svg illustrations to your project courtesy of <a target=\"_blank\"\n\t\t\t\t\t\t\t\t\t\trel=\"nofollow\" href=\"https://undraw.co/\">unDraw</a>, a constantly updated\n\t\t\t\t\t\t\t\t\tcollection of beautiful svg images that you can use completely free and without\n\t\t\t\t\t\t\t\t\tattribution!</p>\n\t\t\t\t\t\t\t\t<a target=\"_blank\" rel=\"nofollow\" href=\"https://undraw.co/\">Browse Illustrations on\n\t\t\t\t\t\t\t\t\tunDraw &rarr;</a>\n\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t</div>\n\n\t\t\t\t\t\t<!-- Approach -->\n\t\t\t\t\t\t<div class=\"card shadow mb-4\">\n\t\t\t\t\t\t\t<div class=\"card-header py-3\">\n\t\t\t\t\t\t\t\t<h6 class=\"m-0 font-weight-bold text-primary\">Development Approach</h6>\n\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t<div class=\"card-body\">\n\t\t\t\t\t\t\t\t<p>SB Admin 2 makes extensive use of Bootstrap 4 utility classes in order to reduce CSS\n\t\t\t\t\t\t\t\t\tbloat and poor page performance. Custom CSS classes are used to create custom\n\t\t\t\t\t\t\t\t\tcomponents and custom utility classes.</p>\n\t\t\t\t\t\t\t\t<p class=\"mb-0\">Before working with this theme, you should become familiar with the\n\t\t\t\t\t\t\t\t\tBootstrap framework, especially the utility classes.</p>\n\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t</div>\n\n\t\t\t\t\t</div>\n\t\t\t\t</div>\n\n\t\t\t</div>\n\t\t\t<!-- /.container-fluid -->\n\n\t\t</div>\n\t\t<!-- End of Main Content -->\n\n\t\t<!-- Footer -->\n\t\t<footer class=\"sticky-footer bg-white\">\n\t\t\t<div class=\"container my-auto\">\n\t\t\t\t<div class=\"copyright text-center my-auto\">\n\t\t\t\t\t<span>Copyright &copy; Your Website 2019</span>\n\t\t\t\t</div>\n\t\t\t</div>\n\t\t</footer>\n\t\t<!-- End of Footer -->\n\n\t</div>\n\t<!-- End of Content Wrapper -->\n\n</div>\n<!-- End of Page Wrapper -->\n\n<!-- Scroll to Top Button-->\n<a class=\"scroll-to-top rounded\" href=\"#page-top\">\n\t<i class=\"fas fa-angle-up\"></i>\n</a>\n\n<!-- Logout Modal-->\n<div class=\"modal fade\" id=\"logoutModal\" tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"exampleModalLabel\"\n\taria-hidden=\"true\">\n\t<div class=\"modal-dialog\" role=\"document\">\n\t\t<div class=\"modal-content\">\n\t\t\t<div class=\"modal-header\">\n\t\t\t\t<h5 class=\"modal-title\" id=\"exampleModalLabel\">Ready to Leave?</h5>\n\t\t\t\t<button class=\"close\" type=\"button\" data-dismiss=\"modal\" aria-label=\"Close\">\n\t\t\t\t\t<span aria-hidden=\"true\">×</span>\n\t\t\t\t</button>\n\t\t\t</div>\n\t\t\t<div class=\"modal-body\">Select \"Logout\" below if you are ready to end your current session.</div>\n\t\t\t<div class=\"modal-footer\">\n\t\t\t\t<button class=\"btn btn-secondary\" type=\"button\" data-dismiss=\"modal\">Cancel</button>\n\t\t\t\t<a class=\"btn btn-primary\" href=\"login.html\">Logout</a>\n\t\t\t</div>\n\t\t</div>\n\t</div>\n</div>"

/***/ }),

/***/ "./src/app/app.component.ts":
/*!**********************************!*\
  !*** ./src/app/app.component.ts ***!
  \**********************************/
/*! exports provided: AppComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AppComponent", function() { return AppComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");


var AppComponent = /** @class */ (function () {
    function AppComponent() {
        this.title = 'WebProfileClient';
    }
    AppComponent = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"])({
            selector: 'app-root',
            template: __webpack_require__(/*! ./app.component.html */ "./src/app/app.component.html"),
            styles: [__webpack_require__(/*! ./app.component.css */ "./src/app/app.component.css")]
        })
    ], AppComponent);
    return AppComponent;
}());



/***/ }),

/***/ "./src/app/app.module.ts":
/*!*******************************!*\
  !*** ./src/app/app.module.ts ***!
  \*******************************/
/*! exports provided: AppModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AppModule", function() { return AppModule; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_platform_browser__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/platform-browser */ "./node_modules/@angular/platform-browser/fesm5/platform-browser.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _app_routing_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./app-routing.module */ "./src/app/app-routing.module.ts");
/* harmony import */ var _app_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./app.component */ "./src/app/app.component.ts");





var AppModule = /** @class */ (function () {
    function AppModule() {
    }
    AppModule = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_2__["NgModule"])({
            declarations: [
                _app_component__WEBPACK_IMPORTED_MODULE_4__["AppComponent"]
            ],
            imports: [
                _angular_platform_browser__WEBPACK_IMPORTED_MODULE_1__["BrowserModule"],
                _app_routing_module__WEBPACK_IMPORTED_MODULE_3__["AppRoutingModule"]
            ],
            providers: [],
            bootstrap: [_app_component__WEBPACK_IMPORTED_MODULE_4__["AppComponent"]]
        })
    ], AppModule);
    return AppModule;
}());



/***/ }),

/***/ "./src/environments/environment.ts":
/*!*****************************************!*\
  !*** ./src/environments/environment.ts ***!
  \*****************************************/
/*! exports provided: environment */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "environment", function() { return environment; });
// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.
var environment = {
    production: false
};
/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.


/***/ }),

/***/ "./src/main.ts":
/*!*********************!*\
  !*** ./src/main.ts ***!
  \*********************/
/*! no exports provided */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _angular_platform_browser_dynamic__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/platform-browser-dynamic */ "./node_modules/@angular/platform-browser-dynamic/fesm5/platform-browser-dynamic.js");
/* harmony import */ var _app_app_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./app/app.module */ "./src/app/app.module.ts");
/* harmony import */ var _environments_environment__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./environments/environment */ "./src/environments/environment.ts");




if (_environments_environment__WEBPACK_IMPORTED_MODULE_3__["environment"].production) {
    Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["enableProdMode"])();
}
Object(_angular_platform_browser_dynamic__WEBPACK_IMPORTED_MODULE_1__["platformBrowserDynamic"])().bootstrapModule(_app_app_module__WEBPACK_IMPORTED_MODULE_2__["AppModule"])
    .catch(function (err) { return console.error(err); });


/***/ }),

/***/ 0:
/*!***************************!*\
  !*** multi ./src/main.ts ***!
  \***************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__(/*! D:\Projects\RewriteMeWeb\WebProfileClient\src\main.ts */"./src/main.ts");


/***/ })

},[[0,"runtime","vendor"]]]);
//# sourceMappingURL=main.js.map