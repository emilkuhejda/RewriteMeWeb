import { LanguageVersion } from './language-version';

export class InformationMessage {
    id: string;
    userId: string;
    campaignName: string;
    wasOpened: boolean;
    dateCreated: string;
    dateUpdated: string;
    datePublished: string;
    languageVersions: LanguageVersion[];
}
