import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { RoutingService } from './routing.service';
import { Observable } from 'rxjs';
import { FileItem } from '../_models/file-item';
import { map } from 'rxjs/operators';
import { FileItemMapper } from '../_mappers/file-item-mapper';
import { RecognitionState } from '../_enums/recognition-state';
import { CommonVariables } from '../_config/common-variables';

@Injectable({
    providedIn: 'root'
})
export class FileItemService {
    constructor(
        private routingService: RoutingService,
        private http: HttpClient) { }

    getAll(userId: string): Observable<FileItem[]> {
        return this.http.get<FileItem[]>(this.routingService.getFileItemsUri() + userId).pipe(map(FileItemMapper.convertAll))
    }

    get(fileItemId: string): Observable<FileItem> {
        return this.http.get<FileItem>(this.routingService.getDetailFileItemsUri() + fileItemId).pipe(map(FileItemMapper.convert))
    }

    restore(userId: string, fileItemId: string) {
        let params = new HttpParams();
        params = params.append('userId', userId);
        params = params.append('fileItemId', fileItemId);
        params = params.append('applicationId', CommonVariables.ApplicationId);

        return this.http.put(this.routingService.getRestoreFileItemUri(), null, { params: params });
    }

    updateRecognitionState(fileItemId: string, recognitionState: RecognitionState): Observable<FileItem> {
        let params = new HttpParams();
        params = params.append('fileItemId', fileItemId);
        params = params.append('recognitionState', recognitionState.toString());
        params = params.append('applicationId', CommonVariables.ApplicationId);

        return this.http.put<FileItem>(this.routingService.getUpdateRecognitionStateUri(), null, { params: params }).pipe(map(FileItemMapper.convert));
    }
}
