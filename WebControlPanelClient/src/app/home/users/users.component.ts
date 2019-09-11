import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { AlertService } from 'src/app/_services/alert.service';
import { ErrorResponse } from 'src/app/_models/error-response';
import { UserService } from 'src/app/_services/user.service';
import { GecoDialog } from 'angular-dynamic-dialog';
import { ConfirmationDialogComponent } from 'src/app/_directives/confirmation-dialog/confirmation-dialog.component';
import * as $ from 'jquery';
import 'datatables.net';
import 'datatables.net-bs4';

@Component({
    selector: 'app-users',
    templateUrl: './users.component.html',
    styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
    private tableWidget: any;
    users: User[];

    constructor(
        private userService: UserService,
        private alertService: AlertService,
        private modal: GecoDialog) { }

    ngOnInit() {
        this.initializeUsers();
    }

    delete(user: User) {
        this.alertService.clear();
        let onAccept = (dialogComponent: ConfirmationDialogComponent) => {
            if (dialogComponent.confirmationValue !== user.email) {
                this.alertService.error("User email address must be correct.");
                dialogComponent.close();
                return;
            }

            this.userService.delete(user.id, dialogComponent.confirmationValue).subscribe(
                () => {
                    this.initializeUsers();
                },
                (err: ErrorResponse) => {
                    let error = err.message;
                    if (err.status === 400)
                        error = "User was not found.";

                    if (err.status === 404)
                        error = "User was not deleted.";

                    this.alertService.error(error);
                })
                .add(() => dialogComponent.close());
        };

        let data = {
            title: `Delete user`,
            message: `Do you really want to delete user '${user.email}'?`,
            confirmationTitle: `E-mail`,
            onAccept: onAccept
        };

        let modal = this.modal.openDialog(ConfirmationDialogComponent, {
            data: data,
            useStyles: 'none'
        });

        modal.onClosedModal().subscribe();
    }

    ngAfterViewInit() {
        this.initDatatable();
    }

    private initializeUsers(): void {
        this.userService.getAll().subscribe(
            users => {
                this.users = users.sort(this.orderUsers);
                this.reInitDatatable();
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            });
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

    private orderUsers(a: User, b: User): number {
        if (a.email > b.email)
            return 1;

        if (b.email > a.email)
            return -1;

        return 0;
    }
}
