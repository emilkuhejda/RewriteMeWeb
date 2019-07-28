import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AlertService } from 'src/app/_services/alert.service';
import { ErrorResponse } from 'src/app/_models/error-response';
import { SubscriptionsService } from 'src/app/_services/subscriptions.service';
import { Observable } from 'rxjs';
import { UserSubscription } from 'src/app/_models/user-subscription';

@Component({
    selector: 'app-subscriptions',
    templateUrl: './subscriptions.component.html',
    styleUrls: ['./subscriptions.component.css']
})
export class SubscriptionsComponent implements OnInit {
    constructor(
        private route: ActivatedRoute,
        private subscriptionsService: SubscriptionsService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.route.paramMap.subscribe(paramMap => {
            let userId = paramMap.get("userId");
            this.subscriptionsService.getAll(userId).subscribe(
                (userSubscription: UserSubscription[]) => {
                    console.log(userSubscription);
                },
                (err: ErrorResponse) => {
                    this.alertService.error(err.message);
                }
            );
        });
    }
}
