export class LanguageHelper {
    private static supportedLanguages: string[] = ['en-GB', 'en-US', 'ru-RU'];

    public static isPhoneCallModelSupported(language: string) {
        var index = this.supportedLanguages.findIndex(x => x === language);
        return index >= 0;
    }
}