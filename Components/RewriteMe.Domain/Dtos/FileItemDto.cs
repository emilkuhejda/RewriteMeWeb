﻿using System;
using System.ComponentModel.DataAnnotations;
using RewriteMe.Domain.Enums;

namespace RewriteMe.Domain.Dtos
{
    public class FileItemDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string FileName { get; set; }

        public string Language { get; set; }

        [Required]
        public string RecognitionStateString { get; set; }

        [Required]
        public UploadStatus UploadStatus { get; set; }

        [Required]
        public long TotalTimeTicks { get; set; }

        [Required]
        public long TranscribedTimeTicks { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime? DateProcessedUtc { get; set; }

        [Required]
        public DateTime DateUpdatedUtc { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
