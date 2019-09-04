import { LanguageVersion } from './language-version';

export class InformationMessage {
    id: string;
    userId: string;
    campaignName: string;
    dateCreated: string;
    languageVersions: LanguageVersion[];
}
