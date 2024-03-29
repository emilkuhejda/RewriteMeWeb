import { Injectable } from '@angular/core';
import { RoutingService } from './routing.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { InformationMessage } from '../_models/information-message';
import { InformationMessageMapper } from '../_mappers/information-message-mapper';

@Injectable({
    providedIn: 'root'
})
export class InformationMessageService {
    constructor(
        private routingService: RoutingService,
        private http: HttpClient) { }

    getAll(): Observable<InformationMessage[]> {
        return this.http.get<InformationMessage[]>(this.routingService.getInformationMessagesUri()).pipe(map(InformationMessageMapper.convertAll));
    }

    get(informationMessageId: string): Observable<InformationMessage> {
        return this.http.get<InformationMessage>(this.routingService.getInformationMessagesUri() + informationMessageId).pipe(map(InformationMessageMapper.convert));
    }

    create(formData: FormData) {
        return this.http.post(this.routingService.getCreateInformationMessageUri(), formData);
    }

    update(informationMessageId: string, formData: FormData) {
        return this.http.put(this.routingService.getInformationMessagesUri() + informationMessageId, formData);
    }

    sendNotification(formData: FormData) {
        return this.http.put(this.routingService.getSendNotificationUri(), formData).pipe(map(InformationMessageMapper.convert));
    }
}
