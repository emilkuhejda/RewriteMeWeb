import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TranscribeItem } from '../_models/transcribe-item';
import { CommonVariables } from '../_config/common-variables';
import { TranscribeItemMapper } from '../_mappers/transcribe-item-mapper';
import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class TranscribeItemService {
    constructor(private http: HttpClient) { }

    getAll(fileItemId: string) {
        return this.http.get<TranscribeItem[]>(CommonVariables.ApiUrl + CommonVariables.ApiTranscribeItemsPath + fileItemId).pipe(map(TranscribeItemMapper.convert));
    }

    updateTranscript(formData) {
        return this.http.put(CommonVariables.ApiUrl + CommonVariables.ApiUpdateTranscriptPath, formData);
    }
}
