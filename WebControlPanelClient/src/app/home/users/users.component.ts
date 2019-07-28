import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/_service/user.service';
import { User } from 'src/app/_models/user';
import { AlertService } from 'src/app/_services/alert.service';
import { ErrorResponse } from 'src/app/_models/error-response';

@Component({
    selector: 'app-users',
    templateUrl: './users.component.html',
    styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
    users: User[];

    constructor(
        private userService: UserService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.userService.getAll().subscribe(
            users => {
                this.users = users.sort(this.orderUsers);
            },
            (err: ErrorResponse) => {
                this.alertService.error(err.message);
            });
    }

    private orderUsers(a: User, b: User): number {
        if (a.email > b.email)
            return 1;

        if (b.email > a.email)
            return -1;

        return 0;
    }
}
