import { Component, OnInit } from '@angular/core';
import { UserService } from '../_services/user.service';
import { UserSubscriptionService } from '../_services/user-subscription.service';
import { AlertService } from '../_services/alert.service';
import { MsalService } from '../_services/msal.service';
import { ErrorResponse } from '../_models/error-response';

@Component({
    selector: 'app-account',
    templateUrl: './account.component.html',
    styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {
    constructor(
        private userService: UserService,
        private userSubscriptionService: UserSubscriptionService,
        private msalService: MsalService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.userSubscriptionService.getSubscriptionRemainingTimeUri().subscribe(
            (data) => {
                console.log(data);
            },
            (error: ErrorResponse) => {
                this.alertService.error(error.message);
            }
        )
    }
}
