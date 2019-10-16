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

    getDeleteUserUri(): string {
        return this.getApiUri() + "api/control-panel/users/delete/";
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

    getDetailFileItemsUri(): string {
        return this.getApiUri() + "api/control-panel/files/detail/";
    }

    getRestoreFileItemUri(): string {
        return this.getApiUri() + "api/control-panel/files/restore/";
    }

    getUpdateRecognitionStateUri(): string {
        return this.getApiUri() + "api/control-panel/files/update-recognition-state/";
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
        return this.getApiUri() + "api/control-panel/generate-hangfire-access/";
    }

    getHasAccessUri(): string {
        return this.getApiUri() + "api/control-panel/has-access/";
    }

    getIsDeploymentSuccessfulUri(): string {
        return this.getApiUri() + "api/control-panel/is-deployment-successful/";
    }

    getResetDatabaseUri(): string {
        return this.getApiUri() + "api/control-panel/reset-database/";
    }

    getChangeStorageUri(): string {
        return this.getApiUri() + "api/control-panel/settings/change-storage/";
    }

    getCleanUpUri(): string {
        return this.getApiUri() + "api/control-panel/settings/clean-up/";
    }

    private getApiUri(): string {
        if (isDevMode()) {
            return CommonVariables.ApiUriDevelopment;
        }

        return CommonVariables.ApiUriProduction;
    }
}
