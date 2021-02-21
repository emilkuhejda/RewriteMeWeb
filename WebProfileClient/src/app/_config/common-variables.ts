import { Language } from '../_enums/language';

export class CommonVariables {
    public static ApplicationId = "ef036280-6a26-421a-a258-c51c8e98b99c";
    public static CurrentIdentity: string = "current.identity";
    public static OpenedMessagesKey: string = "opened.messages";
    public static AccessTokenKey: string = "access.token";
    public static B2CSuccessCallbackToken: string = "b2c.success.callback.token"
    public static ApiUriDevelopment: string = "https://localhost:5001/";
    public static ApiUriProduction: string = "https://voicipher.com/";
    public static DefaultLanguage: Language = 1;
}
