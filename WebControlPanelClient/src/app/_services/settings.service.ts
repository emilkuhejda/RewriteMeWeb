import { Injectable } from '@angular/core';
import { RoutingService } from './routing.service';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class SettingsService {
    constructor(
        private routingService: RoutingService,
        private http: HttpClient) { }

    getStorageSetting(): Observable<string> {
        return this.http.get<string>(this.routingService.getStorageSettingUri());
    }

    changeStorage(storageSetting: string): Observable<any> {
        let params = new HttpParams();
        params = params.append('storageSetting', storageSetting);

        return this.http.put(this.routingService.getChangeStorageUri(), null, { params: params });
    }

    cleanUp(): Observable<any> {
        return this.http.put<any>(this.routingService.getCleanUpUri(), {});
    }
}
