﻿namespace RewriteMe.Domain.Settings
{
    public class SpeechCredentials
    {
        public string Type { get; set; }

        public string Project_id { get; set; }

        public string Private_key_id { get; set; }

        public string Private_key { get; set; }

        public string Client_email { get; set; }

        public string Client_id { get; set; }

        public string Auth_uri { get; set; }

        public string Token_uri { get; set; }

        public string Auth_provider_x509_cert_url { get; set; }

        public string Client_x509_cert_url { get; set; }
    }
}
