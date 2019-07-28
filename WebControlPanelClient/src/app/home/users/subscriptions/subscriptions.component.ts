import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AlertService } from 'src/app/_services/alert.service';
import { ErrorResponse } from 'src/app/_models/error-response';
import { SubscriptionsService } from 'src/app/_services/subscriptions.service';
import { UserSubscription } from 'src/app/_models/user-subscription';
import { GecoDialog } from 'angular-dynamic-dialog';
import { CreateSubscriptionDialogComponent } from 'src/app/_directives/create-subscription-dialog/create-subscription-dialog.component';
import * as $ from 'jquery';
import 'datatables.net';
import 'datatables.net-bs4';

@Component({
    selector: 'app-subscriptions',
    templateUrl: './subscriptions.component.html',
    styleUrls: ['./subscriptions.component.css']
})
export class SubscriptionsComponent implements OnInit {
    private tableWidget: any;
    userSubscriptions: UserSubscription[];

    constructor(
        private route: ActivatedRoute,
        private subscriptionsService: SubscriptionsService,
        private alertService: AlertService,
        private modal: GecoDialog) { }

    ngOnInit() {
        this.route.paramMap.subscribe(paramMap => {
            let userId = paramMap.get("userId");
            this.subscriptionsService.getAll(userId).subscribe(
                (userSubscriptions: UserSubscription[]) => {
                    this.userSubscriptions = userSubscriptions.sort((a, b) => {
                        return <any>new Date(b.dateCreated) - <any>new Date(a.dateCreated);
                    });

                    this.reInitDatatable();
                },
                (err: ErrorResponse) => {
                    this.alertService.error(err.message);
                }
            );
        });
    }

    create() {
        this.alertService.clear();
        let onAccept = (dialogComponent: CreateSubscriptionDialogComponent) => {
            dialogComponent.close();
        };

        let modal = this.modal.openDialog(CreateSubscriptionDialogComponent, {
            data: onAccept,
            useStyles: 'none'
        });

        modal.onClosedModal().subscribe();
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
