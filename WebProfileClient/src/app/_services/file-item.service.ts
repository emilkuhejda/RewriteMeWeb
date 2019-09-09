import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpRequest } from '@angular/common/http';
import { CommonVariables } from '../_config/common-variables';
import { FileItem } from '../_models/file-item';
import { map } from 'rxjs/operators';
import { FileItemMapper } from '../_mappers/file-item-mapper';
import { RoutingService } from './routing.service';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class FileItemService {
    constructor(
        private routingService: RoutingService,
        private http: HttpClient) { }

    get(fileItemId: string): Observable<FileItemMapper> {
        return this.http.get<FileItem>(this.routingService.getFileItemsUri() + fileItemId).pipe(map(FileItemMapper.convert));
    }

    getAll(): Observable<FileItemMapper[]> {
        return this.http.get<FileItem[]>(this.routingService.getFileItemsUri()).pipe(map(FileItemMapper.convertAll));
    }

    getDeletedFileItems(): Observable<FileItemMapper[]> {
        return this.http.get<FileItem[]>(this.routingService.getTemporaryDeletedFileItemsUri()).pipe(map(FileItemMapper.convertAll));
    }

    upload(formData: FormData, params: HttpParams) {
        let uploadRequest = new HttpRequest("POST", this.routingService.getUploadFileItemUri(), formData, {
            params: params,
            reportProgress: true
        });

        return this.http.request(uploadRequest);
    }

    update(formData: FormData) {
        formData.append("applicationId", CommonVariables.ApplicationId);

        return this.http.put(this.routingService.getUpdateFileItemUri(), formData);
    }

    delete(fileItemId: string) {
        let params = new HttpParams();
        params = params.append('fileItemId', fileItemId);
        params = params.append('applicationId', CommonVariables.ApplicationId);
        return this.http.delete(this.routingService.getDeleteFileItemUri(), { params: params });
    }

    transcribe(fileItemId: string, language: string) {
        let params = new HttpParams();
        params = params.append('fileItemId', fileItemId);
        params = params.append('language', language);
        params = params.append('applicationId', CommonVariables.ApplicationId);
        return this.http.put(this.routingService.getTranscribeFileItemUri(), null, { params: params });
    }
}
