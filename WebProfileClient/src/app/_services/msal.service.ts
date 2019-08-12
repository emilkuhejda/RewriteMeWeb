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
        editProfile: "B2C_1_RewriteMe_Edit",
        passwordReset: "B2C_1_RewriteMe_Password_Reset",
        authorityBase: "https://login.microsoftonline.com/tfp/",
        b2cScopes: ["https://rewriteme.onmicrosoft.com/access-api/user_impersonation"]
    };

    private authority = this.tenantConfig.authorityBase + this.tenantConfig.tenant + "/" + this.tenantConfig.signUpSignIn;

    private clientApplication = new Msal.UserAgentApplication(
        this.tenantConfig.clientID,
        this.authority,
        function (errorDesc: any, token: any, error: any, tokenType: any) {
            if (errorDesc !== undefined)
                return;

            if (token === undefined)
                return;

            localStorage.setItem(CommonVariables.B2CSuccessCallbackToken, token);
        },
        { cacheLocation: 'localStorage' }
    );

    constructor() { }

    editProfile() {
        this.clientApplication.authority = this.tenantConfig.authorityBase + this.tenantConfig.tenant + "/" + this.tenantConfig.editProfile;
        this.clientApplication.loginRedirect(this.tenantConfig.b2cScopes);
    }

    resetPassword() {
        this.clientApplication.authority = this.tenantConfig.authorityBase + this.tenantConfig.tenant + "/" + this.tenantConfig.passwordReset;
        this.clientApplication.loginRedirect(this.tenantConfig.b2cScopes);
    }

    getCallbackToken() {
        var token = localStorage.getItem(CommonVariables.B2CSuccessCallbackToken);
        localStorage.removeItem(CommonVariables.B2CSuccessCallbackToken);
        return token;
    }

    logout(): void {
        this.clientApplication.logout();
        localStorage.clear();
    }

    isLoggedIn(): boolean {
        let token = localStorage.getItem(CommonVariables.AccessTokenKey);
        return this.clientApplication.getUser() != null && token != null;
    }

    getMsalGivenName() {
        return this.clientApplication.getUser().idToken['given_name'];
    }

    getMsalFamilyName() {
        return this.clientApplication.getUser().idToken['family_name'];
    }

    getIdentityUserName(): string {
        let identity = this.getIdentity();
        return `${identity.givenName} ${identity.familyName}`;
    }

    getToken() {
        return localStorage.getItem(CommonVariables.AccessTokenKey);
    }

    saveCurrentIdentity(identity: Identity) {
        localStorage.setItem(CommonVariables.CurrentIdentity, JSON.stringify(identity));
    }

    getIdentity(): Identity {
        return JSON.parse(localStorage.getItem(CommonVariables.CurrentIdentity));
    }
}
