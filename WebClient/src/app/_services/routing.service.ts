import { Injectable, isDevMode } from '@angular/core';
import { CommonVariables } from '../_config/common-variables';

@Injectable({
    providedIn: 'root'
})
export class RoutingService {
    constructor() { }

    getRegisterUserUri(): string {
        return this.getApiUri() + "api/b2c/v1/users/register/";
    }

    getCreateContactFormUri(): string {
        return this.getApiUri() + "api/v1/contact-form/create/";
    }

    getProfileUri(): string {
        return this.getApiUri() + "profile/";
    }

    private getApiUri(): string {
        if (isDevMode()) {
            return CommonVariables.ApiUriDevelopment;
        }

        return CommonVariables.ApiUriProduction;
    }
}
