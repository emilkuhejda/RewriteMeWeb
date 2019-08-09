import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TranscribeItem } from '../_models/transcribe-item';
import { TranscribeItemMapper } from '../_mappers/transcribe-item-mapper';
import { map } from 'rxjs/operators';
import { RoutingService } from '../_service/routing.service';

@Injectable({
    providedIn: 'root'
})
export class TranscribeItemService {
    constructor(
        private routingService: RoutingService,
        private http: HttpClient) { }

    getAll(fileItemId: string) {
        return this.http.get<TranscribeItem[]>(this.routingService.getTranscribeItemsUri() + fileItemId).pipe(map(TranscribeItemMapper.convert));
    }

    getAudio(transcribeItemId: string) {
        return this.http.get(this.routingService.getTranscribeAudioUri() + transcribeItemId, { responseType: 'blob' });
    }

    updateTranscript(formData) {
        return this.http.put(this.routingService.getUpdateTranscriptUri(), formData);
    }
}
