import { Component, OnInit } from '@angular/core';
import { UserService } from '../_services/user.service';
import { UserSubscriptionService } from '../_services/user-subscription.service';
import { AlertService } from '../_services/alert.service';
import { MsalService } from '../_services/msal.service';
import { ErrorResponse } from '../_models/error-response';
import { ActivatedRoute } from '@angular/router';
import { Identity } from '../_models/identity';

@Component({
    selector: 'app-account',
    templateUrl: './account.component.html',
    styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {
    remainingTime: string;

    constructor(
        private route: ActivatedRoute,
        private userService: UserService,
        private userSubscriptionService: UserSubscriptionService,
        private msalService: MsalService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.route.fragment.subscribe((fragment: string) => {
            let response = new URLSearchParams(fragment).get('state');
            if (response !== null)
                return;

            this.userSubscriptionService.getSubscriptionRemainingTimeUri().subscribe(
                (remainingTime) => {
                    this.remainingTime = remainingTime;
                }
            );

            let callbackToken = this.msalService.getCallbackToken();
            if (callbackToken != null) {
                this.updateUserData();
            }
        });
    }

    private updateUserData(): void {
        let updateData = {
            givenName: this.msalService.getMsalGivenName(),
            familyName: this.msalService.getMsalFamilyName()
        };

        this.userService.updateUser(updateData).subscribe(
            (identity: Identity) => {
                this.msalService.saveCurrentIdentity(identity);
            },
            (error: ErrorResponse) => {
                this.alertService.error(error.message);
            }
        );
    }

    editProfile(): void {
        this.msalService.editProfile();
    }

    resetPassword(): void {
        this.msalService.resetPassword();
    }
}
