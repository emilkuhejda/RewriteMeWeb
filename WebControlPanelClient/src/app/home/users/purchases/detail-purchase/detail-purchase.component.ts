import { Component, OnInit } from '@angular/core';
import { BillingPurchase } from 'src/app/_models/billing-purchase';
import { BillingPurchaseService } from 'src/app/_services/billing-purchase.service';
import { ActivatedRoute } from '@angular/router';
import { ErrorResponse } from 'src/app/_models/error-response';
import { AlertService } from 'src/app/_services/alert.service';
import { Location } from '@angular/common';

@Component({
    selector: 'app-detail-purchase',
    templateUrl: './detail-purchase.component.html',
    styleUrls: ['./detail-purchase.component.css']
})
export class DetailPurchaseComponent implements OnInit {
    billingPurchase: BillingPurchase;
    transactionDate: string;
    
    constructor(
        private location: Location,
        private route: ActivatedRoute,
        private billingPurchaseService: BillingPurchaseService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.route.paramMap.subscribe(paramMap => {
            let purchaseId = paramMap.get("purchaseId");
            this.billingPurchaseService.get(purchaseId).subscribe(
                (billingPurchase: BillingPurchase) => {
                    this.billingPurchase = billingPurchase;
                    this.transactionDate = new Date(billingPurchase.transactionDateUtc).toUTCString();
                },
                (err: ErrorResponse) => {
                    this.alertService.error(err.message);
                }
            );
        });
    }

    goBack() {
        this.location.back();
    }
}
