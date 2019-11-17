import { LanguageVersion } from './language-version';

export class InformationMessage {
    id: string;
    userId: string;
    campaignName: string;
    wasOpened: boolean;
    dateCreated: Date;
    dateUpdated: Date;
    datePublished: Date;
    languageVersions: LanguageVersion[];
}
