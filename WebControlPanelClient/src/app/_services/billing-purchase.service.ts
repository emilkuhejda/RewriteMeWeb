import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { BillingPurchase } from '../_models/billing-purchase';
import { BillingPurchaseMapper } from '../_mappers/billing-purchase-mapper';
import { RoutingService } from './routing.service';

@Injectable({
    providedIn: 'root'
})
export class BillingPurchaseService {
    constructor(
        private routingService: RoutingService,
        private http: HttpClient) { }

    getAll(userId: string): Observable<BillingPurchase[]> {
        return this.http.get<BillingPurchase[]>(this.routingService.getPurchasesUri() + userId).pipe(map(BillingPurchaseMapper.convertAll));
    }

    get(purchaseId: string): Observable<BillingPurchase> {
        return this.http.get<BillingPurchase>(this.routingService.getPurchaseUri() + purchaseId).pipe(map(BillingPurchaseMapper.convert));
    }
}
