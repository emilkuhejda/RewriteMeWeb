import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpRequest } from '@angular/common/http';
import { CommonVariables } from '../_config/common-variables';
import { FileItem } from '../_models/file-item';
import { map } from 'rxjs/operators';
import { FileItemMapper } from '../_mappers/file-item-mapper';

@Injectable({
    providedIn: 'root'
})
export class FileItemService {
    constructor(private http: HttpClient) { }

    get(fileItemId: string) {
        return this.http.get<FileItem>(CommonVariables.ApiUrl + CommonVariables.ApiFileItemsPath + fileItemId).pipe(map(FileItemMapper.convert));
    }

    getAll() {
        let params = new HttpParams();
        params = params.append('applicationId', CommonVariables.ApplicationId);
        return this.http.get<FileItem[]>(CommonVariables.ApiUrl + CommonVariables.ApiFileItemsPath, { params: params }).pipe(map(FileItemMapper.convertAll));
    }

    upload(formData) {
        let uploadRequest = new HttpRequest("POST", CommonVariables.ApiUrl + CommonVariables.ApiUpdateTranscriptPath, formData, {
            reportProgress: true
        });

        return this.http.request(uploadRequest);
    }

    delete(fileItemId: string) {
        let params = new HttpParams();
        params = params.append('fileItemId', fileItemId);
        params = params.append('applicationId', CommonVariables.ApplicationId);
        return this.http.delete(CommonVariables.ApiUrl + CommonVariables.ApiDeleteFileItemPath, { params: params });
    }
}
