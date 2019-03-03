namespace RewriteMe.Domain.Transcription
{
    public class RecognitionAlternative
    {
        public RecognitionAlternative(string transcript, float confidence)
        {
            Transcript = transcript;
            Confidence = confidence;
        }

        public string Transcript { get; }

        public float Confidence { get; }
    }
}
