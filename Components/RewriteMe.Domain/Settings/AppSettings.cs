﻿using System;

namespace RewriteMe.Domain.Settings
{
    public class AppSettings
    {
        public Guid ApplicationId { get; set; }

        public string ConnectionString { get; set; }

        public Authentication Authentication { get; set; }

        public string GoogleApiAuthUri { get; set; }

        public SpeechCredentials SpeechCredentials { get; set; }
    }
}
