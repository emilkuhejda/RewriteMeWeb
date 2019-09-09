import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { RoutingService } from './routing.service';
import { Observable } from 'rxjs';
import { FileItem } from '../_models/file-item';
import { map } from 'rxjs/operators';
import { FileItemMapper } from '../_mappers/file-item-mapper';

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

    restore(userId: string, fileItemId: string) {
        let params = new HttpParams();
        params = params.append('userId', userId);
        params = params.append('fileItemId', fileItemId);

        return this.http.put(this.routingService.getRestoreFileItemUri(), null, { params: params });
    }
}
