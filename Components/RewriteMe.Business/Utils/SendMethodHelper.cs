using System;

namespace RewriteMe.Business.Utils
{
    public static class SendMethodHelper
    {
        private const string RecognitionProgressMethod = "recognition-progress";
        private const string RecognitionStateMethod = "recognition-state";

        public static string GetRecognitionProgressMethod(Guid userId)
        {
            return $"{RecognitionProgressMethod}-{userId}";
        }

        public static string GetRecognitionStateMethod(Guid userId)
        {
            return $"{RecognitionStateMethod}-{userId}";
        }
    }
}
