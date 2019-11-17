import { InformationMessage } from '../_models/information-message';

import { LanguageVersionMapper } from './language-version-mapper';

export class InformationMessageMapper {
    public static convertAll(data): InformationMessage[] {
        let informationMessages = [];
        for (let item of data) {
            let informationMessage = InformationMessageMapper.convert(item);
            informationMessages.push(informationMessage);
        }

        return informationMessages;
    }

    public static convert(data): InformationMessage {
        if (data === null || data === undefined)
            return null;

        let informationMessage = new InformationMessage();
        informationMessage.id = data.id;
        informationMessage.isUserSpecific = data.isUserSpecific;
        informationMessage.wasOpened = data.wasOpened;
        informationMessage.dateUpdated = new Date(data.dateUpdatedUtc);
        informationMessage.datePublished = new Date(data.datePublishedUtc);
        informationMessage.languageVersions = LanguageVersionMapper.convertAll(data.languageVersions);

        return informationMessage;
    }
}
