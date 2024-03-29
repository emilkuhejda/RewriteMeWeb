import { Language } from '../_enums/language';
import { LanguageVersion } from '../_models/language-version';

export class LanguageVersionMapper {
    public static convertAll(data): LanguageVersion[] {
        if (data == null)
            return;

        let languageVersions = [];
        for (let item of data) {
            let languageVersion = LanguageVersionMapper.convert(item);
            languageVersions.push(languageVersion);
        }

        return languageVersions;
    }

    public static convert(data): LanguageVersion {
        if (data === null || data === undefined)
            return null;

        let languageVersion = new LanguageVersion();
        languageVersion.id = data.id;
        languageVersion.informationMessageId = data.informationMessageId;
        languageVersion.title = data.title;
        languageVersion.message = data.message;
        languageVersion.description = data.description;
        languageVersion.language = <Language>Language[<string>data.languageString];
        languageVersion.sentOnAndroid = data.sentOnAndroid;
        languageVersion.sentOnOsx = data.sentOnOsx;

        return languageVersion;
    }
}
