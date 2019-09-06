import { LanguageVersion } from '../_models/language-version';
import { Language } from '../_enums/language';

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
        let languageVersion = new LanguageVersion();
        languageVersion.id = data.id;
        languageVersion.informationMessageId = data.informationMessageId;
        languageVersion.title = data.title;
        languageVersion.message = data.message;
        languageVersion.description = data.description;
        languageVersion.language = <Language>data.language;
        languageVersion.sentOnAndroid = data.sentOnAndroid;
        languageVersion.sentOnOsx = data.sentOnOsx;

        return languageVersion;
    }
}
