import { Injectable, EventEmitter } from '@angular/core';
import { RoutingService } from './routing.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { InformationMessage } from '../_models/information-message';
import { map } from 'rxjs/operators';
import { InformationMessageMapper } from '../_mappers/information-message-mapper';
import { CommonVariables } from '../_config/common-variables';

@Injectable({
    providedIn: 'root'
})
export class InformationMessageService {
    public messageWasOpened: EventEmitter<void> = new EventEmitter();

    private openedMessages: string[] = [];

    constructor(
        private routingService: RoutingService,
        private http: HttpClient) {
        this.initializeOpenedMessages();
    }

    private initializeOpenedMessages(): string[] {
        var values = localStorage.getItem(CommonVariables.OpenedMessagesKey);
        if (values == null) {
            return;
        }

        this.openedMessages = JSON.parse(values);
    }

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

    markAsOpened(informationMessageId: string): Observable<InformationMessage> {
        let params = new HttpParams();
        params = params.append('informationMessageId', informationMessageId);
        return this.http.put<InformationMessage>(this.routingService.getMarkMessageAsOpenedUri(), null, { params: params }).pipe(map(InformationMessageMapper.convert));
    }

    markAsOpenedLocally(informationMessageId: string) {
        if (this.openedMessages.some(x => x == informationMessageId)) {
            return;
        }

        this.openedMessages.push(informationMessageId);
        localStorage.setItem(CommonVariables.OpenedMessagesKey, JSON.stringify(this.openedMessages));

        this.messageWasOpened.emit();
    }

    updateWasOpenedProperty(informationMessages: InformationMessage[]) {
        let informationMessage = new InformationMessage();
        for (informationMessage of informationMessages) {
            if (informationMessage.isUserSpecific) continue;

            var dateToCompare = new Date();
            dateToCompare.setDate(dateToCompare.getDate() - 7);

            informationMessage.wasOpened = informationMessage.datePublished < dateToCompare || this.openedMessages.some(x => x == informationMessage.id);
        }
    }
}
