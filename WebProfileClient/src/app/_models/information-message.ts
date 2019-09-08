import { LanguageVersion } from './language-version';
import { CommonVariables } from '../_config/common-variables';

export class InformationMessage {
    id: string;
    wasOpened: boolean;
    dateUpdated: string;
    datePublished: string;
    languageVersions: LanguageVersion[];

    getTitle(): string {
        var languageVersion = this.languageVersions.find(x => x.language == CommonVariables.DefaultLanguage);
        if (languageVersion === undefined) {
            return "";
        }

        return languageVersion.title;
    }

    getMessage(): string {
        var languageVersion = this.languageVersions.find(x => x.language == CommonVariables.DefaultLanguage);
        if (languageVersion === undefined) {
            return "";
        }

        return languageVersion.message;
    }
}
