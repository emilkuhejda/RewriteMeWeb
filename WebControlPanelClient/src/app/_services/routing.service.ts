import { Injectable, isDevMode } from '@angular/core';
import { CommonVariables } from '../_config/common-variables';

@Injectable({
    providedIn: 'root'
})
export class RoutingService {
    constructor() { }

    getHangfireUri() {
        return this.getApiUri() + "hangfire/"
    }

    getAuthenticateUri(): string {
        return this.getApiUri() + "api/authenticate/";
    }

    getUsersUri(): string {
        return this.getApiUri() + "api/control-panel/users/";
    }

    getPurchasesUri(): string {
        return this.getApiUri() + "api/control-panel/purchases/";
    }

    getPurchaseUri(): string {
        return this.getApiUri() + "api/control-panel/purchases/detail/";
    }

    getSubscriptionsUri(): string {
        return this.getApiUri() + "api/control-panel/subscriptions/";
    }

    getCreateSubscriptionUri(): string {
        return this.getApiUri() + "api/control-panel/subscriptions/create/";
    }

    getFileItemsUri(): string {
        return this.getApiUri() + "api/control-panel/files/";
    }

    getRestoreFileItemUri(): string {
        return this.getApiUri() + "api/control-panel/files/restore/";
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

    getInformationMessagesUri(): string {
        return this.getApiUri() + "api/control-panel/information-messages/";
    }

    getCreateInformationMessageUri(): string {
        return this.getApiUri() + "api/control-panel/information-messages/create/";
    }

    getSendNotificationUri(): string {
        return this.getApiUri() + "api/control-panel/information-messages/send/";
    }

    getGenerateHangfireAccessUri(): string {
        return this.getApiUri() + "api/generate-hangfire-access/";
    }

    getHasAccessUri(): string {
        return this.getApiUri() + "api/control-panel/has-access/";
    }

    private getApiUri(): string {
        if (isDevMode()) {
            return CommonVariables.ApiUriDevelopment;
        }

        return CommonVariables.ApiUriProduction;
    }
}
