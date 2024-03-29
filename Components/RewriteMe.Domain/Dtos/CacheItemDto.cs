﻿using System;
using RewriteMe.Domain.Enums;

namespace RewriteMe.Domain.Dtos
{
    public class CacheItemDto
    {
        public Guid FileItemId { get; set; }

        public RecognitionState RecognitionState { get; set; }

        public double PercentageDone { get; set; }
    }
}
