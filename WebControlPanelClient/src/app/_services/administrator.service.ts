import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Administrator } from '../_models/administrator';
import { CommonVariables } from '../_config/common-variables';
import { map } from 'rxjs/operators';
import { AdministratorMapper } from '../_mappers/administrator-mapper';

@Injectable({
    providedIn: 'root'
})
export class AdministratorService {
    constructor(private http: HttpClient) { }

    getAll(): Observable<Administrator[]> {
        return this.http.get<Administrator[]>(CommonVariables.ApiUrl + CommonVariables.ApiAdministratorsPath).pipe(map(AdministratorMapper.convertAll));
    }

    create(formData: FormData) {
        return this.http.post(CommonVariables.ApiUrl + CommonVariables.ApiCreateAdministratorPath, formData);
    }

    update(formData: FormData) {
        return this.http.put(CommonVariables.ApiUrl + CommonVariables.ApiUpdateAdministratorPath, formData);
    }
}