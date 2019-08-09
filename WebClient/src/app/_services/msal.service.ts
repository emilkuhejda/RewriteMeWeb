import { Injectable } from '@angular/core';
import * as Msal from 'msal';
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

    constructor() { }

    private clientApplication = new Msal.UserAgentApplication(
        this.tenantConfig.clientID,
        this.authority,
        function (errorDesc: any, token: any, error: any, tokenType: any) {
            if (errorDesc !== undefined)
                return;

            if (token === undefined)
                return;

            localStorage.setItem(CommonVariables.B2CTodoAccessTokenKey, token);
        },
        {
            cacheLocation: 'localStorage',
            redirectUri: CommonVariables.LoginRedirectUri,
            navigateToLoginRequestUrl: false
        }
    );

    public login(): void {
        this.clientApplication.authority = "https://login.microsoftonline.com/tfp/" + this.tenantConfig.tenant + "/" + this.tenantConfig.signUpSignIn;
        this.authenticate();
    }

    private authenticate(): void {
        this.clientApplication.loginRedirect(this.tenantConfig.b2cScopes);
    }

    logout(): void {
        this.clientApplication.logout();
        localStorage.clear();
    };

    isLoggedIn(): boolean {
        return this.clientApplication.getUser() != null;
    };

    getUserEmail(): string {
        return this.getUser().idToken['emails'][0];
    }

    getUserId(): string {
        return this.getUser().idToken['oid'];
    }

    getGivenName(): string {
        return this.getUser().idToken['given_name'];
    }

    getFamilyName(): string {
        return this.getUser().idToken['family_name'];
    }

    private getUser() {
        return this.clientApplication.getUser();
    }
}
