import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonVariables } from '../_config/common-variables';

@Injectable({
    providedIn: 'root'
})
export class ContactFormService {
    constructor(private http: HttpClient) { }

    create(data) {
        return this.http.post(CommonVariables.ApiUrl + CommonVariables.ApiCreateContactFormPath, data);
    }
}
