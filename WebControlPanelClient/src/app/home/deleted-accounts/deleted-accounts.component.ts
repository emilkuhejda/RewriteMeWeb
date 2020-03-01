import { Component, OnInit } from '@angular/core';
import { DeletedAccountService } from 'src/app/_services/deleted-account.service';
import { DeletedAccount } from 'src/app/_models/deleted-account';
import { GecoDialog } from 'angular-dynamic-dialog';
import { AlertService } from 'src/app/_services/alert.service';
import { DialogComponent } from 'src/app/_directives/dialog/dialog.component';
import { ErrorResponse } from 'src/app/_models/error-response';

@Component({
    selector: 'app-deleted-accounts',
    templateUrl: './deleted-accounts.component.html',
    styleUrls: ['./deleted-accounts.component.css']
})
export class DeletedAccountsComponent implements OnInit {
    deletedAccounts: DeletedAccount[];

    constructor(
        private deletedAccountService: DeletedAccountService,
        private alertService: AlertService,
        private modal: GecoDialog) { }

    ngOnInit() {
        this.initialize();
    }

    initialize(): void {
        this.deletedAccountService.getAll().subscribe(accounts => {
            this.deletedAccounts = accounts;
        });
    }

    delete(deletedAccount: DeletedAccount) {
        this.alertService.clear();
        let onAccept = (dialogComponent: DialogComponent) => {
            this.deletedAccountService.delete(deletedAccount.id).subscribe(
                () => {
                    this.initialize();
                },
                (err: ErrorResponse) => {
                    this.alertService.error(err.message);
                })
                .add(() => dialogComponent.close());
        };

        let data = {
            title: `Delete user`,
            message: `Do you really want to delete user '${deletedAccount.userId}'?`,
            onAccept: onAccept
        };

        let modal = this.modal.openDialog(DialogComponent, {
            data: data,
            useStyles: 'none'
        });

        modal.onClosedModal().subscribe();
    }
}
