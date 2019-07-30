import { Component, OnInit } from '@angular/core';
import { Administrator } from 'src/app/_models/administrator';
import { AdministratorService } from 'src/app/_services/administrator.service';
import { AlertService } from 'src/app/_services/alert.service';
import { ErrorResponse } from 'src/app/_models/error-response';
import { GecoDialog } from 'angular-dynamic-dialog';
import { DialogComponent } from 'src/app/_directives/dialog/dialog.component';

@Component({
    selector: 'app-administrators',
    templateUrl: './administrators.component.html',
    styleUrls: ['./administrators.component.css']
})
export class AdministratorsComponent implements OnInit {
    administrators: Administrator[];

    constructor(
        private administratorService: AdministratorService,
        private alertService: AlertService,
        private modal: GecoDialog) { }

    ngOnInit() {
        this.initialize();
    }

    delete(administrator: Administrator) {
        this.alertService.clear();
        let onAccept = (dialogComponent: DialogComponent) => {
            this.administratorService.delete(administrator.id)
                .subscribe(
                    () => {
                        this.alertService.success(`The file '${administrator.username}' was successfully deleted`);
                        this.initialize();
                    },
                    (err: ErrorResponse) => {
                        this.alertService.error(err.message);
                    })
                .add(() => {
                    dialogComponent.close();
                });
        };

        let data = {
            title: `Delete ${administrator.username}`,
            message: `Do you really want to delete file '${administrator.username}'?`,
            onAccept: onAccept
        };

        let modal = this.modal.openDialog(DialogComponent, {
            data: data,
            useStyles: 'none'
        });

        modal.onClosedModal().subscribe();
    }

    private initialize() {
        this.administratorService.getAll().subscribe(
            (administrators: Administrator[]) => {
                this.administrators = administrators.sort(this.orderAdministrators);
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            });
    }

    private orderAdministrators(a: Administrator, b: Administrator): number {
        if (a.username > b.username)
            return 1;

        if (b.username > a.username)
            return -1;

        return 0;
    }
}
