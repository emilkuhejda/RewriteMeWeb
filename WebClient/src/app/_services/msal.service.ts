import { Injectable } from '@angular/core';
import * as Msal from 'msal';
import { CommonVariables } from '../_config/common-variables';
import { UserService } from './user.service';

@Injectable({
    providedIn: 'root'
})
export class MsalService {
    private B2CTodoAccessTokenKey = "b2c.access.token";

    private tenantConfig = {
        tenant: "rewriteme.onmicrosoft.com",
        clientID: '94983a85-6f54-4940-849e-55eaeb1d89dd',
        signUpSignIn: "B2C_1_RewriteMe_SignUp_SignIn",
        b2cScopes: ["https://rewriteme.onmicrosoft.com/access-api/user_impersonation"]
    };

    private authority = "https://login.microsoftonline.com/tfp/" + this.tenantConfig.tenant + "/" + this.tenantConfig.signUpSignIn;

    constructor(private userService: UserService) { }

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

    private authenticate(): void {
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
                        });
                })
        }, function (error: any) {
        });
    }

    private saveAccessTokenToCache(accessToken: string): void {
        localStorage.setItem(this.B2CTodoAccessTokenKey, accessToken);
        this.register();
    }

    private register(): void {
        if (!this.isLoggedIn())
            return;

        let token = this.getUser().idToken;
        let userData = {
            id: token['oid'],
            ApplicationId: CommonVariables.ApplicationId,
            Email: this.getUserEmail(),
            GivenName: token['given_name'],
            FamilyName: token['family_name']
        };

        this.userService.register(userData).subscribe();
    }

    logout(): void {
        this.clientApplication.logout();
    };

    isLoggedIn(): boolean {
        return this.clientApplication.getUser() != null;
    };

    getUserEmail(): string {
        return this.getUser().idToken['emails'][0];
    }

    getUser() {
        return this.clientApplication.getUser()
    }

    getToken() {
        return localStorage.getItem(this.B2CTodoAccessTokenKey);
    }
}
