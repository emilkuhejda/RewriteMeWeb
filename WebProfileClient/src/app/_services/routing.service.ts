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
        return this.getApiUri() + "api/files/";
    }

    getTranscribeItemsUri(): string {
        return this.getApiUri() + "api/transcribe-items/";
    }

    getUpdateTranscriptUri(): string {
        return this.getApiUri() + "api/transcribe-items/update-transcript/";
    }

    getTranscribeAudioUri(): string {
        return this.getApiUri() + "api/transcribe-items/audio-stream/";
    }

    getUploadFileItemUri(): string {
        return this.getApiUri() + "api/files/upload/";
    }

    getUpdateFileItemUri(): string {
        return this.getApiUri() + "api/files/update/";
    }

    getDeleteFileItemUri(): string {
        return this.getApiUri() + "api/files/delete/";
    }

    getTranscribeFileItemUri(): string {
        return this.getApiUri() + "api/files/transcribe/";
    }

    getUpdateUserUri(): string {
        return this.getApiUri() + "api/users/update/";
    }

    getSubscriptionRemainingTimeUri(): string {
        return this.getApiUri() + "api/subscriptions/remaining-time/";
    }

    private getApiUri(): string {
        if (isDevMode()) {
            return CommonVariables.ApiUriDevelopment;
        }

        return CommonVariables.ApiUriProduction;
    }
}
