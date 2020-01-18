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

    getDatabaseBackupSetting(): Observable<boolean> {
        return this.http.get<boolean>(this.routingService.getDatabaseBackupSettingUri());
    }

    getNotificationsSetting(): Observable<boolean> {
        return this.http.get<boolean>(this.routingService.getNotificationsSettingUri());
    }

    changeStorage(storageSetting: string): Observable<any> {
        let params = new HttpParams();
        params = params.append('storageSetting', storageSetting);

        return this.http.put(this.routingService.getChangeStorageUri(), null, { params: params });
    }

    changeDatabaseBackupSettings(isEnabled: boolean): Observable<any> {
        let params = new HttpParams();
        params = params.append('isEnabled', String(isEnabled));

        return this.http.put(this.routingService.getChangeDatabaseBackupSettingsUri(), null, { params: params });
    }

    changeNotificationsSetting(isEnabled: boolean): Observable<any> {
        let params = new HttpParams();
        params = params.append('isEnabled', String(isEnabled));

        return this.http.put(this.routingService.getChangeNotificationsSettingUri(), null, { params: params });
    }

    cleanUp(data: any): Observable<any> {
        return this.http.put<any>(this.routingService.getCleanUpUri(), data);
    }

    cleanOutdatedChunks(): Observable<any> {
        return this.http.delete<any>(this.routingService.getCleanOutdatedChunksUri());
    }
}
