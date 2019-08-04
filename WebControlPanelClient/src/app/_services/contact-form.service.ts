import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ContactForm } from '../_models/contact-form';
import { HttpClient } from '@angular/common/http';
import { CommonVariables } from '../_config/common-variables';
import { map } from 'rxjs/operators';
import { ContactFormMapper } from '../_mappers/contact-form-mapper';

@Injectable({
    providedIn: 'root'
})
export class ContactFormService {
    constructor(private http: HttpClient) { }

    getAll(): Observable<ContactForm[]> {
        return this.http.get<ContactForm[]>(CommonVariables.ApiUrl + CommonVariables.ApiContactFormsPath).pipe(map(ContactFormMapper.convertAll));
    }

    get(contactFormId: string): Observable<ContactForm> {
        return this.http.get<ContactForm>(CommonVariables.ApiUrl + CommonVariables.ApiContactFormsPath + contactFormId).pipe(map(ContactFormMapper.convert));
    }
}
