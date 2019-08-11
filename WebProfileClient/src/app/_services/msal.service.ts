import { Injectable } from '@angular/core';
import * as Msal from 'msal';
import { Identity } from '../_models/identity';
import { CommonVariables } from '../_config/common-variables';

@Injectable({
    providedIn: 'root'
})
export class MsalService {
    private tenantConfig = {
        tenant: "rewriteme.onmicrosoft.com",
        clientID: '94983a85-6f54-4940-849e-55eaeb1d89dd',
        signUpSignIn: "B2C_1_RewriteMe_SignUp_SignIn",
        b2cScopes: ["https://rewriteme.onmicrosoft.com/access-api/user_impersonation"]
    };

    private authority = "https://login.microsoftonline.com/tfp/" + this.tenantConfig.tenant + "/" + this.tenantConfig.signUpSignIn;

    private clientApplication = new Msal.UserAgentApplication(
        this.tenantConfig.clientID,
        this.authority,
        function (errorDesc: any, token: any, error: any, tokenType: any) { },
        { cacheLocation: 'localStorage' }
    );

    constructor() { }

    logout(): void {
        this.clientApplication.logout();
        localStorage.clear();
    }

    isLoggedIn(): boolean {
        let token = localStorage.getItem(CommonVariables.AccessTokenKey);
        return this.clientApplication.getUser() != null && token != null;
    }

    getUserName(): string {
        let identity = this.getIdentity();
        return `${identity.givenName} ${identity.familyName}`;
    }

    getToken() {
        return localStorage.getItem(CommonVariables.AccessTokenKey);
    }

    getIdentity(): Identity {
        return JSON.parse(localStorage.getItem(CommonVariables.CurrentIdentity));
    }
}
