﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Extensions;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Managers;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Managers
{
    public class SpeechRecognitionManager : ISpeechRecognitionManager
    {
        private readonly ISpeechRecognitionService _speechRecognitionService;
        private readonly IFileItemService _fileItemService;
        private readonly ITranscribeItemService _transcribeItemService;
        private readonly IWavFileService _wavFileService;

        public SpeechRecognitionManager(
            ISpeechRecognitionService speechRecognitionService,
            IFileItemService fileItemService,
            ITranscribeItemService transcribeItemService,
            IWavFileService wavFileService)
        {
            _speechRecognitionService = speechRecognitionService;
            _fileItemService = fileItemService;
            _transcribeItemService = transcribeItemService;
            _wavFileService = wavFileService;
        }

        public async Task RunRecognition(FileItem fileItem)
        {
            if (!fileItem.IsSupportedType())
                throw new InvalidOperationException("File type is not supported");

            await _fileItemService.UpdateRecognitionStateAsync(fileItem.Id, RecognitionState.InProgress).ConfigureAwait(false);

            var audioSource = fileItem.IsWav()
                ? fileItem.Source
                : await _wavFileService.ConvertToWav(fileItem.Source).ConfigureAwait(false);

            var wavFiles = await _wavFileService.SplitWavFile(audioSource).ConfigureAwait(false);
            var files = wavFiles.ToList();

            var transcribeItems = await _speechRecognitionService.Recognize(fileItem, files).ConfigureAwait(false);
            await _transcribeItemService.AddAsync(transcribeItems).ConfigureAwait(false);

            await _fileItemService.UpdateRecognitionStateAsync(fileItem.Id, RecognitionState.Completed).ConfigureAwait(false);

            DeleteTempFiles(files);
        }

        private void DeleteTempFiles(IEnumerable<WavPartialFile> files)
        {
            foreach (var file in files)
            {
                if (File.Exists(file.Path))
                    File.Delete(file.Path);
            }
        }
    }
}
