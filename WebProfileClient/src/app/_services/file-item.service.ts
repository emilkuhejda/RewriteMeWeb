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
        return this.http.get<FileItem[]>(CommonVariables.ApiUrl + CommonVariables.ApiFileItemsPath).pipe(map(FileItemMapper.convertAll));
    }

    upload(formData: FormData, params: HttpParams) {
        let uploadRequest = new HttpRequest("POST", CommonVariables.ApiUrl + CommonVariables.ApiUploadFileItemPath, formData, {
            params: params,
            reportProgress: true
        });

        return this.http.request(uploadRequest);
    }

    update(formData: FormData) {
        formData.append("applicationId", CommonVariables.ApplicationId);

        return this.http.put(CommonVariables.ApiUrl + CommonVariables.ApiUpdateFileItemPath, formData);
    }

    delete(fileItemId: string) {
        let params = new HttpParams();
        params = params.append('fileItemId', fileItemId);
        params = params.append('applicationId', CommonVariables.ApplicationId);
        return this.http.delete(CommonVariables.ApiUrl + CommonVariables.ApiDeleteFileItemPath, { params: params });
    }

    transcribe(fileItemId: string, language: string) {
        let params = new HttpParams();
        params = params.append('fileItemId', fileItemId);
        params = params.append('language', language);
        params = params.append('applicationId', CommonVariables.ApplicationId);
        return this.http.post(CommonVariables.ApiUrl + CommonVariables.ApiTranscribeFileItemPath, null, { params: params });
    }
}
