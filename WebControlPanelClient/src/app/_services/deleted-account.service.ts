import { Injectable } from '@angular/core';
import { RoutingService } from './routing.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { DeletedAccount } from '../_models/deleted-account';
import { DeletedAccountMapper } from '../_mappers/deleted-account-mapper';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class DeletedAccountService {
    constructor(
        private routingService: RoutingService,
        private http: HttpClient) { }

    getAll(): Observable<DeletedAccount[]> {
        return this.http.get<DeletedAccount[]>(this.routingService.getDeletedAccountsUri()).pipe(map(DeletedAccountMapper.convertAll));
    }

    delete(deletedAccountId: string) {
        let params = new HttpParams();
        params = params.append('deletedAccountId', deletedAccountId);

        return this.http.delete(this.routingService.getDeletedAccountsUri() + deletedAccountId);
    }
}
