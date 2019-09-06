import { Language } from '../_enums/language';

export class LanguageVersion {
    id: string;
    informationMessageId: string;
    title: string;
    message: string;
    description: string;
    language: Language;
    sentOnOsx: boolean;
    sentOnAndroid: boolean;
}
