import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BillingPurchaseService } from 'src/app/_services/billing-purchase.service';
import { AlertService } from 'src/app/_services/alert.service';
import { ErrorResponse } from 'src/app/_models/error-response';
import { BillingPurchase } from 'src/app/_models/billing-purchase';
import * as $ from 'jquery';
import 'datatables.net';
import 'datatables.net-bs4';

@Component({
    selector: 'app-purchases',
    templateUrl: './purchases.component.html',
    styleUrls: ['./purchases.component.css']
})
export class PurchasesComponent implements OnInit {
    private tableWidget: any;
    billingPurchases: BillingPurchase[];
    userId: string;

    constructor(
        private route: ActivatedRoute,
        private billingPurchaseService: BillingPurchaseService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.route.paramMap.subscribe(paramMap => {
            this.userId = paramMap.get("userId");
            this.billingPurchaseService.getAll(this.userId).subscribe(
                (billingPurchases: BillingPurchase[]) => {
                    this.billingPurchases = billingPurchases.sort((a, b) => {
                        return <any>new Date(b.transactionDateUtc) - <any>new Date(a.transactionDateUtc);
                    });

                    this.reInitDatatable();
                },
                (err: ErrorResponse) => {
                    this.alertService.error(err.message);
                }
            );
        });
    }

    ngAfterViewInit() {
        this.initDatatable();
    }

    private initDatatable(): void {
        let table: any = $('#dataTable');
        this.tableWidget = table.DataTable();
    }

    private reInitDatatable(): void {
        if (this.tableWidget) {
            this.tableWidget.destroy();
            this.tableWidget = null;
        }

        setTimeout(this.initDatatable, 0);
    }
}
