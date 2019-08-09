import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Administrator } from '../_models/administrator';
import { map } from 'rxjs/operators';
import { AdministratorMapper } from '../_mappers/administrator-mapper';
import { RoutingService } from './routing.service';

@Injectable({
    providedIn: 'root'
})
export class AdministratorService {
    constructor(
        private routingService: RoutingService,
        private http: HttpClient) { }

    getAll(): Observable<Administrator[]> {
        return this.http.get<Administrator[]>(this.routingService.getAdministratorsUri()).pipe(map(AdministratorMapper.convertAll));
    }

    get(administratorId: string) {
        return this.http.get<Administrator>(this.routingService.getAdministratorsUri() + administratorId).pipe(map(AdministratorMapper.convert));
    }

    create(formData: any) {
        return this.http.post(this.routingService.getCreateAdministratorUri(), formData);
    }

    update(formData: any) {
        return this.http.put(this.routingService.getUpdateAdministratorUri(), formData);
    }

    delete(administratorId: string) {
        let params = new HttpParams();
        params = params.append('administratorId', administratorId);

        return this.http.delete(this.routingService.getDeleteAdministratorUri(), { params: params });
    }
}
