import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest } from '@angular/common/http';
import { CommonVariables } from '../_config/common-variables';
import { FileItem } from '../_models/file-item';
import { map } from 'rxjs/operators';
import { FileItemMapper } from '../_mappers/file-item-mapper';

@Injectable({
	providedIn: 'root'
})
export class FileItemService {
	constructor(private http: HttpClient) { }

	getAll() {
		return this.http.get<FileItem[]>(CommonVariables.ApiUrl + CommonVariables.ApiFileItemsPath).pipe(map(FileItemMapper.convertAll));
	}

	get(fileItemId: string) {
		return this.http.get<FileItem>(CommonVariables.ApiUrl + CommonVariables.ApiFileItemPath + fileItemId).pipe(map(FileItemMapper.convert));
	}

	create(formData) {
		let uploadRequest = new HttpRequest("POST", CommonVariables.ApiUrl + CommonVariables.ApiCreateFileItemPath, formData, {
			reportProgress: true
		});

		return this.http.request(uploadRequest);
	}

	update(formData) {
		let uploadRequest = new HttpRequest("PUT", CommonVariables.ApiUrl + CommonVariables.ApiUpdateFileItemPath, formData, {
			reportProgress: true
		});

		return this.http.request(uploadRequest);
	}

	transcribe(fileItemId: string) {
		return this.http.post(CommonVariables.ApiUrl + CommonVariables.ApiTranscribeFileItemPath, { fileItemId: fileItemId });
	}

	remove(fileItemId: string) {
		return this.http.delete(CommonVariables.ApiUrl + CommonVariables.ApiRemoveFileItemPath + fileItemId);
	}
}
