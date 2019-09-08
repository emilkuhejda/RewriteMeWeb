import { Language } from '../_enums/language';

export class CommonVariables {
    public static ApplicationId = "ef036280-6a26-421a-a258-c51c8e98b99c";
    public static CurrentIdentity: string = "current.identity";
    public static AccessTokenKey: string = "access.token";
    public static B2CSuccessCallbackToken: string = "b2c.success.callback.token"
    public static ApiUriDevelopment: string = "https://localhost:44357/";
    public static ApiUriProduction: string = "https://rewrite-me.com/";
    public static DefaultLanguage: Language = 1;
}
