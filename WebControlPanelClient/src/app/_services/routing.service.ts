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
        return this.getApiUri() + "api/v1/authenticate/";
    }

    getUsersUri(): string {
        return this.getApiUri() + "api/v1/control-panel/users/";
    }

    getDeleteUserUri(): string {
        return this.getApiUri() + "api/v1/control-panel/users/delete/";
    }

    getDeletedAccountsUri(): string {
        return this.getApiUri() + "api/v1/control-panel/deleted-accounts/";
    }

    getPurchasesUri(): string {
        return this.getApiUri() + "api/v1/control-panel/purchases/";
    }

    getPurchaseUri(): string {
        return this.getApiUri() + "api/v1/control-panel/purchases/detail/";
    }

    getSubscriptionsUri(): string {
        return this.getApiUri() + "api/v1/control-panel/subscriptions/";
    }

    getCreateSubscriptionUri(): string {
        return this.getApiUri() + "api/v1/control-panel/subscriptions/create/";
    }

    getFileItemsUri(): string {
        return this.getApiUri() + "api/v1/control-panel/files/";
    }

    getDetailFileItemsUri(): string {
        return this.getApiUri() + "api/v1/control-panel/files/detail/";
    }

    getRestoreFileItemUri(): string {
        return this.getApiUri() + "api/v1/control-panel/files/restore/";
    }

    getUpdateRecognitionStateUri(): string {
        return this.getApiUri() + "api/v1/control-panel/files/update-recognition-state/";
    }

    getAdministratorsUri(): string {
        return this.getApiUri() + "api/v1/control-panel/administrators/";
    }

    getCreateAdministratorUri(): string {
        return this.getApiUri() + "api/v1/control-panel/administrators/create/";
    }

    getUpdateAdministratorUri(): string {
        return this.getApiUri() + "api/v1/control-panel/administrators/update/";
    }

    getDeleteAdministratorUri(): string {
        return this.getApiUri() + "api/v1/control-panel/administrators/delete/";
    }

    getContactFormsUri(): string {
        return this.getApiUri() + "api/v1/control-panel/contact-forms/";
    }

    getInformationMessagesUri(): string {
        return this.getApiUri() + "api/v1/control-panel/information-messages/";
    }

    getCreateInformationMessageUri(): string {
        return this.getApiUri() + "api/v1/control-panel/information-messages/create/";
    }

    getSendNotificationUri(): string {
        return this.getApiUri() + "api/v1/control-panel/information-messages/send/";
    }

    getHasAccessUri(): string {
        return this.getApiUri() + "api/v1/control-panel/utils/has-access/";
    }

    getIsDeploymentSuccessfulUri(): string {
        return this.getApiUri() + "api/v1/control-panel/utils/is-deployment-successful/";
    }

    getGenerateHangfireAccessUri(): string {
        return this.getApiUri() + "api/v1/control-panel/utils/generate-hangfire-access/";
    }

    getResetDatabaseUri(): string {
        return this.getApiUri() + "api/v1/control-panel/utils/reset-database/";
    }

    getDeleteDatabaseUri(): string {
        return this.getApiUri() + "api/v1/control-panel/utils/delete-database/";
    }

    getStorageSettingUri(): string {
        return this.getApiUri() + "api/v1/control-panel/settings/storage-setting/";
    }

    getChangeStorageUri(): string {
        return this.getApiUri() + "api/v1/control-panel/settings/change-storage/";
    }

    getDatabaseBackupSettingUri(): string {
        return this.getApiUri() + "api/v1/control-panel/settings/database-backup/";
    }

    getChangeDatabaseBackupSettingsUri(): string {
        return this.getApiUri() + "api/v1/control-panel/settings/change-database-backup/";
    }

    getNotificationsSettingUri(): string {
        return this.getApiUri() + "api/v1/control-panel/settings/notifications-setting/";
    }

    getChangeNotificationsSettingUri(): string {
        return this.getApiUri() + "api/v1/control-panel/settings/change-notifications-setting/";
    }

    getChunksStorageSettingUri(): string {
        return this.getApiUri() + "api/v1/control-panel/settings/chunks-storage-setting/";
    }

    getChangeChunksStorageUri(): string {
        return this.getApiUri() + "api/v1/control-panel/settings/change-chunks-storage/";
    }

    getCleanUpUri(): string {
        return this.getApiUri() + "api/v1/control-panel/settings/clean-up/";
    }

    getCleanOutdatedChunksUri(): string {
        return this.getApiUri() + "api/v1/control-panel/settings/clean-chunks/";
    }

    getMigrateUri(): string {
        return this.getApiUri() + "api/v1/control-panel/azure-storage/migrate/";
    }

    private getApiUri(): string {
        if (isDevMode()) {
            return CommonVariables.ApiUriDevelopment;
        }

        return CommonVariables.ApiUriProduction;
    }
}
