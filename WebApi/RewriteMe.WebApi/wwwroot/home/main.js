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

module.exports = "<!-- Navigation -->\n<nav class=\"navbar navbar-expand-lg bg-secondary fixed-top text-uppercase\" id=\"mainNav\">\n    <div class=\"container\">\n        <a class=\"navbar-brand js-scroll-trigger\" href=\"#page-top\">Start Bootstrap</a>\n        <button class=\"navbar-toggler navbar-toggler-right text-uppercase bg-primary text-white rounded\" type=\"button\"\n            data-toggle=\"collapse\" data-target=\"#navbarResponsive\" aria-controls=\"navbarResponsive\"\n            aria-expanded=\"false\" aria-label=\"Toggle navigation\">\n            Menu\n            <i class=\"fas fa-bars\"></i>\n        </button>\n        <div class=\"collapse navbar-collapse\" id=\"navbarResponsive\">\n            <ul class=\"navbar-nav ml-auto\">\n                <li class=\"nav-item mx-0 mx-lg-1\">\n                    <a class=\"nav-link py-3 px-0 px-lg-3 rounded js-scroll-trigger\" href=\"#portfolio\">Portfolio</a>\n                </li>\n                <li class=\"nav-item mx-0 mx-lg-1\">\n                    <a class=\"nav-link py-3 px-0 px-lg-3 rounded js-scroll-trigger\" href=\"#about\">About</a>\n                </li>\n                <li class=\"nav-item mx-0 mx-lg-1\">\n                    <a class=\"nav-link py-3 px-0 px-lg-3 rounded js-scroll-trigger\" href=\"#contact\">Contact</a>\n                </li>\n            </ul>\n        </div>\n    </div>\n</nav>\n\n<!-- Header -->\n<header class=\"masthead bg-primary text-white text-center\">\n    <div class=\"container\">\n        <img class=\"img-fluid mb-5 d-block mx-auto\" src=\"img/profile.png\" alt=\"\">\n        <h1 class=\"text-uppercase mb-0\">Start Bootstrap</h1>\n        <hr class=\"star-light\">\n        <h2 class=\"font-weight-light mb-0\">Web Developer - Graphic Artist - User Experience Designer</h2>\n    </div>\n</header>\n\n<!-- Portfolio Grid Section -->\n<section class=\"portfolio\" id=\"portfolio\">\n    <div class=\"container\">\n        <h2 class=\"text-center text-uppercase text-secondary mb-0\">Portfolio</h2>\n        <hr class=\"star-dark mb-5\">\n        <div class=\"row\">\n            <div class=\"col-md-6 col-lg-4\">\n                <a class=\"portfolio-item d-block mx-auto\" href=\"#portfolio-modal-1\">\n                    <div class=\"portfolio-item-caption d-flex position-absolute h-100 w-100\">\n                        <div class=\"portfolio-item-caption-content my-auto w-100 text-center text-white\">\n                            <i class=\"fas fa-search-plus fa-3x\"></i>\n                        </div>\n                    </div>\n                    <img class=\"img-fluid\" src=\"img/portfolio/cabin.png\" alt=\"\">\n                </a>\n            </div>\n            <div class=\"col-md-6 col-lg-4\">\n                <a class=\"portfolio-item d-block mx-auto\" href=\"#portfolio-modal-2\">\n                    <div class=\"portfolio-item-caption d-flex position-absolute h-100 w-100\">\n                        <div class=\"portfolio-item-caption-content my-auto w-100 text-center text-white\">\n                            <i class=\"fas fa-search-plus fa-3x\"></i>\n                        </div>\n                    </div>\n                    <img class=\"img-fluid\" src=\"img/portfolio/cake.png\" alt=\"\">\n                </a>\n            </div>\n            <div class=\"col-md-6 col-lg-4\">\n                <a class=\"portfolio-item d-block mx-auto\" href=\"#portfolio-modal-3\">\n                    <div class=\"portfolio-item-caption d-flex position-absolute h-100 w-100\">\n                        <div class=\"portfolio-item-caption-content my-auto w-100 text-center text-white\">\n                            <i class=\"fas fa-search-plus fa-3x\"></i>\n                        </div>\n                    </div>\n                    <img class=\"img-fluid\" src=\"img/portfolio/circus.png\" alt=\"\">\n                </a>\n            </div>\n            <div class=\"col-md-6 col-lg-4\">\n                <a class=\"portfolio-item d-block mx-auto\" href=\"#portfolio-modal-4\">\n                    <div class=\"portfolio-item-caption d-flex position-absolute h-100 w-100\">\n                        <div class=\"portfolio-item-caption-content my-auto w-100 text-center text-white\">\n                            <i class=\"fas fa-search-plus fa-3x\"></i>\n                        </div>\n                    </div>\n                    <img class=\"img-fluid\" src=\"img/portfolio/game.png\" alt=\"\">\n                </a>\n            </div>\n            <div class=\"col-md-6 col-lg-4\">\n                <a class=\"portfolio-item d-block mx-auto\" href=\"#portfolio-modal-5\">\n                    <div class=\"portfolio-item-caption d-flex position-absolute h-100 w-100\">\n                        <div class=\"portfolio-item-caption-content my-auto w-100 text-center text-white\">\n                            <i class=\"fas fa-search-plus fa-3x\"></i>\n                        </div>\n                    </div>\n                    <img class=\"img-fluid\" src=\"img/portfolio/safe.png\" alt=\"\">\n                </a>\n            </div>\n            <div class=\"col-md-6 col-lg-4\">\n                <a class=\"portfolio-item d-block mx-auto\" href=\"#portfolio-modal-6\">\n                    <div class=\"portfolio-item-caption d-flex position-absolute h-100 w-100\">\n                        <div class=\"portfolio-item-caption-content my-auto w-100 text-center text-white\">\n                            <i class=\"fas fa-search-plus fa-3x\"></i>\n                        </div>\n                    </div>\n                    <img class=\"img-fluid\" src=\"img/portfolio/submarine.png\" alt=\"\">\n                </a>\n            </div>\n        </div>\n    </div>\n</section>\n\n<!-- About Section -->\n<section class=\"bg-primary text-white mb-0\" id=\"about\">\n    <div class=\"container\">\n        <h2 class=\"text-center text-uppercase text-white\">About</h2>\n        <hr class=\"star-light mb-5\">\n        <div class=\"row\">\n            <div class=\"col-lg-4 ml-auto\">\n                <p class=\"lead\">Freelancer is a free bootstrap theme created by Start Bootstrap. The download includes\n                    the\n                    complete source files including HTML, CSS, and JavaScript as well as optional LESS stylesheets for\n                    easy\n                    customization.</p>\n            </div>\n            <div class=\"col-lg-4 mr-auto\">\n                <p class=\"lead\">Whether you're a student looking to showcase your work, a professional looking to\n                    attract\n                    clients, or a graphic artist looking to share your projects, this template is the perfect starting\n                    point!</p>\n            </div>\n        </div>\n        <div class=\"text-center mt-4\">\n            <a class=\"btn btn-xl btn-outline-light\" href=\"#\">\n                <i class=\"fas fa-download mr-2\"></i>\n                Download Now!\n            </a>\n        </div>\n    </div>\n</section>\n\n<!-- Contact Section -->\n<section id=\"contact\">\n    <div class=\"container\">\n        <h2 class=\"text-center text-uppercase text-secondary mb-0\">Contact Me</h2>\n        <hr class=\"star-dark mb-5\">\n        <div class=\"row\">\n            <div class=\"col-lg-8 mx-auto\">\n                <!-- To configure the contact form email address, go to mail/contact_me.php and update the email address in the PHP file on line 19. -->\n                <!-- The form should work on most web servers, but if the form is not working you may need to configure your web server differently. -->\n                <form name=\"sentMessage\" id=\"contactForm\" novalidate=\"novalidate\">\n                    <div class=\"control-group\">\n                        <div class=\"form-group floating-label-form-group controls mb-0 pb-2\">\n                            <label>Name</label>\n                            <input class=\"form-control\" id=\"name\" type=\"text\" placeholder=\"Name\" required=\"required\"\n                                data-validation-required-message=\"Please enter your name.\">\n                            <p class=\"help-block text-danger\"></p>\n                        </div>\n                    </div>\n                    <div class=\"control-group\">\n                        <div class=\"form-group floating-label-form-group controls mb-0 pb-2\">\n                            <label>Email Address</label>\n                            <input class=\"form-control\" id=\"email\" type=\"email\" placeholder=\"Email Address\"\n                                required=\"required\" data-validation-required-message=\"Please enter your email address.\">\n                            <p class=\"help-block text-danger\"></p>\n                        </div>\n                    </div>\n                    <div class=\"control-group\">\n                        <div class=\"form-group floating-label-form-group controls mb-0 pb-2\">\n                            <label>Phone Number</label>\n                            <input class=\"form-control\" id=\"phone\" type=\"tel\" placeholder=\"Phone Number\"\n                                required=\"required\" data-validation-required-message=\"Please enter your phone number.\">\n                            <p class=\"help-block text-danger\"></p>\n                        </div>\n                    </div>\n                    <div class=\"control-group\">\n                        <div class=\"form-group floating-label-form-group controls mb-0 pb-2\">\n                            <label>Message</label>\n                            <textarea class=\"form-control\" id=\"message\" rows=\"5\" placeholder=\"Message\"\n                                required=\"required\"\n                                data-validation-required-message=\"Please enter a message.\"></textarea>\n                            <p class=\"help-block text-danger\"></p>\n                        </div>\n                    </div>\n                    <br>\n                    <div id=\"success\"></div>\n                    <div class=\"form-group\">\n                        <button type=\"submit\" class=\"btn btn-primary btn-xl\" id=\"sendMessageButton\">Send</button>\n                    </div>\n                </form>\n            </div>\n        </div>\n    </div>\n</section>\n\n<!-- Footer -->\n<footer class=\"footer text-center\">\n    <div class=\"container\">\n        <div class=\"row\">\n            <div class=\"col-md-4 mb-5 mb-lg-0\">\n                <h4 class=\"text-uppercase mb-4\">Location</h4>\n                <p class=\"lead mb-0\">2215 John Daniel Drive\n                    <br>Clark, MO 65243</p>\n            </div>\n            <div class=\"col-md-4 mb-5 mb-lg-0\">\n                <h4 class=\"text-uppercase mb-4\">Around the Web</h4>\n                <ul class=\"list-inline mb-0\">\n                    <li class=\"list-inline-item\">\n                        <a class=\"btn btn-outline-light btn-social text-center rounded-circle\" href=\"#\">\n                            <i class=\"fab fa-fw fa-facebook-f\"></i>\n                        </a>\n                    </li>\n                    <li class=\"list-inline-item\">\n                        <a class=\"btn btn-outline-light btn-social text-center rounded-circle\" href=\"#\">\n                            <i class=\"fab fa-fw fa-google-plus-g\"></i>\n                        </a>\n                    </li>\n                    <li class=\"list-inline-item\">\n                        <a class=\"btn btn-outline-light btn-social text-center rounded-circle\" href=\"#\">\n                            <i class=\"fab fa-fw fa-twitter\"></i>\n                        </a>\n                    </li>\n                    <li class=\"list-inline-item\">\n                        <a class=\"btn btn-outline-light btn-social text-center rounded-circle\" href=\"#\">\n                            <i class=\"fab fa-fw fa-linkedin-in\"></i>\n                        </a>\n                    </li>\n                    <li class=\"list-inline-item\">\n                        <a class=\"btn btn-outline-light btn-social text-center rounded-circle\" href=\"#\">\n                            <i class=\"fab fa-fw fa-dribbble\"></i>\n                        </a>\n                    </li>\n                </ul>\n            </div>\n            <div class=\"col-md-4\">\n                <h4 class=\"text-uppercase mb-4\">About Freelancer</h4>\n                <p class=\"lead mb-0\">Freelance is a free to use, open source Bootstrap theme created by\n                    <a href=\"http://startbootstrap.com\">Start Bootstrap</a>.</p>\n            </div>\n        </div>\n    </div>\n</footer>\n\n<div class=\"copyright py-4 text-center text-white\">\n    <div class=\"container\">\n        <small>Copyright &copy; Your Website 2019</small>\n    </div>\n</div>\n\n<!-- Scroll to Top Button (Only visible on small and extra-small screen sizes) -->\n<div class=\"scroll-to-top d-lg-none position-fixed \">\n    <a class=\"js-scroll-trigger d-block text-center text-white rounded\" href=\"#page-top\">\n        <i class=\"fa fa-chevron-up\"></i>\n    </a>\n</div>\n\n<!-- Portfolio Modals -->\n\n<!-- Portfolio Modal 1 -->\n<div class=\"portfolio-modal mfp-hide\" id=\"portfolio-modal-1\">\n    <div class=\"portfolio-modal-dialog bg-white\">\n        <a class=\"close-button d-none d-md-block portfolio-modal-dismiss\" href=\"#\">\n            <i class=\"fa fa-3x fa-times\"></i>\n        </a>\n        <div class=\"container text-center\">\n            <div class=\"row\">\n                <div class=\"col-lg-8 mx-auto\">\n                    <h2 class=\"text-secondary text-uppercase mb-0\">Project Name</h2>\n                    <hr class=\"star-dark mb-5\">\n                    <img class=\"img-fluid mb-5\" src=\"img/portfolio/cabin.png\" alt=\"\">\n                    <p class=\"mb-5\">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Mollitia neque assumenda\n                        ipsam\n                        nihil, molestias magnam, recusandae quos quis inventore quisquam velit asperiores, vitae?\n                        Reprehenderit\n                        soluta, eos quod consequuntur itaque. Nam.</p>\n                    <a class=\"btn btn-primary btn-lg rounded-pill portfolio-modal-dismiss\" href=\"#\">\n                        <i class=\"fa fa-close\"></i>\n                        Close Project</a>\n                </div>\n            </div>\n        </div>\n    </div>\n</div>\n\n<!-- Portfolio Modal 2 -->\n<div class=\"portfolio-modal mfp-hide\" id=\"portfolio-modal-2\">\n    <div class=\"portfolio-modal-dialog bg-white\">\n        <a class=\"close-button d-none d-md-block portfolio-modal-dismiss\" href=\"#\">\n            <i class=\"fa fa-3x fa-times\"></i>\n        </a>\n        <div class=\"container text-center\">\n            <div class=\"row\">\n                <div class=\"col-lg-8 mx-auto\">\n                    <h2 class=\"text-secondary text-uppercase mb-0\">Project Name</h2>\n                    <hr class=\"star-dark mb-5\">\n                    <img class=\"img-fluid mb-5\" src=\"img/portfolio/cake.png\" alt=\"\">\n                    <p class=\"mb-5\">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Mollitia neque assumenda\n                        ipsam\n                        nihil, molestias magnam, recusandae quos quis inventore quisquam velit asperiores, vitae?\n                        Reprehenderit\n                        soluta, eos quod consequuntur itaque. Nam.</p>\n                    <a class=\"btn btn-primary btn-lg rounded-pill portfolio-modal-dismiss\" href=\"#\">\n                        <i class=\"fa fa-close\"></i>\n                        Close Project</a>\n                </div>\n            </div>\n        </div>\n    </div>\n</div>\n\n<!-- Portfolio Modal 3 -->\n<div class=\"portfolio-modal mfp-hide\" id=\"portfolio-modal-3\">\n    <div class=\"portfolio-modal-dialog bg-white\">\n        <a class=\"close-button d-none d-md-block portfolio-modal-dismiss\" href=\"#\">\n            <i class=\"fa fa-3x fa-times\"></i>\n        </a>\n        <div class=\"container text-center\">\n            <div class=\"row\">\n                <div class=\"col-lg-8 mx-auto\">\n                    <h2 class=\"text-secondary text-uppercase mb-0\">Project Name</h2>\n                    <hr class=\"star-dark mb-5\">\n                    <img class=\"img-fluid mb-5\" src=\"img/portfolio/circus.png\" alt=\"\">\n                    <p class=\"mb-5\">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Mollitia neque assumenda\n                        ipsam\n                        nihil, molestias magnam, recusandae quos quis inventore quisquam velit asperiores, vitae?\n                        Reprehenderit\n                        soluta, eos quod consequuntur itaque. Nam.</p>\n                    <a class=\"btn btn-primary btn-lg rounded-pill portfolio-modal-dismiss\" href=\"#\">\n                        <i class=\"fa fa-close\"></i>\n                        Close Project</a>\n                </div>\n            </div>\n        </div>\n    </div>\n</div>\n\n<!-- Portfolio Modal 4 -->\n<div class=\"portfolio-modal mfp-hide\" id=\"portfolio-modal-4\">\n    <div class=\"portfolio-modal-dialog bg-white\">\n        <a class=\"close-button d-none d-md-block portfolio-modal-dismiss\" href=\"#\">\n            <i class=\"fa fa-3x fa-times\"></i>\n        </a>\n        <div class=\"container text-center\">\n            <div class=\"row\">\n                <div class=\"col-lg-8 mx-auto\">\n                    <h2 class=\"text-secondary text-uppercase mb-0\">Project Name</h2>\n                    <hr class=\"star-dark mb-5\">\n                    <img class=\"img-fluid mb-5\" src=\"img/portfolio/game.png\" alt=\"\">\n                    <p class=\"mb-5\">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Mollitia neque assumenda\n                        ipsam\n                        nihil, molestias magnam, recusandae quos quis inventore quisquam velit asperiores, vitae?\n                        Reprehenderit\n                        soluta, eos quod consequuntur itaque. Nam.</p>\n                    <a class=\"btn btn-primary btn-lg rounded-pill portfolio-modal-dismiss\" href=\"#\">\n                        <i class=\"fa fa-close\"></i>\n                        Close Project</a>\n                </div>\n            </div>\n        </div>\n    </div>\n</div>\n\n<!-- Portfolio Modal 5 -->\n<div class=\"portfolio-modal mfp-hide\" id=\"portfolio-modal-5\">\n    <div class=\"portfolio-modal-dialog bg-white\">\n        <a class=\"close-button d-none d-md-block portfolio-modal-dismiss\" href=\"#\">\n            <i class=\"fa fa-3x fa-times\"></i>\n        </a>\n        <div class=\"container text-center\">\n            <div class=\"row\">\n                <div class=\"col-lg-8 mx-auto\">\n                    <h2 class=\"text-secondary text-uppercase mb-0\">Project Name</h2>\n                    <hr class=\"star-dark mb-5\">\n                    <img class=\"img-fluid mb-5\" src=\"img/portfolio/safe.png\" alt=\"\">\n                    <p class=\"mb-5\">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Mollitia neque assumenda\n                        ipsam\n                        nihil, molestias magnam, recusandae quos quis inventore quisquam velit asperiores, vitae?\n                        Reprehenderit\n                        soluta, eos quod consequuntur itaque. Nam.</p>\n                    <a class=\"btn btn-primary btn-lg rounded-pill portfolio-modal-dismiss\" href=\"#\">\n                        <i class=\"fa fa-close\"></i>\n                        Close Project</a>\n                </div>\n            </div>\n        </div>\n    </div>\n</div>\n\n<!-- Portfolio Modal 6 -->\n<div class=\"portfolio-modal mfp-hide\" id=\"portfolio-modal-6\">\n    <div class=\"portfolio-modal-dialog bg-white\">\n        <a class=\"close-button d-none d-md-block portfolio-modal-dismiss\" href=\"#\">\n            <i class=\"fa fa-3x fa-times\"></i>\n        </a>\n        <div class=\"container text-center\">\n            <div class=\"row\">\n                <div class=\"col-lg-8 mx-auto\">\n                    <h2 class=\"text-secondary text-uppercase mb-0\">Project Name</h2>\n                    <hr class=\"star-dark mb-5\">\n                    <img class=\"img-fluid mb-5\" src=\"img/portfolio/submarine.png\" alt=\"\">\n                    <p class=\"mb-5\">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Mollitia neque assumenda\n                        ipsam\n                        nihil, molestias magnam, recusandae quos quis inventore quisquam velit asperiores, vitae?\n                        Reprehenderit\n                        soluta, eos quod consequuntur itaque. Nam.</p>\n                    <a class=\"btn btn-primary btn-lg rounded-pill portfolio-modal-dismiss\" href=\"#\">\n                        <i class=\"fa fa-close\"></i>\n                        Close Project</a>\n                </div>\n            </div>\n        </div>\n    </div>\n</div>"

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
        this.title = 'WebClient';
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

module.exports = __webpack_require__(/*! D:\Projects\RewriteMeWeb\WebClient\src\main.ts */"./src/main.ts");


/***/ })

},[[0,"runtime","vendor"]]]);
//# sourceMappingURL=main.js.map