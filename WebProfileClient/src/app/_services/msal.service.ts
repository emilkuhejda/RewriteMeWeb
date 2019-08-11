import { Injectable } from '@angular/core';
import * as Msal from 'msal';
import { Router } from '@angular/router';

@Injectable({
    providedIn: 'root'
})
export class MsalService {
    private AccessTokenKey = "access.token";

    private tenantConfig = {
        tenant: "rewriteme.onmicrosoft.com",
        clientID: '94983a85-6f54-4940-849e-55eaeb1d89dd',
        signUpSignIn: "B2C_1_RewriteMe_SignUp_SignIn",
        b2cScopes: ["https://rewriteme.onmicrosoft.com/access-api/user_impersonation"]
    };

    private authority = "https://login.microsoftonline.com/tfp/" + this.tenantConfig.tenant + "/" + this.tenantConfig.signUpSignIn;

    constructor(private router: Router) { }

    private clientApplication = new Msal.UserAgentApplication(
        this.tenantConfig.clientID,
        this.authority,
        function (errorDesc: any, token: any, error: any, tokenType: any) { },
        { cacheLocation: 'localStorage' }
    );

    public login(): void {
        this.clientApplication.authority = "https://login.microsoftonline.com/tfp/" + this.tenantConfig.tenant + "/" + this.tenantConfig.signUpSignIn;
        this.authenticate();
    }

    public authenticate(): void {
        var _this = this;
        this.clientApplication.loginPopup(this.tenantConfig.b2cScopes).then(function (idToken: any) {
            _this.clientApplication.acquireTokenSilent(_this.tenantConfig.b2cScopes).then(
                function (accessToken: any) {
                    _this.saveAccessTokenToCache(accessToken);
                }, function (error: any) {
                    _this.clientApplication.acquireTokenPopup(_this.tenantConfig.b2cScopes).then(
                        function (accessToken: any) {
                            _this.saveAccessTokenToCache(accessToken);
                        }, function (error: any) {
                            console.log("error: ", error);
                        });
                })
        }, function (error: any) {
            console.log("error: ", error);
        });
    }

    saveAccessTokenToCache(accessToken: string): void {
        localStorage.setItem(this.AccessTokenKey, accessToken);
    }

    logout(): void {
        this.clientApplication.logout();
        localStorage.clear();
    }

    isLoggedIn(): boolean {
        let token = localStorage.getItem(this.AccessTokenKey);
        return this.clientApplication.getUser() != null && token != null;
    }

    getUserEmail(): string {
        return this.getUser().idToken['emails'][0];
    }

    getUserName(): string {
        return `${this.getUser().idToken['given_name']} ${this.getUser().idToken['family_name']}`;
    }

    getUser() {
        return this.clientApplication.getUser();
    }

    getToken() {
        return localStorage.getItem(this.AccessTokenKey);
    }
}
