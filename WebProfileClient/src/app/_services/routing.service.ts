import { Injectable, isDevMode } from '@angular/core';
import { CommonVariables } from '../_config/common-variables';

@Injectable({
    providedIn: 'root'
})
export class RoutingService {
    constructor() { }

    getHomeUri(): string {
        return this.getApiUri() + "home/";
    }

    getFileItemsUri(): string {
        return this.getApiUri() + "api/v1/files/";
    }

    getTemporaryDeletedFileItemsUri(): string {
        return this.getApiUri() + "api/v1/files/temporary-deleted/";
    }

    getPermanentDeleteAll(): string {
        return this.getApiUri() + "api/v1/files/permanent-delete-all/";
    }

    getRestoreAllUri(): string {
        return this.getApiUri() + "api/v1/files/restore-all/";
    }

    getTranscribeItemsUri(): string {
        return this.getApiUri() + "api/v1/transcribe-items/";
    }

    getUpdateTranscriptUri(): string {
        return this.getApiUri() + "api/v1/transcribe-items/update-transcript/";
    }

    getTranscribeAudioUri(): string {
        return this.getApiUri() + "api/v1/transcribe-items/audio-stream/";
    }

    getUploadFileItemUri(): string {
        return this.getApiUri() + "api/v1/files/upload/";
    }

    getUpdateFileItemUri(): string {
        return this.getApiUri() + "api/v1/files/update/";
    }

    getDeleteFileItemUri(): string {
        return this.getApiUri() + "api/v1/files/delete/";
    }

    getTranscribeFileItemUri(): string {
        return this.getApiUri() + "api/v1/files/transcribe/";
    }

    getUpdateUserUri(): string {
        return this.getApiUri() + "api/v1/users/update/";
    }

    getSubscriptionRemainingTimeUri(): string {
        return this.getApiUri() + "api/v1/subscriptions/remaining-time/";
    }

    getInformationMessagesUri(): string {
        return this.getApiUri() + "api/v1/information-messages/";
    }

    getMarkMessageAsOpenedUri(): string {
        return this.getApiUri() + "api/v1/information-messages/mark-as-opened/";
    }

    getCacheUri(): string {
        return this.getApiUri() + "api/cache/";
    }

    private getApiUri(): string {
        if (isDevMode()) {
            return CommonVariables.ApiUriDevelopment;
        }

        return CommonVariables.ApiUriProduction;
    }
}
