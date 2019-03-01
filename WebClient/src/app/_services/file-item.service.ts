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
		return this.http.get<FileItem[]>(CommonVariables.ApiUrl + CommonVariables.ApiFilesPath).pipe(map(FileItemMapper.convert));
	}

	create(fileData) {
		let uploadRequest = new HttpRequest("POST", CommonVariables.ApiUrl + CommonVariables.ApiCreateFilePath, fileData, {
			reportProgress: true
		});

		return this.http.request(uploadRequest);
	}

	transcribe(fileId: string) {
		return this.http.post(CommonVariables.ApiUrl + CommonVariables.ApiTranscribeFilePath, { fileId: fileId });
	}
}
