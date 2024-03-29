import { Injectable } from '@angular/core';
import * as Msal from 'msal';
import { CommonVariables } from '../_config/common-variables';
import { Router } from '@angular/router';

@Injectable({
    providedIn: 'root'
})
export class MsalService {
    public static passwordResetError: string = "AADB2C90118";
    public static undefinedError: string = "0";

    private tenantConfig = {
        tenant: "voicipher.onmicrosoft.com",
        clientID: '3f16cd47-52fe-44e6-96b3-131e1e57b09c',
        signUpSignIn: "B2C_1_Voicipher_SignUp_SignIn",
        passwordReset: "B2C_1_Voicipher_Password_Reset",
        authorityBase: "https://login.microsoftonline.com/tfp/",
        b2cScopes: ["https://voicipher.onmicrosoft.com/access-api/user_impersonation"]
    };

    private authority = this.tenantConfig.authorityBase + this.tenantConfig.tenant + "/" + this.tenantConfig.signUpSignIn;

    constructor(private router: Router) { }

    private clientApplication = new Msal.UserAgentApplication(
        this.tenantConfig.clientID,
        this.authority,
        function (errorDesc: any, token: any) {
            localStorage.removeItem(CommonVariables.B2CAccessErrorKey);

            if (errorDesc !== undefined) {
                if (errorDesc.includes(MsalService.passwordResetError)) {
                    localStorage.setItem(CommonVariables.B2CAccessErrorKey, MsalService.passwordResetError);
                    return false;
                }

                localStorage.setItem(CommonVariables.B2CAccessErrorKey, MsalService.undefinedError);
                return false;
            }

            if (token === undefined) {
                localStorage.setItem(CommonVariables.B2CAccessErrorKey, MsalService.undefinedError);
                return false;
            }

            localStorage.setItem(CommonVariables.B2CAccessTokenKey, token);
            return true;
        },
        {
            cacheLocation: 'localStorage',
            redirectUri: CommonVariables.LoginRedirectUri,
            navigateToLoginRequestUrl: false
        }
    );

    public loginOrRedirect(): void {
        let error = localStorage.getItem(CommonVariables.B2CAccessErrorKey);
        localStorage.removeItem(CommonVariables.B2CAccessErrorKey);

        if (error === MsalService.passwordResetError) {
            this.passwordReset();
            return;
        }

        if (error === MsalService.undefinedError) {
            this.router.navigate(['/']);
            return;
        }

        this.login();
    }

    public login(): void {
        this.clientApplication.authority = this.tenantConfig.authorityBase + this.tenantConfig.tenant + "/" + this.tenantConfig.signUpSignIn;
        this.authenticate();
    }

    private passwordReset(): void {
        this.clientApplication.authority = this.tenantConfig.authorityBase + this.tenantConfig.tenant + "/" + this.tenantConfig.passwordReset;
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

    saveToken(token: string) {
        localStorage.setItem(CommonVariables.AccessTokenKey, token);
    }

    saveCurrentIdentity(identity: any) {
        localStorage.setItem(CommonVariables.CurrentIdentity, identity);
    }

    getUserEmail(): string {
        return this.getUser().idToken['emails'][0];
    }

    getUserId(): string {
        return this.getUser().idToken['oid'];
    }

    getGivenName(): string {
        return this.decode(this.getUser().idToken['given_name']);
    }

    getFamilyName(): string {
        return this.decode(this.getUser().idToken['family_name']);
    }

    getB2CToken() {
        return localStorage.getItem(CommonVariables.B2CAccessTokenKey);
    }

    private getUser() {
        return this.clientApplication.getUser();
    }

    private decode(text: string) {
        return decodeURIComponent(escape(text));
    }
}
