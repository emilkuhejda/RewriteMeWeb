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
        let informationMessage = new InformationMessage();
        informationMessage.id = data.id;
        informationMessage.wasOpened = data.wasOpened;
        informationMessage.dateUpdated = data.dateUpdated;
        informationMessage.datePublished = data.dateCreated;
        informationMessage.languageVersions = LanguageVersionMapper.convertAll(data.languageVersions);

        return informationMessage;
    }
}
