using System;

namespace RewriteMe.Business.Utils
{
    public static class HubMethodsHelper
    {
        private const string RecognitionProgressChangedMethod = "recognition-progress";
        private const string RecognitionStateChangedMethod = "recognition-state";

        public static string GetRecognitionProgressChangedMethod(Guid userId)
        {
            return $"{RecognitionProgressChangedMethod}-{userId}";
        }

        public static string GetRecognitionStateChangedMethod(Guid userId)
        {
            return $"{RecognitionStateChangedMethod}-{userId}";
        }
    }
}
