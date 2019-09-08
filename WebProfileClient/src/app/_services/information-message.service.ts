import { Injectable } from '@angular/core';
import { RoutingService } from './routing.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { InformationMessage } from '../_models/information-message';
import { map } from 'rxjs/operators';
import { InformationMessageMapper } from '../_mappers/information-message-mapper';

@Injectable({
    providedIn: 'root'
})
export class InformationMessageService {
    constructor(
        private routingService: RoutingService,
        private http: HttpClient) { }

    getAll(count: number): Observable<InformationMessage[]> {
        let params = new HttpParams();
        if (count !== undefined) {
            params = params.append('count', count.toString());
        }

        return this.http.get<InformationMessage[]>(this.routingService.getInformationMessagesUri(), { params: params }).pipe(map(InformationMessageMapper.convertAll));
    }

    get(informationMessageId: string): Observable<InformationMessage> {
        return this.http.get<InformationMessage>(this.routingService.getInformationMessagesUri() + informationMessageId).pipe(map(InformationMessageMapper.convert));
    }
}
