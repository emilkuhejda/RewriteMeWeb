import { LanguageVersion } from './language-version';
import { Language } from '../_enums/language';

export class InformationMessage {
    id: string;
    wasOpened: boolean;
    dateUpdated: string;
    datePublished: string;
    languageVersions: LanguageVersion[];

    getTitle(language: Language): string {
        var languageVersion = this.languageVersions.find(x => x.language == language);
        if (languageVersion === undefined) {
            return "";
        }

        return languageVersion.title;
    }

    getMessage(language: Language): string {
        var languageVersion = this.languageVersions.find(x => x.language == language);
        if (languageVersion === undefined) {
            return "";
        }

        return languageVersion.message;
    }
}
