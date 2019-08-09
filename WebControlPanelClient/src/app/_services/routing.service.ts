import { Injectable, isDevMode } from '@angular/core';
import { CommonVariables } from '../_config/common-variables';

@Injectable({
    providedIn: 'root'
})
export class RoutingService {
    constructor() { }

    getAuthenticateUri(): string {
        return this.getApiUri() + "api/authenticate/";
    }

    getUsersUri(): string {
        return this.getApiUri() + "api/control-panel/users/";
    }

    getPurchasesUri(): string {
        return this.getApiUri() + "api/control-panel/purchases/";
    }

    getSubscriptionsUri(): string {
        return this.getApiUri() + "api/control-panel/subscriptions/";
    }

    getCreateSubscriptionUri(): string {
        return this.getApiUri() + "api/control-panel/subscriptions/create/";
    }

    getAdministratorsUri(): string {
        return this.getApiUri() + "api/control-panel/administrators/";
    }

    getCreateAdministratorUri(): string {
        return this.getApiUri() + "api/control-panel/administrators/create/";
    }

    getUpdateAdministratorUri(): string {
        return this.getApiUri() + "api/control-panel/administrators/update/";
    }

    getDeleteAdministratorUri(): string {
        return this.getApiUri() + "api/control-panel/administrators/delete/";
    }

    getContactFormsUri(): string {
        return this.getApiUri() + "api/control-panel/contact-forms/";
    }

    private getApiUri(): string {
        if (isDevMode()) {
            return CommonVariables.ApiUriDevelopment;
        }

        return CommonVariables.ApiUriProduction;
    }
}
