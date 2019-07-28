import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BillingPurchaseService } from 'src/app/_services/billing-purchase.service';
import { AlertService } from 'src/app/_services/alert.service';
import { ErrorResponse } from 'src/app/_models/error-response';
import { BillingPurchase } from 'src/app/_models/billing-purchase';

@Component({
    selector: 'app-purchases',
    templateUrl: './purchases.component.html',
    styleUrls: ['./purchases.component.css']
})
export class PurchasesComponent implements OnInit {
    constructor(
        private route: ActivatedRoute,
        private billingPurchaseService: BillingPurchaseService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.route.paramMap.subscribe(paramMap => {
            let userId = paramMap.get("userId");
            this.billingPurchaseService.getAll(userId).subscribe(
                (billingPurchases: BillingPurchase[]) => { },
                (err: ErrorResponse) => {
                    this.alertService.error(err.message);
                }
            );
        });
    }
}
