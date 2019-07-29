import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonVariables } from '../_config/common-variables';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { BillingPurchase } from '../_models/billing-purchase';
import { BillingPurchaseMapper } from '../_mappers/billing-purchase-mapper';

@Injectable({
    providedIn: 'root'
})
export class BillingPurchaseService {
    constructor(private http: HttpClient) { }

    getAll(userId: string): Observable<BillingPurchase[]> {
        return this.http.get<BillingPurchase[]>(CommonVariables.ApiUrl + CommonVariables.ApiPurchasesPath + userId).pipe(map(BillingPurchaseMapper.convertAll));
    }
}
