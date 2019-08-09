import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ContactForm } from '../_models/contact-form';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { ContactFormMapper } from '../_mappers/contact-form-mapper';
import { RoutingService } from './routing.service';

@Injectable({
    providedIn: 'root'
})
export class ContactFormService {
    constructor(
        private routingService: RoutingService,
        private http: HttpClient) { }

    getAll(): Observable<ContactForm[]> {
        return this.http.get<ContactForm[]>(this.routingService.getContactFormsUri()).pipe(map(ContactFormMapper.convertAll));
    }

    get(contactFormId: string): Observable<ContactForm> {
        return this.http.get<ContactForm>(this.routingService.getContactFormsUri() + contactFormId).pipe(map(ContactFormMapper.convert));
    }
}
