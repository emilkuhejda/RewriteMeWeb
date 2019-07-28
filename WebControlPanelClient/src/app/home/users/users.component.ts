import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/_service/user.service';
import { User } from 'src/app/_models/user';
import { AlertService } from 'src/app/_services/alert.service';
import { ErrorResponse } from 'src/app/_models/error-response';
import * as $ from 'jquery';
import 'datatables.net';
import 'datatables.net-bs4';

@Component({
    selector: 'app-users',
    templateUrl: './users.component.html',
    styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
    users: User[];
    private tableWidget: any;

    constructor(
        private userService: UserService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.userService.getAll().subscribe(
            users => {
                this.users = users.sort(this.orderUsers);
                this.reInitDatatable();
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
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

    private orderUsers(a: User, b: User): number {
        if (a.email > b.email)
            return 1;

        if (b.email > a.email)
            return -1;

        return 0;
    }
}
